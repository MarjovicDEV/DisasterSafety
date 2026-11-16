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
        const string ScenarioFolder = "Assets/Resources/Scenarios";

        public static void Capture(string dir)
        {
            EditorSceneManager.OpenScene(ProjectBuilder.LevelScenePath, OpenSceneMode.Single);
            var c = UnityEngine.Object.FindAnyObjectByType<LevelController>();
            var canvas = UnityEngine.Object.FindAnyObjectByType<Canvas>();
            var cam = Camera.main;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cam;
            canvas.planeDistance = 5f;

            var s = PrepareLevel(c, new LevelId(Episode.Earthquake, Difficulty.Easy, 1));

            var all = new[] { c.choicesPanel, c.dimmer, c.feedbackPanel, c.completedPanel, c.pausePanel, c.bantay.bubble, c.cheerBantay.bubble };

            void Shot(string name, params ScreenPanel[] show)
            {
                foreach (var p in all) p.gameObject.SetActive(Array.IndexOf(show, p) >= 0);
                foreach (var size in MenuScreenshots.Sizes)
                    MenuScreenshots.Render(cam, canvas, size, Path.Combine(dir, name + "-" + size.x + "x" + size.y + ".png"));
            }

            c.bantay.bubbleText.text = s.intro.text;
            Shot("10-level", c.choicesPanel, c.bantay.bubble);

            c.bantay.bubbleText.text = UIStrings.HintLine(s.hint.text);
            Shot("11-level-hint", c.choicesPanel, c.bantay.bubble);

            // An Unsafe Choice (the window), then the Safe Choice.
            var window = Array.FindIndex(s.choices, x => x.id == "window");
            c.choiceCards[window].SetTried(true);
            Feedback(c, s.choices[window]);
            Shot("12-feedback-unsafe", c.choicesPanel, c.dimmer, c.feedbackPanel);

            Feedback(c, s.SafeChoice);
            Shot("13-feedback-safe", c.choicesPanel, c.dimmer, c.feedbackPanel);

            Shot("14-pause", c.choicesPanel, c.dimmer, c.pausePanel);

            c.choiceCards[window].SetTried(false);
            c.cheerBantay.bubbleText.text = UIStrings.LevelCompletedHeadline("Niño");
            c.safePicture.sprite = Picture(c, s.SafeChoice.picture);
            c.safeChoiceText.text = UIStrings.SafeChoiceLine(s.SafeChoice.label);
            c.lessonText.text = s.lesson.text;
            c.stars.ShowInstant(2);
            Shot("15-level-completed", c.dimmer, c.completedPanel, c.cheerBantay.bubble);

            var corridor = PrepareLevel(c, new LevelId(Episode.Earthquake, Difficulty.Easy, 2));
            c.bantay.bubbleText.text = corridor.intro.text;
            Shot("20-level2", c.choicesPanel, c.bantay.bubble);

            Feedback(c, corridor.SafeChoice);
            Shot("21-level2-feedback-safe", c.choicesPanel, c.dimmer, c.feedbackPanel);
        }

        static Scenario PrepareLevel(LevelController c, LevelId level)
        {
            var scenario = Scenario.Parse(File.ReadAllText(Path.Combine(ScenarioFolder, level.Key + ".json")));
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
