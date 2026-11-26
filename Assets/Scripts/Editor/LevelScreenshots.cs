using System;
using System.IO;
using DisasterSafety.Core;
using DisasterSafety.Game;
using DisasterSafety.Game.UI;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DisasterSafety.EditorTools
{
    /// <summary>
    /// Renders the Level scene's screens (the Student's Act-outs, Feedback, Pause, Level
    /// Completed) to PNG without entering Play mode, filled with authored Earthquake Scenarios.
    /// Runs as part of MenuScreenshots.Capture. For layout review only.
    /// </summary>
    public static class LevelScreenshots
    {
        public static void Capture(string dir)
        {
            EditorSceneManager.OpenScene(ProjectBuilder.LevelScenePath, OpenSceneMode.Single);
            var c = UnityEngine.Object.FindAnyObjectByType<LevelController>();
            var canvas = UnityEngine.Object.FindAnyObjectByType<Canvas>();
            var cam = Camera.main;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cam;
            canvas.planeDistance = 5f;

            var scenarios = GameServices.LoadScenarios();
            var all = new[] { c.dimmer, c.feedbackPanel, c.completedPanel, c.pausePanel, c.actionBar.panel };

            void Shot(string name, params ScreenPanel[] show)
            {
                foreach (var p in all) p.gameObject.SetActive(Array.IndexOf(show, p) >= 0);
                foreach (var size in MenuScreenshots.Sizes)
                    MenuScreenshots.Render(cam, canvas, size, Path.Combine(dir, name + "-" + size.x + "x" + size.y + ".png"));
            }

            // Easy 1: Pause over the Action Bar, then Level Completed with the Display Name.
            var s = StartStudentLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Easy, 1), StudentKind.Boy);
            Shot("14-pause", c.actionBar.panel, c.dimmer, c.pausePanel);
            CompletedWithStudent(c, s, 2, UIStrings.LevelCompletedHeadline("Niño"));
            Shot("15-level-completed", c.dimmer, c.completedPanel);
            c.completedBubble.Hide();

            // Normal 5 (the beach): Level Completed opening Hard.
            var normal = StartStudentLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Normal, 5), StudentKind.Boy);
            var hardName = UIStrings.DifficultyName(Difficulty.Hard);
            c.openedText.text = UIStrings.Opened(hardName);
            c.openedRibbon.SetActive(true);
            CompletedWithStudent(c, normal, 3, LevelController.OpenedLine(hardName, Episode.Earthquake, Difficulty.Hard, scenarios.Playable));
            Shot("46-level-completed-hard-open", c.dimmer, c.completedPanel);
            c.completedBubble.Hide();
            c.openedRibbon.SetActive(false);

            CaptureHardCompletions(c, scenarios, Shot);

            CaptureStudentFlow(c, scenarios, Shot);
            CaptureCorridor(c, scenarios, Shot);
            CaptureClassroomAfter(c, scenarios, Shot);
            CaptureBedroom(c, scenarios, Shot);
            CaptureHome(c, scenarios, Shot);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Normal, 4, "n4", "building", StudentKind.Boy);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 2, "h2", "route_post", StudentKind.Girl);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Normal, 5, "n5", "watch", StudentKind.Boy);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 3, "h3", "shop_window", StudentKind.Boy);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 4, "h4", "run_push", StudentKind.Girl);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 5, "h5", "pull_up", StudentKind.Boy);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 6, "h6", "push_past_mom", StudentKind.Girl);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 7, "h7", "rumor", StudentKind.Boy);
        }

        static void CaptureHardCompletions(LevelController c, ScenarioLibrary scenarios, Action<string, ScreenPanel[]> shot)
        {
            LevelId Hard(int n) => new LevelId(Episode.Earthquake, Difficulty.Hard, n);
            void Finish(ProgressState progress, int number, string name)
            {
                var id = Hard(number);
                var scenario = StartStudentLevel(c, scenarios, id, StudentKind.Boy);
                var result = LevelCompletion.Record(progress, id, 3);
                var openedFire = result.Opened.Count > 0;
                var line = UIStrings.LevelCompletedHeadline("Niño");
                if (openedFire)
                {
                    var fire = UIStrings.EpisodeName(Episode.Fire);
                    c.openedText.text = UIStrings.Opened(fire);
                    line = LevelController.OpenedLine(fire, Episode.Fire, Difficulty.Easy, scenarios.Playable);
                }
                c.openedRibbon.SetActive(openedFire);
                c.SetCompletionNavigation(id, result.OffersNextEpisode, progress, scenarios.Playable);
                CompletedWithStudent(c, scenario, 3, line);
                shot(name, new[] { c.dimmer, c.completedPanel });
            }

            var progress = ProgressState.NewGame();
            progress.Open(Episode.Earthquake, Difficulty.Hard);
            Finish(progress, 7, "58-hard7-early");
            for (var n = 2; n <= 6; n++) LevelCompletion.Record(progress, Hard(n), 3);
            Finish(progress, 1, "58-hard1-opens-fire");
            Finish(progress, 7, "58-hard7-replay");

            progress = ProgressState.NewGame();
            for (var n = 1; n <= 6; n++) LevelCompletion.Record(progress, Hard(n), 3);
            Finish(progress, 7, "58-level-completed-fire-open");
            c.nextEpisodeButton.gameObject.SetActive(false);
            c.completedBubble.Hide();
            c.openedRibbon.SetActive(false);
        }

        static void CaptureHome(LevelController c, ScenarioLibrary scenarios, Action<string, ScreenPanel[]> shot)
        {
            CaptureSharedSettingLevel(c, scenarios, shot, Difficulty.Normal, 2, "n2", "hidden", StudentKind.Boy);
            CaptureSharedSettingLevel(c, scenarios, shot, Difficulty.Normal, 3, "n3", "glass", StudentKind.Girl);
            CaptureSharedSettingLevel(c, scenarios, shot, Difficulty.Hard, 1, "h1", "meet_home", StudentKind.Boy);
        }

        // Capture the cast at the start and the last visible state of the chosen Act-outs.
        static void CaptureSharedSettingLevel(LevelController c, ScenarioLibrary scenarios, Action<string, ScreenPanel[]> shot,
            Difficulty difficulty, int number, string prefix, string stopKey, StudentKind kind)
        {
            var s = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, difficulty, number));
            c.ShowSetting(ScenarioSettings.ActionArtKey(s.setting));
            c.UseStudentLayout();
            c.captionGoal.text = s.goal.text;
            c.captionBody.text = s.intro.text;
            c.FitCaption();
            c.actionBar.Setup(s, _ => { });

            var setting = c.ActiveSetting;
            setting.PrepareStudentStart(s.level, kind);
            var student = setting.student;
            student.bubble.Hide();
            void SetWobblers(bool shaking)
            {
                if (setting.shaker == null) return;
                foreach (var wobbler in setting.shaker.wobblers)
                    wobbler.GetComponent<UnityEngine.UI.Image>().sprite = wobbler.frames[shaking ? 1 : 0];
            }
            var props = setting.ActiveProps;
            var places = Array.ConvertAll(props, p => ((RectTransform)p.transform).anchoredPosition);
            var visibility = Array.ConvertAll(props, p => p.gameObject.activeSelf);

            SetWobblers(s.Shakes);
            shot(prefix + "-actionbar", new[] { c.actionBar.panel });
            if (difficulty == Difficulty.Hard && number == 4)
                shot("h4-pause", new[] { c.actionBar.panel, c.dimmer, c.pausePanel });

            void Outcome(string suffix, ActOut act, bool safe)
            {
                var facingLeft = false;
                var x = setting.StartX;
                var y = setting.startY;
                var capturedDadEntry = false;
                var last = act.LastStep(!safe);
                for (var i = 0; i <= last; i++)
                {
                    var step = act.steps[i];
                    // The Hard 6 aisle crosses the front leg at x 260. Render the walk at that point.
                    const float tableLegX = 260f;
                    if (act.key == "go_bag" && step.kind == ActOutStepKind.Walk && x > tableLegX && step.x <= tableLegX)
                    {
                        var fraction = Mathf.InverseLerp(x, step.x, tableLegX);
                        student.PlaceAt(tableLegX, Mathf.Lerp(y, step.y, fraction));
                        student.Face(true);
                        student.ShowFrame(StudentPose.Walk, 1);
                        shot("h6-table-crossing", Array.Empty<ScreenPanel>());
                    }
                    if (step.Moves)
                    {
                        var carrying = step.kind == ActOutStepKind.Carry || step.kind == ActOutStepKind.CarryTo;
                        if (!carrying) facingLeft = false;
                        else if (step.x != x) facingLeft = step.x < x;
                        x = step.x;
                        if (step.kind != ActOutStepKind.Carry) y = step.y;
                    }
                    switch (step.kind)
                    {
                        case ActOutStepKind.Face:
                            facingLeft = step.faceLeft;
                            break;
                        case ActOutStepKind.Overlay:
                            step.target.SetActive(true);
                            break;
                        case ActOutStepKind.PropHide:
                            step.target.GetComponent<LevelProp>().Hide();
                            break;
                        case ActOutStepKind.PropShow:
                            step.target.GetComponent<LevelProp>().Show();
                            break;
                        case ActOutStepKind.PropPickUp:
                            step.target.GetComponent<LevelProp>().PickUp(student.Hands);
                            break;
                        case ActOutStepKind.PropPlace:
                            step.target.GetComponent<LevelProp>().Place(step.place);
                            break;
                        case ActOutStepKind.Walk when step.grownUp != null:
                        case ActOutStepKind.CarryTo when step.grownUp != null:
                            step.grownUp.PlaceAt(step.grownUpDestination.x, step.grownUpDestination.y);
                            step.grownUp.Face(false);
                            step.grownUp.Play(GrownUpPose.Idle);
                            break;
                        case ActOutStepKind.GrownUp when step.grownUpAction == GrownUpAction.Walk:
                            step.grownUp.PlaceAt(step.x, step.y);
                            step.grownUp.Face(false);
                            step.grownUp.Play(GrownUpPose.Idle);
                            break;
                        case ActOutStepKind.GrownUp when step.grownUpAction == GrownUpAction.Turn:
                            step.grownUp.Face(step.faceLeft);
                            break;
                        case ActOutStepKind.GrownUp when step.grownUpAction == GrownUpAction.Pose:
                            step.grownUp.Play(step.grownUpPose);
                            break;
                    }

                    if (act.key == "call_adult" && step.kind == ActOutStepKind.Say && step.line == UIStrings.ActOutHelp)
                    {
                        student.PlaceAt(x, y);
                        student.Face(facingLeft);
                        student.ShowFrame(StudentPose.Call, 0);
                        setting.PositionReaction();
                        student.bubble.Say(step.line);
                        shot("h5-doorway-call", Array.Empty<ScreenPanel>());
                        student.bubble.Hide();
                    }
                    if (act.key == "call_adult" && step.kind == ActOutStepKind.GrownUp &&
                        step.grownUpAction == GrownUpAction.Walk && !capturedDadEntry)
                    {
                        capturedDadEntry = true;
                        step.grownUp.Play(GrownUpPose.Walk);
                        shot("h5-dad-entry", Array.Empty<ScreenPanel>());
                    }

                    // Hard 2's first step ends at the threshold exit. Its third ends at the shared turn.
                    if (safe && s.SafeChoice.action == "route_field" && (i == 0 || i == 2))
                    {
                        bool atCorner = i == 2;
                        student.PlaceAt(x, y);
                        student.Face(atCorner);
                        student.ShowFrame(atCorner ? StudentPose.Walk : StudentPose.Idle, 0);
                        if (atCorner)
                        {
                            step.grownUp.Face(true);
                            step.grownUp.Play(GrownUpPose.Walk);
                        }
                        shot(atCorner ? "h2-clear-corner" : "h2-door-exit", Array.Empty<ScreenPanel>());
                    }

                    // Hard 4's seventh step, (352, 230), is the first point on the pavement outside the exit,
                    // before the parking. The steps are the four stair points, then (279, 235), (329, 235) and it.
                    if (safe && s.SafeChoice.action == "stairs" && i == 6)
                    {
                        student.PlaceAt(x, y);
                        student.Face(false);
                        student.ShowFrame(StudentPose.Walk, 0);
                        shot("h4-exit", Array.Empty<ScreenPanel>());
                    }
                }

                student.PlaceAt(x, y);
                student.Face(facingLeft);
                student.ShowFrame(safe ? act.EndPose : act.stopPose, 0);
                setting.PositionReaction(act);
                var safeLine = s.phase == ScenarioPhases.Before ? UIStrings.ReactionReady : UIStrings.ReactionSafe;
                student.bubble.Say(safe ? safeLine : UIStrings.ReactionWait);
                SetWobblers(!safe && s.Shakes);
                shot(prefix + suffix, Array.Empty<ScreenPanel>());

                if (act.FirstOverlay != null) act.FirstOverlay.SetActive(false);
                for (var i = 0; i < props.Length; i++)
                {
                    props[i].Place(places[i]);
                    if (!visibility[i]) props[i].Hide();
                }
                foreach (var grownUp in setting.ActiveGrownUps) grownUp.PlaceAtStart();
                student.bubble.Hide();
            }

            Outcome("-safe", setting.ActOutFor(s.SafeChoice.action), true);
            Outcome("-stop", setting.ActOutFor(stopKey), false);
            if (s.SafeChoice.action == "call_adult")
                Outcome("-wait-stop", setting.ActOutFor("wait_for_mom"), false);
            if (s.SafeChoice.action == "field")
                Outcome("-tree-stop", setting.ActOutFor("tree"), false);
            if (s.SafeChoice.action == "route_field")
            {
                Outcome("-tree-stop", setting.ActOutFor("route_tree"), false);
                Outcome("-later-stop", setting.ActOutFor("route_later"), false);
            }
            // Hard 7's court: the stop at the open side toward home, and the phone pose at the start.
            if (s.SafeChoice.action == "radio")
            {
                Outcome("-home-stop", setting.ActOutFor("go_home"), false);
                Outcome("-phone-stop", setting.ActOutFor("phone_all_day"), false);
            }
            if (s.SafeChoice.action == "stairs")
            {
                Outcome("-wall-stop", setting.ActOutFor("mall_wall"), false);
                Outcome("-elevator-stop", setting.ActOutFor("elevator"), false);

                // Try Again climbs back up the stairs facing left. The tread halfway up is at art row 210,
                // which is 20 art pixels above the floor line at row 230.
                student.PlaceAt(215f, 20f);
                student.Face(true);
                student.ShowFrame(StudentPose.Walk, 1);
                shot("h4-stair-return", Array.Empty<ScreenPanel>());
            }
            SetWobblers(false);
        }

        // Normal 1 in the bedroom (n1-*): the Action Bar, the box set down on the low shelf, and the stop
        // after the Student reaches up with the books.
        static void CaptureBedroom(LevelController c, ScenarioLibrary scenarios, Action<string, ScreenPanel[]> shot)
        {
            var s = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Normal, 1));
            c.ShowSetting(ScenarioSettings.ActionArtKey(s.setting));
            c.UseStudentLayout();
            c.captionGoal.text = s.goal.text;
            c.captionBody.text = s.intro.text;
            c.FitCaption();
            c.actionBar.Setup(s, _ => { });

            var setting = c.ActiveSetting;
            var student = setting.student;
            LevelProp Prop(string key) => Array.Find(setting.props, p => p.key == key);
            var box = Prop("box");
            var placed = Prop("box_placed");
            var books = Prop("books");
            student.SetKind(StudentKind.Boy);
            student.bubble.Hide();

            void Stand(float x, StudentPose pose, bool faceLeft = false, int frame = 0)
            {
                student.PlaceAt(x);
                student.Face(faceLeft);
                student.ShowFrame(pose, frame);
            }

            Stand(setting.startX, setting.startPose);
            shot("n1-actionbar", new[] { c.actionBar.panel });

            var shelf = setting.ActOutFor(s.SafeChoice.action);
            box.gameObject.SetActive(false);
            placed.gameObject.SetActive(true);
            Stand(shelf.EndX(setting.startX), StudentPose.Idle);
            student.bubble.Say(UIStrings.ReactionReady);
            shot("n1-safe", Array.Empty<ScreenPanel>());
            placed.gameObject.SetActive(false);
            box.gameObject.SetActive(true);

            // An Unsafe stop that reaches up: the prop put where the Act-out puts it, the Student at the stop on
            // the given reach-up frame, with the Reaction when one is given. The prop goes back afterwards.
            void ReachUpStop(string name, LevelProp prop, string action, bool faceLeft, int frame, string reaction)
            {
                var actOut = setting.ActOutFor(action);
                var put = Array.Find(actOut.steps, step => step.kind == ActOutStepKind.PropPlace);
                var from = ((RectTransform)prop.transform).anchoredPosition;
                prop.Place(put.place);
                Stand(actOut.StopX(setting.startX), actOut.stopPose, faceLeft, frame);
                if (reaction != null) student.bubble.Say(reaction);
                shot(name, Array.Empty<ScreenPanel>());
                prop.Place(from);
                student.bubble.Hide();
            }
            ReachUpStop("n1-stop", books, "books_top", true, 0, UIStrings.ReactionWait);

            // The peak reach-up frame for both Students (C7): the picture held over the bed facing right
            // and the books held at the cabinet facing left (n1-picture-peak-*, n1-books-peak-*).
            foreach (var kind in new[] { StudentKind.Boy, StudentKind.Girl })
            {
                student.SetKind(kind);
                var suffix = "-" + kind.ToString().ToLowerInvariant();
                ReachUpStop("n1-picture-peak" + suffix, Prop("picture"), "picture", false, 1, null);
                ReachUpStop("n1-books-peak" + suffix, books, "books_top", true, 1, null);
            }
            student.SetKind(StudentKind.Boy);
        }

        // Easy 2 in the corridor (e2-*): the Action Bar, the last Safe step (cover at the inner wall) and
        // an Unsafe stop at the top of the stairs. The girl acts here, so both Students are seen in the shots.
        static void CaptureCorridor(LevelController c, ScenarioLibrary scenarios, Action<string, ScreenPanel[]> shot)
        {
            var s = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Easy, 2));
            c.ShowSetting(ScenarioSettings.ActionArtKey(s.setting));
            c.UseStudentLayout();
            c.captionGoal.text = s.goal.text;
            c.captionBody.text = s.intro.text;
            c.FitCaption();
            c.actionBar.Setup(s, _ => { });

            var setting = c.ActiveSetting;
            var student = setting.student;
            student.SetKind(StudentKind.Girl);
            student.bubble.Hide();

            void Stand(float x, StudentPose pose, bool faceLeft = false)
            {
                student.PlaceAt(x);
                student.Face(faceLeft);
                student.ShowFrame(pose, 0);
            }

            Stand(setting.startX, StudentPose.Idle);
            shot("e2-actionbar", new[] { c.actionBar.panel });

            var cover = setting.ActOutFor("cover");
            Stand(cover.EndX(setting.startX), cover.EndPose);
            student.bubble.Say(UIStrings.ReactionSafe);
            shot("e2-safe", new ScreenPanel[0]);

            var stairs = setting.ActOutFor("stairs");
            Stand(stairs.StopX(setting.startX), stairs.stopPose);
            student.bubble.Say(UIStrings.ReactionWait);
            shot("e2-stop", new ScreenPanel[0]);
            student.bubble.Hide();
        }

        // Easy 3 in the classroom after the shaking: the Action Bar, the exit and the stop under the desk.
        static void CaptureClassroomAfter(LevelController c, ScenarioLibrary scenarios, Action<string, ScreenPanel[]> shot)
        {
            var s = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Easy, 3));
            c.ShowSetting(ScenarioSettings.ActionArtKey(s.setting));
            c.UseStudentLayout();
            c.captionGoal.text = s.goal.text;
            c.captionBody.text = s.intro.text;
            c.FitCaption();
            c.actionBar.Setup(s, _ => { });

            var setting = c.ActiveSetting;
            var student = setting.student;
            var teacher = setting.grownUps[0];
            student.SetKind(StudentKind.Boy);
            student.PlaceAt(setting.startX, setting.startY);
            student.ShowFrame(setting.startPose, 0);
            student.bubble.Hide();
            teacher.PlaceAtStart();
            shot("e3-actionbar", new[] { c.actionBar.panel });

            var walk = setting.ActOutFor(s.SafeChoice.action);
            var exit = Array.Find(walk.steps, step => step.kind == ActOutStepKind.Walk && step.grownUp == teacher);
            teacher.PlaceAt(exit.grownUpDestination.x, exit.grownUpDestination.y);
            teacher.Play(GrownUpPose.Idle);
            student.PlaceAt(exit.x, exit.y);
            student.ShowFrame(walk.EndPose, 0);
            student.bubble.Say(UIStrings.ReactionSafe);
            shot("e3-safe", Array.Empty<ScreenPanel>());

            teacher.PlaceAtStart();
            var stay = setting.ActOutFor("stay");
            student.PlaceAt(stay.StopX(setting.startX), setting.startY);
            student.ShowFrame(stay.stopPose, 0);
            student.bubble.Say(UIStrings.ReactionWait);
            shot("e3-stop", Array.Empty<ScreenPanel>());
            student.bubble.Hide();

            var openedName = UIStrings.DifficultyName(Difficulty.Normal);
            c.openedText.text = UIStrings.Opened(openedName);
            c.openedRibbon.SetActive(true);
            Completed(c, s, 3);
            c.completedStudent.SetKind(StudentKind.Boy);
            c.completedStudent.ShowFrame(StudentPose.Happy, 0);
            c.completedBubble.Say(LevelController.OpenedLine(openedName, Episode.Earthquake, Difficulty.Normal, scenarios.Playable), -1f);
            shot("32-level-completed-normal-open", new[] { c.dimmer, c.completedPanel });
            c.completedBubble.Hide();
        }

        // Easy 1 in the Student flow (60-65, and the Feedback layout). Poses and spots come from the Setting group.
        static void CaptureStudentFlow(LevelController c, ScenarioLibrary scenarios, Action<string, ScreenPanel[]> shot)
        {
            var s = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Easy, 1));
            c.ShowSetting(ScenarioSettings.ActionArtKey(s.setting));
            c.UseStudentLayout();
            c.captionGoal.text = s.goal.text;
            c.captionBody.text = s.intro.text;
            c.FitCaption();
            c.actionBar.Setup(s, _ => { });

            var setting = c.ActiveSetting;
            var student = setting.student;
            student.SetKind(StudentKind.Boy);
            student.bubble.Hide();
            foreach (var actOut in setting.actOuts)
                foreach (var step in actOut.steps)
                    if (step.kind == ActOutStepKind.Overlay && step.target != null) step.target.SetActive(false);

            void Stand(float x, StudentPose pose, bool faceLeft = false)
            {
                student.PlaceAt(x);
                student.Face(faceLeft);
                student.ShowFrame(pose, 0);
            }

            Stand(setting.startX, StudentPose.Idle);
            shot("60-pilot-start", new ScreenPanel[0]);

            Stand(setting.startX, StudentPose.Surprised);
            student.bubble.Say(UIStrings.ReactionWhoa);
            shot("61-pilot-shaking", new ScreenPanel[0]);

            Stand(setting.startX, StudentPose.Idle);
            student.bubble.Hide();
            shot("62-pilot-actionbar", new[] { c.actionBar.panel });

            var desk = setting.ActOutFor("desk");
            Stand(desk.EndX(setting.startX), desk.EndPose);
            desk.FirstOverlay.SetActive(true);
            student.bubble.Say(UIStrings.ReactionSafe);
            shot("63-pilot-duck", new ScreenPanel[0]);
            desk.FirstOverlay.SetActive(false);

            var door = setting.ActOutFor("door");
            Stand(door.StopX(setting.startX), door.stopPose);
            student.bubble.Say(UIStrings.ReactionWait);
            shot("64-pilot-wait", new ScreenPanel[0]);
            student.bubble.Hide();

            var unsafeChoice = s.choices[ChoiceIndex(s, "run")];
            c.feedbackBanner.sprite = c.unsafeBanner;
            c.feedbackHeading.text = string.IsNullOrEmpty(unsafeChoice.heading) ? UIStrings.UnsafeHeading : unsafeChoice.heading;
            c.feedbackText.text = unsafeChoice.feedback;
            c.tryAgainButton.gameObject.SetActive(true);
            c.finishButton.gameObject.SetActive(false);
            shot("67-pilot-feedback", new[] { c.dimmer, c.feedbackPanel });

            c.safeChoiceText.text = UIStrings.SafeChoiceLine(s.SafeChoice.label);
            c.lessonText.text = s.lesson.text;
            c.stars.ShowInstant(3);
            c.completedStudent.SetKind(StudentKind.Boy);
            c.completedStudent.ShowFrame(StudentPose.Happy, 0);
            c.completedBubble.Say(UIStrings.LevelCompletedHeadline("Niño"), -1f);
            shot("65-pilot-completed", new[] { c.dimmer, c.completedPanel });
            c.completedBubble.Hide();
        }

        static void Completed(LevelController c, Scenario scenario, int stars)
        {
            var safe = scenario.SafeChoice;
            c.safeChoiceText.text = UIStrings.SafeChoiceLine(safe.label);
            c.lessonText.text = scenario.lesson.text;
            c.stars.ShowInstant(stars);
        }

        static Scenario PrepareLevel(LevelController c, ScenarioLibrary scenarios, LevelId level)
        {
            if (!scenarios.TryGet(level, out var scenario))
                throw new InvalidOperationException($"Cannot capture Level screenshots. '{level.Key}' has no Playable Level.");
            c.ShowSetting(ScenarioSettings.ActionArtKey(scenario.setting));
            c.SetLevelLabels(level);
            return scenario;
        }

        // A Level at its start in the Student flow: the Caption, the Action Bar and the Student in place.
        static Scenario StartStudentLevel(LevelController c, ScenarioLibrary scenarios, LevelId level, StudentKind kind)
        {
            var s = PrepareLevel(c, scenarios, level);
            c.UseStudentLayout();
            c.captionGoal.text = s.goal.text;
            c.captionBody.text = s.intro.text;
            c.FitCaption();
            c.actionBar.Setup(s, _ => { });
            c.ActiveSetting.PrepareStudentStart(s.level, kind);
            c.ActiveSetting.student.bubble.Hide();
            return s;
        }

        // Level Completed in the Student flow: the happy Student and the cheer line in its bubble.
        static void CompletedWithStudent(LevelController c, Scenario scenario, int stars, string line)
        {
            Completed(c, scenario, stars);
            c.completedStudent.SetKind(StudentKind.Boy);
            c.completedStudent.ShowFrame(StudentPose.Happy, 0);
            c.completedBubble.Say(line, -1f);
        }

        static int ChoiceIndex(Scenario scenario, string id)
        {
            var index = Array.FindIndex(scenario.choices, choice => choice.id == id);
            if (index < 0)
                throw new InvalidOperationException($"Cannot capture Level screenshots. Scenario '{scenario.level}' is missing Choice '{id}'.");
            return index;
        }
    }
}
