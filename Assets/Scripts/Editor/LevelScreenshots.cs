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
    /// Renders the Level scene's screens (playing, Hint, both Feedback cards, Pause, Level
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
            var s = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Easy, 1));

            var all = new[] { c.choicesPanel, c.dimmer, c.feedbackPanel, c.completedPanel, c.pausePanel, c.bantay.bubble, c.cheerBantay.bubble, c.actionBar.panel };

            void Shot(string name, params ScreenPanel[] show)
            {
                foreach (var p in all) p.gameObject.SetActive(Array.IndexOf(show, p) >= 0);
                foreach (var size in MenuScreenshots.Sizes)
                {
                    c.choicesLayout?.Apply((float)size.x / size.y);
                    MenuScreenshots.Render(cam, canvas, size, Path.Combine(dir, name + "-" + size.x + "x" + size.y + ".png"));
                }
            }

            c.bantay.bubbleText.text = s.intro.text;
            Shot("10-level", c.choicesPanel, c.bantay.bubble);

            c.bantay.bubbleText.text = UIStrings.HintLine(s.hint.text);
            Shot("11-level-hint", c.choicesPanel, c.bantay.bubble);

            // An Unsafe Choice (the window), then the Safe Choice.
            var window = ChoiceIndex(s, "window");
            c.choiceCards[window].SetTried(true);
            Feedback(c, s.choices[window]);
            Shot("12-feedback-unsafe", c.choicesPanel, c.dimmer, c.feedbackPanel);

            Feedback(c, s.SafeChoice);
            Shot("13-feedback-safe", c.choicesPanel, c.dimmer, c.feedbackPanel);

            Shot("14-pause", c.choicesPanel, c.dimmer, c.pausePanel);

            c.choiceCards[window].SetTried(false);
            c.cheerBantay.bubbleText.text = UIStrings.LevelCompletedHeadline("Niño");
            Completed(c, s, 2);
            Shot("15-level-completed", c.dimmer, c.completedPanel, c.cheerBantay.bubble);

            var corridor = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Easy, 2));
            c.bantay.bubbleText.text = corridor.intro.text;
            Shot("20-level2", c.choicesPanel, c.bantay.bubble);

            Feedback(c, corridor.SafeChoice);
            Shot("21-level2-feedback-safe", c.choicesPanel, c.dimmer, c.feedbackPanel);

            var normalShots = new[] { "40-normal1", "41-normal2", "42-normal3", "43-normal4", "44-normal5" };
            Scenario normal = null;
            for (var i = 0; i < normalShots.Length; i++)
            {
                normal = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Normal, i + 1));
                c.bantay.bubbleText.text = normal.intro.text;
                Shot(normalShots[i], c.choicesPanel, c.bantay.bubble);
            }

            // Normal 5 (the beach): an Unsafe Choice, then Level Completed opening Hard.
            var fish = ChoiceIndex(normal, "fish");
            c.choiceCards[fish].SetTried(true);
            Feedback(c, normal.choices[fish]);
            Shot("45-normal5-feedback-unsafe", c.choicesPanel, c.dimmer, c.feedbackPanel);
            c.choiceCards[fish].SetTried(false);

            var hardName = UIStrings.DifficultyName(Difficulty.Hard);
            c.openedText.text = UIStrings.Opened(hardName);
            c.openedRibbon.SetActive(true);
            c.cheerBantay.bubbleText.text = LevelController.OpenedLine(hardName, Episode.Earthquake, Difficulty.Hard, scenarios.Playable);
            Completed(c, normal, 3);
            Shot("46-level-completed-hard-open", c.dimmer, c.completedPanel, c.cheerBantay.bubble);
            c.openedRibbon.SetActive(false);

            var hardShots = new[] { "50-hard1", "51-hard2", "52-hard3", "53-hard4", "54-hard5", "55-hard6", "56-hard7" };
            for (var i = 0; i < hardShots.Length; i++)
            {
                var hard = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Hard, i + 1));
                c.bantay.bubbleText.text = hard.intro.text;
                Shot(hardShots[i], c.choicesPanel, c.bantay.bubble);
            }

            // Hard 3 (the mall food court): an Unsafe Choice.
            var mall = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Hard, 3));
            var elevator = ChoiceIndex(mall, "elevator");
            c.choiceCards[elevator].SetTried(true);
            Feedback(c, mall.choices[elevator]);
            Shot("57-hard3-feedback-unsafe", c.choicesPanel, c.dimmer, c.feedbackPanel);
            c.choiceCards[elevator].SetTried(false);

            // Hard 7 (the Evacuation Area): Level Completed opening Fire, which has no Playable Level yet.
            var evacuation = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Hard, 7));
            var fireName = UIStrings.EpisodeName(Episode.Fire);
            c.openedText.text = UIStrings.Opened(fireName);
            c.openedRibbon.SetActive(true);
            c.cheerBantay.bubbleText.text = LevelController.OpenedLine(fireName, Episode.Fire, Difficulty.Easy, scenarios.Playable);
            Completed(c, evacuation, 3);
            Shot("58-level-completed-fire-open", c.dimmer, c.completedPanel, c.cheerBantay.bubble);
            c.openedRibbon.SetActive(false);

            CaptureStudentFlow(c, scenarios, Shot);
            CaptureCorridor(c, scenarios, Shot);
            CaptureClassroomAfter(c, scenarios, Shot);
            CaptureBedroom(c, scenarios, Shot);
            CaptureHome(c, scenarios, Shot);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Normal, 4, "n4", "building", StudentKind.Boy);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 2, "h2", "route_post", StudentKind.Girl);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Normal, 5, "n5", "fish", StudentKind.Boy);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 3, "h3", "shop_window", StudentKind.Boy);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 4, "h4", "run_push", StudentKind.Girl);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 5, "h5", "pull_up", StudentKind.Boy);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 6, "h6", "no_note", StudentKind.Girl);
            CaptureSharedSettingLevel(c, scenarios, Shot, Difficulty.Hard, 7, "h7", "rumor", StudentKind.Boy);
        }

        static void CaptureHome(LevelController c, ScenarioLibrary scenarios, Action<string, ScreenPanel[]> shot)
        {
            CaptureSharedSettingLevel(c, scenarios, shot, Difficulty.Normal, 2, "n2", "hidden", StudentKind.Boy);
            CaptureSharedSettingLevel(c, scenarios, shot, Difficulty.Normal, 3, "n3", "run", StudentKind.Girl);
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

            void Outcome(string suffix, ActOut act, bool safe)
            {
                var facingLeft = false;
                var x = setting.StartX;
                var y = setting.startY;
                var last = act.LastStep(!safe);
                for (var i = 0; i <= last; i++)
                {
                    var step = act.steps[i];
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
            SetWobblers(false);
        }

        // Normal 1 in the bedroom (n1-*): the Action Bar, the box set down on the low shelf, and the stop
        // after the bed has slid toward the window.
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
            var bed = (RectTransform)Prop("bed").transform;
            student.SetKind(StudentKind.Boy);
            student.bubble.Hide();

            void Stand(float x, StudentPose pose, bool faceLeft = false)
            {
                student.PlaceAt(x);
                student.Face(faceLeft);
                student.ShowFrame(pose, 0);
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

            var push = setting.ActOutFor("bed_window");
            var slide = Array.Find(push.steps, step => step.kind == ActOutStepKind.Push);
            var bedStart = bed.anchoredPosition;
            bed.anchoredPosition = bedStart + new Vector2(slide.x * student.unitsPerPixel, 0f);
            Stand(push.StopX(setting.startX), push.stopPose, faceLeft: true);
            student.bubble.Say(UIStrings.ReactionWait);
            shot("n1-stop", Array.Empty<ScreenPanel>());
            bed.anchoredPosition = bedStart;
            student.bubble.Hide();
        }

        // Easy 2 in the corridor (e2-*): the Action Bar, the last Safe step (cover at the inner wall) and
        // an Unsafe stop (the top of the stairs). The girl acts here, so both Students are seen in the shots.
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
            Stand(stairs.StopX(setting.startX), StudentPose.Wait);
            student.bubble.Say(UIStrings.ReactionWait);
            shot("e2-stop", new ScreenPanel[0]);
            student.bubble.Hide();
        }

        // Easy 3 in the classroom after the shaking: the Action Bar, the exit and the stop behind the Teacher.
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
            var push = setting.ActOutFor("push");
            student.PlaceAt(push.StopX(setting.startX), setting.startY);
            student.ShowFrame(push.stopPose, 0);
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

            var window = setting.ActOutFor("window");
            Stand(window.StopX(setting.startX), StudentPose.Wait);
            student.bubble.Say(UIStrings.ReactionWait);
            shot("64-pilot-wait", new ScreenPanel[0]);
            student.bubble.Hide();

            var unsafeChoice = s.choices[ChoiceIndex(s, "window")];
            c.feedbackBanner.sprite = c.unsafeBanner;
            c.feedbackHeading.text = UIStrings.UnsafeHeading;
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
            c.safePicture.sprite = Picture(c, safe.picture);
            c.safeChoiceText.text = UIStrings.SafeChoiceLine(safe.label);
            c.lessonText.text = scenario.lesson.text;
            c.stars.ShowInstant(stars);
        }

        static Scenario PrepareLevel(LevelController c, ScenarioLibrary scenarios, LevelId level)
        {
            if (!scenarios.TryGet(level, out var scenario))
                throw new InvalidOperationException($"Cannot capture Level screenshots. '{level.Key}' has no Playable Level.");
            c.ShowSetting(scenario.setting);

            c.goalText.text = scenario.goal.text;
            c.levelLabel.text = UIStrings.LevelLabel(level);
            for (var i = 0; i < c.choiceCards.Length; i++)
            {
                var used = i < scenario.choices.Length;
                c.choiceCards[i].gameObject.SetActive(used);
                if (used) c.choiceCards[i].Show(scenario.choices[i], Picture(c, scenario.choices[i].picture));
            }
            return scenario;
        }

        static int ChoiceIndex(Scenario scenario, string id)
        {
            var index = Array.FindIndex(scenario.choices, choice => choice.id == id);
            if (index < 0)
                throw new InvalidOperationException($"Cannot capture Level screenshots. Scenario '{scenario.level}' is missing Choice '{id}'.");
            return index;
        }

        static void Feedback(LevelController c, ScenarioChoice choice)
        {
            c.feedbackBanner.sprite = choice.safe ? c.safeBanner : c.unsafeBanner;
            c.feedbackHeading.text = !string.IsNullOrEmpty(choice.heading) ? choice.heading
                : choice.safe ? UIStrings.SafeHeading : UIStrings.UnsafeHeading;
            c.feedbackPicture.sprite = Picture(c, choice.picture);
            c.feedbackText.text = choice.feedback;
            c.tryAgainButton.gameObject.SetActive(!choice.safe);
            c.finishButton.gameObject.SetActive(choice.safe);
        }

        static Sprite Picture(LevelController c, string key)
        {
            foreach (var p in c.pictures)
                if (p != null && p.name == key) return p;
            return null;
        }
    }
}
