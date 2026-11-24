using System;
using System.IO;
using DisasterSafety.Core;
using DisasterSafety.Game;
using DisasterSafety.Game.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace DisasterSafety.EditorTools
{
    /// <summary>
    /// Builds the Boot, MainMenu and Level scenes, the audio library, the pixel fonts, the Build
    /// Settings scene list and the Android orientation settings from the generated art.
    /// Menu: Disaster Safety > Build Everything. Batch: -executeMethod
    /// DisasterSafety.EditorTools.ProjectBuilder.BuildAllBatch
    /// Re-running it replaces every scene, so make layout changes here, not by hand.
    /// The Level scene lives in ProjectBuilder.Level.cs.
    /// </summary>
    public static partial class ProjectBuilder
    {
        public const string BootScenePath = "Assets/Scenes/Boot.unity";
        public const string MenuScenePath = "Assets/Scenes/MainMenu.unity";
        const string Art = "Assets/Art/";
        const string AudioRoot = "Assets/Audio/";
        const float Px = 4f; // UI units per art pixel

        static readonly Color Ink = new Color32(0x2b, 0x21, 0x35, 0xff);
        static readonly Color ErrorRed = new Color32(0xc8, 0x2a, 0x34, 0xff);
        static readonly Color HintGray = new Color32(0x8a, 0x84, 0x78, 0xff);

        static Font f3, f4, f6, f8;

        [MenuItem("Disaster Safety/Build Everything")]
        public static void BuildAll()
        {
            AssetDatabase.ImportAsset("Assets/Art", ImportAssetOptions.ImportRecursive | ImportAssetOptions.ForceUpdate);
            PixelFontBuilder.Build();
            f3 = PixelFontBuilder.Load(3);
            f4 = PixelFontBuilder.Load(4);
            f6 = PixelFontBuilder.Load(6);
            f8 = PixelFontBuilder.Load(8);
            BuildAudioLibrary();
            ConfigurePlayer();
            BuildBootScene();
            BuildMenuScene();
            BuildLevelScene();
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(BootScenePath, true),
                new EditorBuildSettingsScene(MenuScenePath, true),
                new EditorBuildSettingsScene(LevelScenePath, true),
            };
            AssetDatabase.SaveAssets();
            Debug.Log("Disaster Safety: build complete.");
        }

        public static void BuildAllBatch()
        {
            try
            {
                BuildAll();
                EditorApplication.Exit(0);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                EditorApplication.Exit(1);
            }
        }

        public const string ApkPath = "Builds/DisasterSafety-pilot.apk";

        /// <summary>
        /// Batch: BuildAll, then the Android test APK at <see cref="ApkPath"/> (landscape, the
        /// existing bundle id, IL2CPP for ARM64, development build off). Run with -buildTarget Android.
        /// </summary>
        public static void BuildAndroidBatch()
        {
            try
            {
                BuildAll();
                var android = UnityEditor.Build.NamedBuildTarget.Android;
                if (string.IsNullOrEmpty(PlayerSettings.GetApplicationIdentifier(android)))
                    PlayerSettings.SetApplicationIdentifier(android,
                        PlayerSettings.GetApplicationIdentifier(UnityEditor.Build.NamedBuildTarget.Standalone));
                PlayerSettings.SetScriptingBackend(android, ScriptingImplementation.IL2CPP);
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
                EditorUserBuildSettings.buildAppBundle = false;
                Directory.CreateDirectory(Path.GetDirectoryName(ApkPath));
                var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
                {
                    scenes = new[] { BootScenePath, MenuScenePath, LevelScenePath },
                    locationPathName = ApkPath,
                    target = BuildTarget.Android,
                    options = BuildOptions.None,
                });
                var summary = report.summary;
                Debug.Log($"Disaster Safety: Android build {summary.result}, {summary.totalSize} bytes, {ApkPath}");
                EditorApplication.Exit(summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded ? 0 : 1);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                EditorApplication.Exit(1);
            }
        }

        // Project settings --------------------------------------------------------
        static void ConfigurePlayer()
        {
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            // Chapter III states Android 10 (API 29) or later.
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel29;
        }

        static void BuildAudioLibrary()
        {
            Directory.CreateDirectory("Assets/Resources");
            var path = "Assets/Resources/" + AudioLibrary.ResourcePath + ".asset";
            var lib = AssetDatabase.LoadAssetAtPath<AudioLibrary>(path);
            if (lib == null)
            {
                lib = ScriptableObject.CreateInstance<AudioLibrary>();
                AssetDatabase.CreateAsset(lib, path);
            }
            lib.menuMusic = Clip("Music/bgm_menu");
            lib.levelMusic = Clip("Music/bgm_level");
            lib.click = Clip("Sfx/sfx_click");
            lib.back = Clip("Sfx/sfx_back");
            lib.open = Clip("Sfx/sfx_open");
            lib.confirm = Clip("Sfx/sfx_confirm");
            lib.locked = Clip("Sfx/sfx_locked");
            lib.type = Clip("Sfx/sfx_type");
            lib.toggle = Clip("Sfx/sfx_toggle");
            lib.sparkle = Clip("Sfx/sfx_sparkle");
            lib.bark = Clip("Sfx/sfx_bark");
            lib.soon = Clip("Sfx/sfx_soon");
            lib.rumble = Clip("Sfx/sfx_rumble");
            lib.safe = Clip("Sfx/sfx_safe");
            lib.unsafeChoice = Clip("Sfx/sfx_unsafe");
            lib.star = Clip("Sfx/sfx_star");
            lib.complete = Clip("Sfx/sfx_complete");
            lib.opened = Clip("Sfx/sfx_opened");
            lib.blip = Clip("Sfx/sfx_blip");
            EditorUtility.SetDirty(lib);
        }

        static AudioClip Clip(string rel)
        {
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(AudioRoot + rel + ".wav");
            if (clip == null) Debug.LogWarning("Missing audio: " + rel + ". Run ArtSource/audio/make_audio.py.");
            return clip;
        }

        static Sprite S(string rel)
        {
            var s = AssetDatabase.LoadAssetAtPath<Sprite>(Art + rel + ".png");
            if (s == null) throw new FileNotFoundException("Missing sprite. Run the Aseprite art build.", Art + rel + ".png");
            return s;
        }

        static Sprite[] Frames(string rel, int count)
        {
            var list = new Sprite[count];
            for (var i = 0; i < count; i++) list[i] = S(rel + "_" + i);
            return list;
        }

        // Boot scene ----------------------------------------------------------------
        static void BuildBootScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            MakeCamera();
            new GameObject("Boot", typeof(Boot));
            Directory.CreateDirectory("Assets/Scenes");
            EditorSceneManager.SaveScene(scene, BootScenePath);
        }

        static void MakeCamera()
        {
            var cam = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener)).GetComponent<Camera>();
            cam.tag = "MainCamera";
            cam.orthographic = true;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color32(0x68, 0xbc, 0xf0, 0xff);
            cam.transform.position = new Vector3(0f, 0f, -10f);
        }

        // Main menu scene -------------------------------------------------------------
        static void BuildMenuScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            MakeCamera();

            var es = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            es.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();

            var canvasGo = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.layer = 5;
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            scaler.referencePixelsPerUnit = 100f;
            var root = (RectTransform)canvasGo.transform;

            var c = new GameObject("MenuController", typeof(MainMenuController)).GetComponent<MainMenuController>();

            BuildWorld(root);

            var ui = Stretch(Rect("SafeArea", root));
            ui.gameObject.AddComponent<SafeAreaFitter>();

            BuildMain(ui, c);
            BuildEpisodes(ui, c);
            BuildDifficulties(ui, c);
            BuildLevels(ui, c);
            c.student = BuildMenuStudent(ui);
            c.dimmer = Panel(Stretch(Rect("Dimmer", ui)), ScreenPanel.Style.Fade);
            Img(c.dimmer.transform, S("UI/solid"), new Color(Ink.r, Ink.g, Ink.b, 0.72f), raycast: true);
            BuildSettings(ui, c);
            BuildHowToPlay(ui, c);
            BuildExit(ui, c);
            BuildNameEntry(ui, c);
            BuildStudentPick(ui, c);

            var effects = Stretch(Rect("Effects", root));
            var juice = effects.gameObject.AddComponent<Juice>();
            juice.sparkle = S("UI/sparkle");
            juice.effectsLayer = effects;

            EditorUtility.SetDirty(c);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, MenuScenePath);
        }

        // World: the animated courtyard -----------------------------------------------
        static void BuildWorld(RectTransform root)
        {
            var world = Rect("World", root);
            world.anchorMin = world.anchorMax = new Vector2(0.5f, 0.5f);
            world.pivot = new Vector2(0.5f, 0.5f);
            world.sizeDelta = new Vector2(1920f, 1080f);
            world.gameObject.AddComponent<EnvelopeScaler>();

            ArtImage(world, "Background", S("Background/bg_courtyard"), 0, 0);

            var clouds = Stretch(Rect("Clouds", world));
            AddCloud(clouds, "Cloud A", S("Background/cloud_big"), 250, 20, 10f);
            AddCloud(clouds, "Cloud B", S("Background/cloud_small"), 120, 45, 16f);
            AddCloud(clouds, "Cloud C", S("Background/cloud_small"), 360, 8, 7f);

            var birds = Stretch(Rect("Birds", world));
            birds.gameObject.AddComponent<BirdFlock>().frames = Frames("Background/bird", 2);

            var flag = ArtImage(world, "Flag", S("Background/flag_0"), 202, 40);
            var flagAnim = flag.gameObject.AddComponent<SpriteAnimator>();
            flagAnim.frames = Frames("Background/flag", 4);
            flagAnim.framesPerSecond = 7f;

            var leaves = Stretch(Rect("Leaves", world));
            var fall = leaves.gameObject.AddComponent<LeafFall>();
            fall.leaves = new[] { S("UI/leaf_0"), S("UI/leaf_1") };
            fall.spawnArea = new Rect(0f, 560f, 300f, 300f);
            fall.groundY = 250f;

            var walkers = Stretch(Rect("Students", world));
            AddWalker(walkers, "Student Girl", Frames("Characters/student_girl", 6), 128f, 2040f, -120f, 118f, 3.5f);
            AddWalker(walkers, "Student Boy", Frames("Characters/student_boy", 6), 96f, -120f, 2040f, 140f, 0.4f);
        }

        static RectTransform ArtImage(RectTransform parent, string name, Sprite sprite, int artX, int artY, float artScale = 1f)
        {
            var r = Rect(name, parent);
            r.anchorMin = r.anchorMax = Vector2.zero;
            r.pivot = Vector2.zero;
            var w = sprite.rect.width * Px * artScale;
            var h = sprite.rect.height * Px * artScale;
            r.sizeDelta = new Vector2(w, h);
            r.anchoredPosition = new Vector2(artX * Px, 1080f - artY * Px - h);
            Img(r, sprite, Color.white);
            return r;
        }

        static void AddCloud(RectTransform parent, string name, Sprite sprite, int artX, int artY, float speed)
        {
            var r = ArtImage(parent, name, sprite, artX, artY);
            var d = r.gameObject.AddComponent<Drifter>();
            d.speed = speed;
        }

        static void AddWalker(RectTransform parent, string name, Sprite[] frames, float y, float from, float to, float speed, float delay)
        {
            var r = Rect(name, parent);
            r.anchorMin = r.anchorMax = Vector2.zero;
            r.pivot = new Vector2(0.5f, 0f);
            r.sizeDelta = new Vector2(frames[0].rect.width * Px, frames[0].rect.height * Px);
            r.anchoredPosition = new Vector2(from, y);
            Img(r, frames[0], Color.white);
            var anim = r.gameObject.AddComponent<SpriteAnimator>();
            anim.frames = frames;
            anim.framesPerSecond = 9f;
            var w = r.gameObject.AddComponent<Walker>();
            w.fromX = from;
            w.toX = to;
            w.speed = speed;
            w.startDelay = delay;
        }

        // Main menu ------------------------------------------------------------------
        static void BuildMain(RectTransform ui, MainMenuController c)
        {
            var main = Stretch(Rect("Main Menu", ui));
            c.mainPanel = Panel(main, ScreenPanel.Style.Fade);

            var title = S("UI/title_logo");
            var titleR = Rect("Title", main);
            TopLeft(titleR, 90f, -28f, title.rect.width * Px, title.rect.height * Px);
            Img(titleR, title, Color.white);

            var ribbon = S("UI/subtitle_ribbon");
            var ribbonR = Rect("Subtitle", main);
            var rw = ribbon.rect.width * Px;
            TopLeft(ribbonR, 90f + (title.rect.width * Px - rw) / 2f, -28f - title.rect.height * Px - 4f, rw, ribbon.rect.height * Px);
            Img(ribbonR, ribbon, Color.white);

            const float x = 120f, w = 540f;
            c.playButton = MakeButton(main, "Play", "green", UIStrings.Play, f8, S("Icons/icon_play"), Sfx.Open);
            TopLeft((RectTransform)c.playButton.transform, x, -396f, w, 140f);
            c.playButton.GetComponent<PressBounce>().pulse = true;

            c.continueButton = MakeButton(main, "Continue", "blue", UIStrings.Continue, f6, S("Icons/icon_continue"), Sfx.Click);
            TopLeft((RectTransform)c.continueButton.transform, x, -554f, w, 116f);
            c.continueButton.gameObject.AddComponent<DisabledClickRelay>();

            c.settingsButton = MakeButton(main, "Settings", "orange", UIStrings.Settings, f6, S("Icons/icon_settings"), Sfx.Open);
            TopLeft((RectTransform)c.settingsButton.transform, x, -688f, w, 116f);

            c.howToPlayButton = MakeButton(main, "How To Play", "teal", UIStrings.HowToPlay, f6, S("Icons/icon_help"), Sfx.Open);
            TopLeft((RectTransform)c.howToPlayButton.transform, x, -822f, w, 116f);

            c.exitButton = MakeButton(main, "Exit", "red", null, null, S("Icons/icon_exit"), Sfx.Open);
            var er = (RectTransform)c.exitButton.transform;
            er.anchorMin = er.anchorMax = new Vector2(1f, 1f);
            er.pivot = new Vector2(1f, 1f);
            er.sizeDelta = new Vector2(128f, 128f);
            er.anchoredPosition = new Vector2(-36f, -36f);
        }

        // Episode Selection -------------------------------------------------------------
        static readonly string[] EpisodeColors = { "orange", "red", "purple", "blue", "brown" };
        static readonly string[] EpisodeArt = { "earthquake", "fire", "typhoon", "flood", "landslide" };

        static void BuildEpisodes(RectTransform ui, MainMenuController c)
        {
            var page = Stretch(Rect("Episode Selection", ui));
            c.episodePanel = Panel(page, ScreenPanel.Style.Slide);
            Backdrop(page);
            Header(page, UIStrings.EpisodeTitle, 760f);

            c.episodeCards = new EpisodeCard[5];
            c.episodeEmblems = new Sprite[5];
            for (var i = 0; i < 5; i++)
            {
                var episode = Curriculum.Episodes[i];
                var emblemSprite = S("Episodes/episode_" + EpisodeArt[i]);
                c.episodeEmblems[i] = emblemSprite;

                var b = MakeButton(page, episode.ToString(), EpisodeColors[i], null, null, null, Sfx.Click);
                var r = (RectTransform)b.transform;
                Centered(r, -672f + i * 336f, 10f, 300f, 400f);
                var face = b.GetComponent<PressBounce>().face;

                var emblem = Rect("Emblem", face);
                emblem.anchorMin = emblem.anchorMax = new Vector2(0.5f, 1f);
                emblem.pivot = new Vector2(0.5f, 1f);
                emblem.sizeDelta = new Vector2(200f, 200f);
                emblem.anchoredPosition = new Vector2(0f, -36f);
                var emblemImg = Img(emblem, emblemSprite, Color.white);

                var label = Rect("Label", face);
                label.anchorMin = label.anchorMax = new Vector2(0.5f, 0f);
                label.pivot = new Vector2(0.5f, 0f);
                label.sizeDelta = new Vector2(280f, 60f);
                label.anchoredPosition = new Vector2(0f, 44f);
                var text = Label(label, UIStrings.EpisodeName(episode), f4, Color.white, TextAnchor.MiddleCenter);

                var lockBadge = LockBadge(face, new Vector2(0f, -136f), new Vector2(0.5f, 1f));

                var card = b.gameObject.AddComponent<EpisodeCard>();
                card.episode = episode;
                card.button = b;
                card.face = b.GetComponent<Image>();
                card.emblem = emblemImg;
                card.label = text;
                card.lockBadge = lockBadge;
                c.episodeCards[i] = card;
            }
            c.episodeBack = BackButton(page);
        }

        // Difficulty Selection ------------------------------------------------------------
        static readonly string[] DifficultyColors = { "green", "orange", "red" };

        static void BuildDifficulties(RectTransform ui, MainMenuController c)
        {
            var page = Stretch(Rect("Difficulty Selection", ui));
            c.difficultyPanel = Panel(page, ScreenPanel.Style.Slide);
            Backdrop(page);
            var header = Header(page, "Earthquake", 760f);
            c.difficultyHeader = header.GetComponentInChildren<Text>();
            var emblem = Rect("Emblem", header);
            emblem.anchorMin = emblem.anchorMax = new Vector2(0f, 0.5f);
            emblem.pivot = new Vector2(0f, 0.5f);
            emblem.sizeDelta = new Vector2(120f, 120f);
            emblem.anchoredPosition = new Vector2(24f, 6f);
            c.difficultyEmblem = Img(emblem, S("Episodes/episode_earthquake"), Color.white);

            var hat = S("Icons/icon_hardhat");
            c.difficultyCards = new DifficultyCard[3];
            for (var i = 0; i < 3; i++)
            {
                var d = Curriculum.Difficulties[i];
                var b = MakeButton(page, d.ToString(), DifficultyColors[i], null, null, null, Sfx.Click);
                Centered((RectTransform)b.transform, -480f + i * 480f, 0f, 420f, 440f);
                var face = b.GetComponent<PressBounce>().face;

                var title = Rect("Label", face);
                title.anchorMin = title.anchorMax = new Vector2(0.5f, 1f);
                title.pivot = new Vector2(0.5f, 1f);
                title.sizeDelta = new Vector2(380f, 100f);
                title.anchoredPosition = new Vector2(0f, -40f);
                var label = Label(title, UIStrings.DifficultyName(d), f8, Color.white, TextAnchor.MiddleCenter);

                var hats = new Image[i + 1];
                for (var h = 0; h <= i; h++)
                {
                    var hr = Rect("Hard Hat " + (h + 1), face);
                    Centered(hr, (h - i / 2f) * 92f, 0f, hat.rect.width * Px, hat.rect.height * Px);
                    hats[h] = Img(hr, hat, Color.white);
                }

                var countR = Rect("Count", face);
                countR.anchorMin = countR.anchorMax = new Vector2(0.5f, 0f);
                countR.pivot = new Vector2(0.5f, 0f);
                countR.sizeDelta = new Vector2(380f, 60f);
                countR.anchoredPosition = new Vector2(0f, 44f);
                var count = Label(countR, UIStrings.Levels(Curriculum.LevelCount(d)), f4, Color.white, TextAnchor.MiddleCenter);

                // Pokes out of the top-right corner so it never covers the label.
                var lockBadge = LockBadge(face, new Vector2(-10f, -6f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f));

                var card = b.gameObject.AddComponent<DifficultyCard>();
                card.difficulty = d;
                card.button = b;
                card.face = b.GetComponent<Image>();
                card.label = label;
                card.count = count;
                card.lockBadge = lockBadge;
                card.hats = hats;
                c.difficultyCards[i] = card;
            }
            c.difficultyBack = BackButton(page);
        }

        // Level Selection -------------------------------------------------------------------
        static void BuildLevels(RectTransform ui, MainMenuController c)
        {
            var page = Stretch(Rect("Level Selection", ui));
            c.levelPanel = Panel(page, ScreenPanel.Style.Slide);
            Backdrop(page);
            var header = Header(page, "Earthquake - Easy", 900f);
            c.levelHeader = header.GetComponentInChildren<Text>();

            var row = Rect("Slots", page);
            Centered(row, 0f, 40f, 1600f, 270f);
            var layout = row.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 24f;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = layout.childControlHeight = false;
            layout.childForceExpandWidth = layout.childForceExpandHeight = false;

            var starOn = S("Icons/icon_star");
            var starOff = S("Icons/icon_starEmpty");
            c.levelSlots = new LevelSlot[7];
            for (var i = 0; i < 7; i++)
            {
                var slotR = Rect("Level " + (i + 1), row);
                slotR.sizeDelta = new Vector2(200f, 240f);
                var frame = Stretch(Rect("Selected", slotR));
                frame.offsetMin = new Vector2(-14f, -14f);
                frame.offsetMax = new Vector2(14f, 14f);
                Img(frame, S("UI/button_yellow"), Color.white, sliced: true);

                var b = MakeButton(slotR, "Tile", "white", null, null, null, Sfx.Click);
                Stretch((RectTransform)b.transform);
                var face = b.GetComponent<PressBounce>().face;

                var numR = Rect("Number", face);
                numR.anchorMin = numR.anchorMax = new Vector2(0.5f, 1f);
                numR.pivot = new Vector2(0.5f, 1f);
                numR.sizeDelta = new Vector2(180f, 110f);
                numR.anchoredPosition = new Vector2(0f, -24f);
                var number = Label(numR, (i + 1).ToString(), f8, Ink, TextAnchor.MiddleCenter, shadow: false);

                var stars = new Image[3];
                for (var s = 0; s < 3; s++)
                {
                    var sr = Rect("Star " + (s + 1), face);
                    sr.anchorMin = sr.anchorMax = new Vector2(0.5f, 0f);
                    sr.pivot = new Vector2(0.5f, 0f);
                    sr.sizeDelta = new Vector2(48f, 48f);
                    sr.anchoredPosition = new Vector2((s - 1) * 54f, 30f + (s == 1 ? 12f : 0f));
                    stars[s] = Img(sr, starOff, Color.white);
                }

                var slot = slotR.gameObject.AddComponent<LevelSlot>();
                slot.button = b;
                slot.number = number;
                slot.stars = stars;
                slot.selectedFrame = frame.gameObject;
                slot.starOn = starOn;
                slot.starOff = starOff;
                c.levelSlots[i] = slot;
            }

            c.startButton = MakeButton(page, "Start", "green", UIStrings.Start, f8, S("Icons/icon_play"), Sfx.Confirm);
            var st = (RectTransform)c.startButton.transform;
            st.anchorMin = st.anchorMax = new Vector2(0.5f, 0f);
            st.pivot = new Vector2(0.5f, 0f);
            st.sizeDelta = new Vector2(440f, 140f);
            st.anchoredPosition = new Vector2(0f, 48f);
            c.startButton.interactable = false;

            c.levelBack = BackButton(page);
        }

        // The Learner's Student on the Main Menu, in Bantay's old corner (bottom right). Its speech bubble is
        // wide enough for the longest menu line in f4, and sits above and to the left, as Bantay's did.
        static MenuStudent BuildMenuStudent(RectTransform ui)
        {
            const float scale = 1.25f;
            var actor = BuildStudent(ui, withBubble: false);
            actor.name = "Menu Student";
            var r = (RectTransform)actor.transform;
            r.anchorMin = r.anchorMax = new Vector2(1f, 0f);
            r.pivot = new Vector2(1f, 0f);
            r.sizeDelta = new Vector2(32f * Px * scale, 50f * Px * scale);
            r.anchoredPosition = new Vector2(-40f, 20f);
            actor.body.raycastTarget = true;

            var bubble = BuildSpeechBubble(r, 620f, 150f);
            var br = (RectTransform)bubble.transform;
            br.anchorMin = br.anchorMax = new Vector2(0.5f, 1f);
            br.pivot = new Vector2(0.9f, 0f);
            br.anchoredPosition = new Vector2(-10f, 12f);
            bubble.Hide();
            actor.bubble = bubble;

            var student = actor.gameObject.AddComponent<MenuStudent>();
            student.actor = actor;
            return student;
        }

        // Bantay (the card flow in the Level scene only, until the cleanup) ---------------------------
        static BantayGuide BuildBantay(RectTransform ui)
        {
            var frames = Frames("Characters/bantay", 4);
            var r = Rect("Bantay", ui);
            r.anchorMin = r.anchorMax = new Vector2(1f, 0f);
            r.pivot = new Vector2(1f, 0f);
            r.sizeDelta = new Vector2(frames[0].rect.width * Px, frames[0].rect.height * Px);
            r.anchoredPosition = new Vector2(-40f, 20f);

            var body = Stretch(Rect("Body", r));
            var bodyImg = Img(body, frames[0], Color.white, raycast: true);

            var bubbleR = Rect("Bubble", r);
            bubbleR.anchorMin = bubbleR.anchorMax = new Vector2(0.5f, 1f);
            bubbleR.pivot = new Vector2(0.9f, 0f);
            bubbleR.sizeDelta = new Vector2(620f, 120f);
            bubbleR.anchoredPosition = new Vector2(-10f, 12f);
            var bubblePanel = Panel(bubbleR, ScreenPanel.Style.Pop);
            Img(bubbleR, S("UI/bubble"), Color.white, sliced: true);
            var v = bubbleR.gameObject.AddComponent<VerticalLayoutGroup>();
            v.padding = new RectOffset(32, 32, 28, 36);
            v.childControlHeight = v.childControlWidth = true;
            v.childForceExpandHeight = false;
            var fit = bubbleR.gameObject.AddComponent<ContentSizeFitter>();
            fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var textR = Rect("Text", bubbleR);
            var text = Label(textR, string.Empty, f4, Ink, TextAnchor.MiddleLeft, shadow: false);
            text.horizontalOverflow = HorizontalWrapMode.Wrap;

            var tail = S("UI/bubble_tail");
            var tailR = Rect("Tail", bubbleR);
            var le = tailR.gameObject.AddComponent<LayoutElement>();
            le.ignoreLayout = true;
            tailR.anchorMin = tailR.anchorMax = new Vector2(0.9f, 0f);
            tailR.pivot = new Vector2(0.5f, 1f);
            tailR.sizeDelta = new Vector2(tail.rect.width * Px, tail.rect.height * Px);
            tailR.anchoredPosition = new Vector2(0f, 8f);
            Img(tailR, tail, Color.white);

            var guide = r.gameObject.AddComponent<BantayGuide>();
            guide.frames = frames;
            guide.body = bodyImg;
            guide.bubble = bubblePanel;
            guide.bubbleText = text;
            return guide;
        }

        // Dialogs ---------------------------------------------------------------------------
        static RectTransform Dialog(RectTransform ui, string name, float w, float h, out ScreenPanel panel)
        {
            var holder = Stretch(Rect(name, ui));
            var box = Rect("Box", holder);
            Centered(box, 0f, 0f, w, h);
            Img(box, S("UI/panel"), Color.white, sliced: true, raycast: true);
            panel = Panel(holder, ScreenPanel.Style.Pop);
            panel.content = box;
            return box;
        }

        static void DialogTitle(RectTransform box, string text, Font font)
        {
            var r = Rect("Title", box);
            r.anchorMin = r.anchorMax = new Vector2(0.5f, 1f);
            r.pivot = new Vector2(0.5f, 1f);
            r.sizeDelta = new Vector2(box.sizeDelta.x - 80f, 100f);
            r.anchoredPosition = new Vector2(0f, -44f);
            Label(r, text, font, Ink, TextAnchor.MiddleCenter, shadow: false);
        }

        static Button DialogButton(RectTransform box, string name, string color, string label, Sprite icon, Sfx sfx, float x, float w)
        {
            var b = MakeButton(box, name, color, label, f6, icon, sfx);
            var r = (RectTransform)b.transform;
            r.anchorMin = r.anchorMax = new Vector2(0.5f, 0f);
            r.pivot = new Vector2(0.5f, 0f);
            r.sizeDelta = new Vector2(w, 120f);
            r.anchoredPosition = new Vector2(x, 48f);
            return b;
        }

        static void BuildSettings(RectTransform ui, MainMenuController c)
        {
            var box = Dialog(ui, "Settings", 1000f, 720f, out c.settingsPanel);
            DialogTitle(box, UIStrings.SettingsTitle, f8);
            c.musicSwitch = SettingRow(box, "Music", UIStrings.Music, S("Icons/icon_music"), 110f);
            c.soundSwitch = SettingRow(box, "Sounds", UIStrings.Sounds, S("Icons/icon_sound"), -40f);
            c.settingsBack = DialogButton(box, "Back", "orange", UIStrings.Back, S("Icons/icon_back"), Sfx.Back, 0f, 320f);
        }

        static SwitchButton SettingRow(RectTransform box, string name, string text, Sprite icon, float y)
        {
            var row = Rect(name, box);
            Centered(row, 0f, y, 760f, 130f);
            var bg = Img(row, S("UI/input_box"), new Color(1f, 1f, 1f, 0.55f), sliced: true);
            bg.raycastTarget = false;

            var ir = Rect("Icon", row);
            ir.anchorMin = ir.anchorMax = new Vector2(0f, 0.5f);
            ir.pivot = new Vector2(0f, 0.5f);
            ir.sizeDelta = new Vector2(icon.rect.width * Px, icon.rect.height * Px);
            ir.anchoredPosition = new Vector2(36f, 0f);
            Img(ir, icon, Color.white);

            var lr = Rect("Label", row);
            lr.anchorMin = lr.anchorMax = new Vector2(0f, 0.5f);
            lr.pivot = new Vector2(0f, 0.5f);
            lr.sizeDelta = new Vector2(360f, 90f);
            lr.anchoredPosition = new Vector2(140f, 0f);
            Label(lr, text, f6, Ink, TextAnchor.MiddleLeft, shadow: false);

            var on = S("UI/switch_on");
            var off = S("UI/switch_off");
            var sr = Rect("Switch", row);
            sr.anchorMin = sr.anchorMax = new Vector2(1f, 0.5f);
            sr.pivot = new Vector2(1f, 0.5f);
            sr.sizeDelta = new Vector2(200f, 128f);
            sr.anchoredPosition = new Vector2(-20f, 0f);
            var hit = Img(sr, S("UI/solid"), new Color(1f, 1f, 1f, 0f), raycast: true);
            var button = sr.gameObject.AddComponent<Button>();
            button.targetGraphic = hit;
            button.transition = Selectable.Transition.None;
            button.navigation = new Navigation { mode = Navigation.Mode.None };
            var bounce = sr.gameObject.AddComponent<PressBounce>();
            bounce.sound = Sfx.Toggle;
            var art = Rect("Art", sr);
            Centered(art, 0f, 0f, on.rect.width * Px, on.rect.height * Px);
            var img = Img(art, on, Color.white);

            var sw = sr.gameObject.AddComponent<SwitchButton>();
            sw.image = img;
            sw.onSprite = on;
            sw.offSprite = off;
            sw.button = button;
            return sw;
        }

        static void BuildHowToPlay(RectTransform ui, MainMenuController c)
        {
            var box = Dialog(ui, "How To Play", 1440f, 940f, out c.howToPlayPanel);
            var pages = UIStrings.TutorialPages;
            DialogTitle(box, UIStrings.TutorialPageTitle(0, pages[0].Title), f6);
            var title = box.Find("Title").GetComponent<Text>();

            // Picture: framed, animated, and swipeable.
            var holder = Rect("Picture", box);
            Centered(holder, 0f, 150f, 576f, 336f);
            Img(holder, S("UI/input_box"), Color.white, sliced: true, raycast: true);
            var pic = Rect("Art", holder);
            Centered(pic, 0f, 0f, 136f * Px, 76f * Px);
            var frames = new Sprite[pages.Length * 2];
            for (var i = 0; i < pages.Length; i++)
            {
                frames[i * 2] = S("Tutorial/tutorial_" + pages[i].Art + "_0");
                frames[i * 2 + 1] = S("Tutorial/tutorial_" + pages[i].Art + "_1");
            }
            var picImg = Img(pic, frames[0], Color.white);

            var prev = MakeButton(box, "Previous", "blue", null, null, S("Icons/icon_play"), Sfx.Click);
            Centered((RectTransform)prev.transform, -620f, 150f, 140f, 140f);
            // Mirror the Next arrow so Previous does not look like the orange BACK button.
            prev.transform.Find("Face/Icon").localScale = new Vector3(-1f, 1f, 1f);
            var next = MakeButton(box, "Next", "green", null, null, S("Icons/icon_play"), Sfx.Click);
            Centered((RectTransform)next.transform, 620f, 150f, 140f, 140f);

            var cr = Rect("Caption", box);
            Centered(cr, 0f, -60f, 1300f, 70f);
            var caption = Label(cr, pages[0].Caption, f4, Ink, TextAnchor.MiddleCenter, shadow: false);
            caption.horizontalOverflow = HorizontalWrapMode.Wrap;

            // The Learner's Student beside the Tip for this page.
            var guide = BuildStudent(box, withBubble: false);
            guide.name = "How To Play Student";
            Centered((RectTransform)guide.transform, -540f, -170f, 32f * Px * 0.75f, 50f * Px * 0.75f);
            var bubble = Rect("Tip Bubble", box);
            Centered(bubble, 80f, -170f, 1000f, 130f);
            Img(bubble, S("UI/bubble"), Color.white, sliced: true);
            var tr = Stretch(Rect("Tip", bubble));
            tr.offsetMin = new Vector2(32f, 16f);
            tr.offsetMax = new Vector2(-32f, -16f);
            var tip = Label(tr, pages[0].Tip, f4, Ink, TextAnchor.MiddleLeft, shadow: false);
            tip.horizontalOverflow = HorizontalWrapMode.Wrap;

            // Star page dots.
            var starOn = S("Icons/icon_star");
            var starOff = S("Icons/icon_starEmpty");
            var dots = new Image[pages.Length];
            for (var i = 0; i < pages.Length; i++)
            {
                var d = Rect("Dot " + (i + 1), box);
                Centered(d, (i - (pages.Length - 1) / 2f) * 64f, -275f, 48f, 48f);
                dots[i] = Img(d, i == 0 ? starOn : starOff, Color.white);
            }

            c.howToPlayBack = DialogButton(box, "Back", "orange", UIStrings.Back, S("Icons/icon_back"), Sfx.Back, 0f, 320f);

            var view = holder.gameObject.AddComponent<TutorialView>();
            view.title = title;
            view.caption = caption;
            view.tip = tip;
            view.picture = picImg;
            view.pictureHolder = holder;
            view.previous = prev;
            view.next = next;
            view.closeLabel = c.howToPlayBack.GetComponentInChildren<Text>();
            view.dots = dots;
            view.dotOn = starOn;
            view.dotOff = starOff;
            view.frames = frames;
            view.student = guide;
            c.tutorial = view;
        }

        static void BuildExit(RectTransform ui, MainMenuController c)
        {
            var box = Dialog(ui, "Exit Confirm", 940f, 540f, out c.exitPanel);
            DialogTitle(box, UIStrings.ExitTitle, f6);
            var br = Rect("Body", box);
            Centered(br, 0f, 50f, 800f, 80f);
            Label(br, UIStrings.ExitBody, f4, Ink, TextAnchor.MiddleCenter, shadow: false);
            c.exitYes = DialogButton(box, "Yes", "red", UIStrings.Yes, S("Icons/icon_exit"), Sfx.Click, -190f, 300f);
            c.exitNo = DialogButton(box, "No", "green", UIStrings.No, S("Icons/icon_back"), Sfx.Back, 190f, 300f);
        }

        static void BuildNameEntry(RectTransform ui, MainMenuController c)
        {
            var box = Dialog(ui, "Name Entry", 1160f, 680f, out c.namePanel);
            DialogTitle(box, UIStrings.NameTitle, f6);

            // Name Entry comes before the Student pick, so the boy and the girl stand side by side.
            foreach (var (kind, x) in new[] { ("boy", -460f), ("girl", -320f) })
            {
                var idle = S("Student/" + kind + "_idle_0");
                var sr = Rect(kind == "boy" ? "Boy" : "Girl", box);
                Centered(sr, x, 30f, idle.rect.width * Px, idle.rect.height * Px);
                Img(sr, idle, Color.white);
            }

            var fr = Rect("Name Field", box);
            Centered(fr, 110f, 60f, 640f, 128f);
            var fieldImg = Img(fr, S("UI/input_box"), Color.white, sliced: true, raycast: true);
            var textR = Stretch(Rect("Text", fr));
            textR.offsetMin = new Vector2(32f, 12f);
            textR.offsetMax = new Vector2(-32f, -12f);
            var text = Label(textR, string.Empty, f6, Ink, TextAnchor.MiddleLeft, shadow: false);
            text.supportRichText = false;
            var phR = Stretch(Rect("Placeholder", fr));
            phR.offsetMin = textR.offsetMin;
            phR.offsetMax = textR.offsetMax;
            var placeholder = Label(phR, UIStrings.NamePlaceholder, f6, HintGray, TextAnchor.MiddleLeft, shadow: false);
            var input = fr.gameObject.AddComponent<InputField>();
            input.targetGraphic = fieldImg;
            input.textComponent = text;
            input.placeholder = placeholder;
            input.lineType = InputField.LineType.SingleLine;
            input.caretWidth = 4;
            input.customCaretColor = true;
            input.caretColor = Ink;
            input.selectionColor = new Color32(0x9c, 0xd0, 0xff, 0xc0);
            input.navigation = new Navigation { mode = Navigation.Mode.None };
            c.nameInput = input;

            var er = Rect("Error", box);
            Centered(er, 110f, -40f, 640f, 60f);
            c.nameError = Label(er, string.Empty, f4, ErrorRed, TextAnchor.MiddleLeft, shadow: false);

            c.nameBack = DialogButton(box, "Back", "orange", UIStrings.Back, S("Icons/icon_back"), Sfx.Back, -220f, 320f);
            c.nameOk = DialogButton(box, "OK", "green", UIStrings.Ok, S("Icons/icon_check"), Sfx.Click, 220f, 320f);
        }

        static void BuildStudentPick(RectTransform ui, MainMenuController c)
        {
            var box = Dialog(ui, "Student Pick", 1160f, 760f, out c.studentPanel);
            DialogTitle(box, UIStrings.StudentTitle, f6);
            c.studentBoy = StudentChoice(box, "Boy", S("Student/boy_portrait"), UIStrings.StudentBoy, "blue", -260f);
            c.studentGirl = StudentChoice(box, "Girl", S("Student/girl_portrait"), UIStrings.StudentGirl, "orange", 260f);
        }

        static Button StudentChoice(RectTransform box, string name, Sprite portrait, string label, string color, float x)
        {
            const float scale = 6.5f;
            var pr = Rect(name + " Portrait", box);
            Centered(pr, x, 10f, portrait.rect.width * scale, portrait.rect.height * scale);
            Img(pr, portrait, Color.white);
            return DialogButton(box, name, color, label, null, Sfx.Click, x, 340f);
        }

        // Shared page parts ---------------------------------------------------------------
        static void Backdrop(RectTransform page)
        {
            var r = Stretch(Rect("Backdrop", page));
            r.SetAsFirstSibling();
            Img(r, S("UI/solid"), new Color(Ink.r, Ink.g, Ink.b, 0.45f), raycast: true);
        }

        static RectTransform Header(RectTransform page, string text, float width)
        {
            var r = Rect("Header", page);
            r.anchorMin = r.anchorMax = new Vector2(0.5f, 1f);
            r.pivot = new Vector2(0.5f, 1f);
            r.sizeDelta = new Vector2(width, 136f);
            r.anchoredPosition = new Vector2(0f, -40f);
            Img(r, S("UI/button_teal"), Color.white, sliced: true);
            var tr = Stretch(Rect("Text", r));
            tr.offsetMin = new Vector2(0f, 16f);
            Label(tr, text, f6, Color.white, TextAnchor.MiddleCenter);
            return r;
        }

        static Button BackButton(RectTransform page)
        {
            var b = MakeButton(page, "Back", "orange", UIStrings.Back, f6, S("Icons/icon_back"), Sfx.Back);
            var r = (RectTransform)b.transform;
            r.anchorMin = r.anchorMax = Vector2.zero;
            r.pivot = Vector2.zero;
            r.sizeDelta = new Vector2(320f, 124f);
            r.anchoredPosition = new Vector2(40f, 40f);
            return b;
        }

        static GameObject LockBadge(RectTransform parent, Vector2 pos, Vector2 anchor, Vector2? pivot = null)
        {
            var sprite = S("Icons/icon_lock");
            var r = Rect("Lock", parent);
            r.anchorMin = r.anchorMax = anchor;
            r.pivot = pivot ?? new Vector2(0.5f, 1f);
            r.sizeDelta = new Vector2(sprite.rect.width * Px * 1.5f, sprite.rect.height * Px * 1.5f);
            r.anchoredPosition = pos;
            Img(r, sprite, Color.white);
            return r.gameObject;
        }

        // Building blocks -------------------------------------------------------------------
        static Button MakeButton(RectTransform parent, string name, string color, string label, Font font, Sprite icon, Sfx sfx)
        {
            var r = Rect(name, parent);
            var img = Img(r, S("UI/button_" + color), Color.white, sliced: true, raycast: true);
            var b = r.gameObject.AddComponent<Button>();
            b.targetGraphic = img;
            b.transition = Selectable.Transition.SpriteSwap;
            b.spriteState = new SpriteState
            {
                pressedSprite = S("UI/button_" + color + "_pressed"),
                disabledSprite = S("UI/button_gray"),
            };
            b.navigation = new Navigation { mode = Navigation.Mode.None };

            var face = Stretch(Rect("Face", r));
            face.offsetMin = new Vector2(0f, 16f); // sit above the 4-pixel lip

            if (label != null || icon != null)
            {
                var h = face.gameObject.AddComponent<HorizontalLayoutGroup>();
                h.childAlignment = TextAnchor.MiddleCenter;
                h.spacing = 20f;
                h.childControlWidth = h.childControlHeight = true;
                h.childForceExpandWidth = h.childForceExpandHeight = false;
                if (icon != null)
                {
                    var ir = Rect("Icon", face);
                    var iconImg = Img(ir, icon, Color.white);
                    var le = ir.gameObject.AddComponent<LayoutElement>();
                    le.preferredWidth = icon.rect.width * Px;
                    le.preferredHeight = icon.rect.height * Px;
                    iconImg.preserveAspect = true;
                }
                if (label != null)
                {
                    var lr = Rect("Label", face);
                    Label(lr, label, font, Color.white, TextAnchor.MiddleCenter);
                }
            }

            var bounce = r.gameObject.AddComponent<PressBounce>();
            bounce.face = face;
            bounce.sound = sfx;
            return b;
        }

        static ScreenPanel Panel(RectTransform r, ScreenPanel.Style style)
        {
            r.gameObject.AddComponent<CanvasGroup>();
            var p = r.gameObject.AddComponent<ScreenPanel>();
            p.style = style;
            return p;
        }

        static RectTransform Rect(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.layer = 5;
            var r = (RectTransform)go.transform;
            r.SetParent(parent, false);
            return r;
        }

        static RectTransform Stretch(RectTransform r)
        {
            r.anchorMin = Vector2.zero;
            r.anchorMax = Vector2.one;
            r.pivot = new Vector2(0.5f, 0.5f);
            r.offsetMin = r.offsetMax = Vector2.zero;
            return r;
        }

        static void TopLeft(RectTransform r, float x, float y, float w, float h)
        {
            r.anchorMin = r.anchorMax = new Vector2(0f, 1f);
            r.pivot = new Vector2(0f, 1f);
            r.sizeDelta = new Vector2(w, h);
            r.anchoredPosition = new Vector2(x, y);
        }

        static void Centered(RectTransform r, float x, float y, float w, float h)
        {
            r.anchorMin = r.anchorMax = new Vector2(0.5f, 0.5f);
            r.pivot = new Vector2(0.5f, 0.5f);
            r.sizeDelta = new Vector2(w, h);
            r.anchoredPosition = new Vector2(x, y);
        }

        static Image Img(Transform t, Sprite sprite, Color color, bool sliced = false, bool raycast = false)
        {
            var img = t.gameObject.AddComponent<Image>();
            img.sprite = sprite;
            img.color = color;
            img.type = sliced ? Image.Type.Sliced : Image.Type.Simple;
            img.raycastTarget = raycast;
            return img;
        }

        static Text Label(RectTransform r, string text, Font font, Color color, TextAnchor anchor, bool shadow = true)
        {
            var t = r.gameObject.AddComponent<Text>();
            t.font = font;
            t.fontSize = 0; // custom fonts draw at their own size
            t.text = text;
            t.color = color;
            t.alignment = anchor;
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            t.supportRichText = false;
            t.raycastTarget = false;
            if (shadow)
            {
                var s = r.gameObject.AddComponent<Shadow>();
                s.effectColor = Ink;
                s.effectDistance = new Vector2(4f, -4f);
            }
            return t;
        }
    }
}
