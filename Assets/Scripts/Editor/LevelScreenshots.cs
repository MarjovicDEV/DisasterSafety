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

            var classroomAfter = PrepareLevel(c, scenarios, new LevelId(Episode.Earthquake, Difficulty.Easy, 3));
            c.bantay.bubbleText.text = classroomAfter.intro.text;
            Shot("30-level3", c.choicesPanel, c.bantay.bubble);

            var stay = ChoiceIndex(classroomAfter, "stay");
            c.choiceCards[stay].SetTried(true);
            Feedback(c, classroomAfter.choices[stay]);
            Shot("31-level3-feedback-unsafe", c.choicesPanel, c.dimmer, c.feedbackPanel);

            var openedName = UIStrings.DifficultyName(Difficulty.Normal);
            c.openedText.text = UIStrings.Opened(openedName);
            c.openedRibbon.SetActive(true);
            c.cheerBantay.bubbleText.text = LevelController.OpenedLine(openedName, Episode.Earthquake, Difficulty.Normal, scenarios.Playable);
            Completed(c, classroomAfter, 3);
            Shot("32-level-completed-normal-open", c.dimmer, c.completedPanel, c.cheerBantay.bubble);

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
