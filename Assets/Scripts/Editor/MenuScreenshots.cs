using System;
using System.IO;
using DisasterSafety.Core;
using DisasterSafety.Game.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.EditorTools
{
    /// <summary>
    /// Renders every Main Menu screen to PNG without entering Play mode, at the reference
    /// resolution and at a wide phone shape. For layout review only; it does not save the scene.
    /// Menu: Disaster Safety > Capture Menu Screenshots. Batch: -executeMethod
    /// DisasterSafety.EditorTools.MenuScreenshots.CaptureBatch -shotsDir &lt;folder&gt;
    /// </summary>
    public static class MenuScreenshots
    {
        internal static readonly Vector2Int[] Sizes = { new Vector2Int(1920, 1080), new Vector2Int(2400, 1080) };

        [MenuItem("Disaster Safety/Capture Menu Screenshots")]
        public static void CaptureToProjectFolder() => Capture(Path.GetFullPath("Screenshots"));

        public static void CaptureBatch()
        {
            var args = Environment.GetCommandLineArgs();
            var dir = Path.GetFullPath("Screenshots");
            for (var i = 0; i < args.Length - 1; i++)
                if (args[i] == "-shotsDir") dir = args[i + 1];
            try
            {
                Capture(dir);
                EditorApplication.Exit(0);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                EditorApplication.Exit(1);
            }
        }

        public static void Capture(string dir)
        {
            Directory.CreateDirectory(dir);
            EditorSceneManager.OpenScene(ProjectBuilder.MenuScenePath, OpenSceneMode.Single);
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();
            var canvas = UnityEngine.Object.FindAnyObjectByType<Canvas>();
            var cam = Camera.main;

            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cam;
            canvas.planeDistance = 5f;

            // Fresh-install state for the shots: Earthquake/Easy open, nothing playable.
            foreach (var card in c.episodeCards) card.Show(card.episode == Episode.Earthquake);
            foreach (var card in c.difficultyCards) card.Show(card.difficulty == Difficulty.Easy);
            c.continueButton.interactable = false;
            c.startButton.interactable = false;
            for (var i = 0; i < c.levelSlots.Length; i++)
            {
                c.levelSlots[i].gameObject.SetActive(i < 3);
                c.levelSlots[i].Show(i + 1, 0, i == 0);
            }
            c.levelHeader.text = "Earthquake - Easy";
            // The Main Menu Student greets in the Happy pose on the first shot and stands Idle on the others.
            var student = c.student;

            var gate = MenuBackground.SchoolGate;
            var shots = new (string name, ScreenPanel[] show, MenuBackground background)[]
            {
                ("01-main", new[] { c.mainPanel }, gate),
                ("02-episodes", new[] { c.episodePanel }, MenuBackground.Noticeboard),
                ("03-difficulty", new[] { c.difficultyPanel }, MenuBackground.DrillGround),
                ("04-levels", new[] { c.levelPanel }, MenuBackground.Classroom),
                ("05-settings", new[] { c.mainPanel, c.dimmer, c.settingsPanel }, gate),
                ("06-howtoplay", new[] { c.mainPanel, c.dimmer, c.howToPlayPanel }, gate),
                ("07-exit", new[] { c.mainPanel, c.dimmer, c.exitPanel }, gate),
                ("08-name", new[] { c.mainPanel, c.dimmer, c.namePanel }, gate),
                ("66-student-picker", new[] { c.mainPanel, c.dimmer, c.studentPanel }, gate),
            };
            var all = new[]
            {
                c.mainPanel, c.settingsPanel, c.howToPlayPanel, c.exitPanel, c.namePanel, c.studentPanel, c.episodePanel,
                c.difficultyPanel, c.levelPanel, c.dimmer
            };

            foreach (var size in Sizes)
            {
                foreach (var shot in shots)
                {
                    foreach (var p in all) p.gameObject.SetActive(Array.IndexOf(shot.show, p) >= 0);
                    c.world.Show(shot.background);
                    var greeting = shot.name == "01-main";
                    student.actor.ShowFrame(greeting ? StudentPose.Happy : StudentPose.Idle, 0);
                    if (greeting) student.Bubble.Say(Game.UIStrings.StudentHello, -1f);
                    else student.Bubble.Hide();
                    Render(cam, canvas, size, Path.Combine(dir, shot.name + "-" + size.x + "x" + size.y + ".png"));
                }
            }

            CaptureBackgrounds(dir, c, cam, canvas, all);

            // Every How to Play Tutorial Page, at both sizes.
            c.world.Show(gate);
            foreach (var p in all) p.gameObject.SetActive(p == c.mainPanel || p == c.dimmer || p == c.howToPlayPanel);
            student.Bubble.Hide();
            foreach (var size in Sizes)
            {
                for (var page = 0; page < Game.UIStrings.TutorialPages.Length; page++)
                {
                    c.tutorial.Open();
                    for (var n = 0; n < page; n++) c.tutorial.NextPage();
                    Render(cam, canvas, size, Path.Combine(dir, "06-howtoplay-page" + (page + 1) + "-" + size.x + "x" + size.y + ".png"));
                    if (page == 2)
                    {
                        c.tutorial.picture.sprite = c.tutorial.frames[page * 2 + 1];
                        Render(cam, canvas, size, Path.Combine(dir, "06-howtoplay-page3-pressed-" + size.x + "x" + size.y + ".png"));
                    }
                }
            }
            LevelScreenshots.Capture(dir);
            Debug.Log("Screenshots written to " + dir);
        }

        /// <summary>
        /// The gate with a long greeting for both Students, and each other selection background:
        /// Fire Difficulty selection, Earthquake Normal and Hard Level selection and empty Fire
        /// Level selection (C3, C4).
        /// </summary>
        static void CaptureBackgrounds(string dir, MainMenuController c, Camera cam, Canvas canvas, ScreenPanel[] all)
        {
            var student = c.student;
            var longGreeting = Game.UIStrings.StudentWelcomeBack("Marijoyceann");
            foreach (var kind in new[] { StudentKind.Boy, StudentKind.Girl })
            {
                foreach (var p in all) p.gameObject.SetActive(p == c.mainPanel);
                c.world.Show(MenuBackground.SchoolGate);
                student.SetKind(kind);
                student.actor.ShowFrame(StudentPose.Happy, 0);
                student.Bubble.Say(longGreeting, -1f);
                foreach (var size in MenuScreenshots.Sizes)
                    Render(cam, canvas, size, Path.Combine(dir, "01-main-long-name-" + kind.ToString().ToLowerInvariant()
                        + "-" + size.x + "x" + size.y + ".png"));
            }
            student.SetKind(StudentKind.Boy);
            student.actor.ShowFrame(StudentPose.Idle, 0);
            student.Bubble.Hide();

            void Shot(string name, ScreenPanel panel, MenuBackground background)
            {
                foreach (var p in all) p.gameObject.SetActive(p == panel);
                c.world.Show(background);
                foreach (var size in MenuScreenshots.Sizes)
                    Render(cam, canvas, size, Path.Combine(dir, name + "-" + size.x + "x" + size.y + ".png"));
            }

            var fire = Array.Find(c.episodeCards, card => card.episode == Episode.Fire);
            fire.Show(true);
            Shot("02-episodes-fire-open", c.episodePanel, MenuBackground.Noticeboard);
            fire.Show(false);

            c.difficultyHeader.text = Game.UIStrings.EpisodeName(Episode.Fire);
            c.difficultyEmblem.sprite = c.episodeEmblems[(int)Episode.Fire];
            Shot("03-difficulty-fire", c.difficultyPanel, MenuBackground.FireStation);
            c.difficultyHeader.text = Game.UIStrings.EpisodeName(Episode.Earthquake);
            c.difficultyEmblem.sprite = c.episodeEmblems[(int)Episode.Earthquake];

            void StageLevels(Episode e, Difficulty d)
            {
                c.levelHeader.text = Game.UIStrings.EpisodeName(e) + " - " + Game.UIStrings.DifficultyName(d);
                var count = Curriculum.LevelCount(d);
                for (var i = 0; i < c.levelSlots.Length; i++)
                {
                    c.levelSlots[i].gameObject.SetActive(i < count);
                    c.levelSlots[i].Show(i + 1, 0, i == 0);
                }
            }
            void Levels(string name, Episode e, Difficulty d, MenuBackground background)
            {
                StageLevels(e, d);
                Shot(name, c.levelPanel, background);
            }
            Levels("04-levels-normal", Episode.Earthquake, Difficulty.Normal, MenuBackground.Schoolyard);
            Levels("04-levels-hard", Episode.Earthquake, Difficulty.Hard, MenuBackground.CoveredCourt);
            Levels("04-levels-fire", Episode.Fire, Difficulty.Easy, MenuBackground.StationNoticeboard);
            // Put back the Earthquake Easy staging that 04-levels used, without capturing it again.
            StageLevels(Episode.Earthquake, Difficulty.Easy);
        }

        internal static void Render(Camera cam, Canvas canvas, Vector2Int size, string path)
        {
            var rt = new RenderTexture(size.x, size.y, 24, RenderTextureFormat.ARGB32);
            cam.targetTexture = rt;
            for (var pass = 0; pass < 3; pass++)
            {
                Canvas.ForceUpdateCanvases();
                var root = (RectTransform)canvas.transform;
                foreach (var env in canvas.GetComponentsInChildren<EnvelopeScaler>(true))
                {
                    var s = Mathf.Max(root.rect.width / env.artSize.x, root.rect.height / env.artSize.y);
                    env.transform.localScale = new Vector3(s, s, 1f);
                }
                foreach (var g in canvas.GetComponentsInChildren<LayoutGroup>(true))
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)g.transform);
            }
            cam.Render();
            var prev = RenderTexture.active;
            RenderTexture.active = rt;
            var tex = new Texture2D(size.x, size.y, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, size.x, size.y), 0, 0);
            tex.Apply();
            RenderTexture.active = prev;
            cam.targetTexture = null;
            File.WriteAllBytes(path, tex.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(tex);
            rt.Release();
            UnityEngine.Object.DestroyImmediate(rt);
        }
    }
}
