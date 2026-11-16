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
        static readonly Vector2Int[] Sizes = { new Vector2Int(1920, 1080), new Vector2Int(2400, 1080) };

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
            c.bantay.bubbleText.text = "Hi! I am Bantay. Tap PLAY to start!";

            var shots = new (string name, ScreenPanel[] show)[]
            {
                ("01-main", new[] { c.mainPanel, c.bantay.bubble }),
                ("02-episodes", new[] { c.episodePanel }),
                ("03-difficulty", new[] { c.difficultyPanel }),
                ("04-levels", new[] { c.levelPanel }),
                ("05-settings", new[] { c.mainPanel, c.dimmer, c.settingsPanel }),
                ("06-howtoplay", new[] { c.mainPanel, c.dimmer, c.howToPlayPanel }),
                ("07-exit", new[] { c.mainPanel, c.dimmer, c.exitPanel }),
                ("08-name", new[] { c.mainPanel, c.dimmer, c.namePanel }),
            };
            var all = new[]
            {
                c.mainPanel, c.settingsPanel, c.howToPlayPanel, c.exitPanel, c.namePanel, c.episodePanel,
                c.difficultyPanel, c.levelPanel, c.dimmer, c.bantay.bubble
            };

            foreach (var size in Sizes)
            {
                foreach (var shot in shots)
                {
                    foreach (var p in all) p.gameObject.SetActive(Array.IndexOf(shot.show, p) >= 0);
                    Render(cam, canvas, size, Path.Combine(dir, shot.name + "-" + size.x + "x" + size.y + ".png"));
                }
            }
            Debug.Log("Screenshots written to " + dir);
        }

        static void Render(Camera cam, Canvas canvas, Vector2Int size, string path)
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
