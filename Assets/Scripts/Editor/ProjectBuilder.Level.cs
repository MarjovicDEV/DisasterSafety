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
    /// The one reusable Level scene: it loads whichever Scenario GameServices.CurrentLevel names.
    /// Layout must fit 1920x1080 and wide phones (2400x1080 scales to about 2147x966 units).
    /// </summary>
    public static partial class ProjectBuilder
    {
        public const string LevelScenePath = "Assets/Scenes/Level.unity";

        static readonly string[] ChoicePictureNames =
        {
            "choice_desk", "choice_run", "choice_window", "choice_cabinet",
            "choice_cover", "choice_stairs", "choice_hall_window", "choice_plants",
            "choice_walk", "choice_push", "choice_stay",
        };

        static void BuildLevelScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            MakeCamera();
            Object.FindAnyObjectByType<Camera>().backgroundColor = new Color32(0xf6, 0xe6, 0xb8, 0xff);

            var es = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            es.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();

            var canvasGo = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.layer = 5;
            canvasGo.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            scaler.referencePixelsPerUnit = 100f;
            var root = (RectTransform)canvasGo.transform;

            var c = new GameObject("LevelController", typeof(LevelController)).GetComponent<LevelController>();

            var world = Rect("World", root);
            world.anchorMin = world.anchorMax = new Vector2(0.5f, 0.5f);
            world.pivot = new Vector2(0.5f, 0.5f);
            world.sizeDelta = new Vector2(1920f, 1080f);
            world.gameObject.AddComponent<EnvelopeScaler>();
            c.world = world;

            c.settings = new[] { BuildClassroom(world), BuildCorridor(world), BuildClassroomAfter(world) };
            for (var i = 1; i < c.settings.Length; i++)
                c.settings[i].gameObject.SetActive(false);

            var ui = Stretch(Rect("SafeArea", root));
            ui.gameObject.AddComponent<SafeAreaFitter>();

            BuildLevelTopBar(ui, c);
            BuildChoices(ui, c);
            c.guidance = Stretch(Rect("Guidance", ui));
            c.bantay = BuildBantay(c.guidance);
            ((RectTransform)c.bantay.transform).anchoredPosition = new Vector2(-40f, 384f);
            BuildHintButton(c.guidance, c);

            c.dimmer = Panel(Stretch(Rect("Dimmer", ui)), ScreenPanel.Style.Fade);
            Img(c.dimmer.transform, S("UI/solid"), new Color(Ink.r, Ink.g, Ink.b, 0.6f), raycast: true);
            BuildFeedback(ui, c);
            BuildLevelCompleted(ui, c);
            BuildPause(ui, c);

            var effects = Stretch(Rect("Effects", root));
            var juice = effects.gameObject.AddComponent<Juice>();
            juice.sparkle = S("UI/sparkle");
            juice.effectsLayer = effects;
            var confetti = effects.gameObject.AddComponent<Confetti>();
            confetti.piece = S("UI/solid");
            confetti.layer = effects;
            c.confetti = confetti;

            var pictures = new Sprite[ChoicePictureNames.Length];
            for (var i = 0; i < pictures.Length; i++) pictures[i] = S("Level/" + ChoicePictureNames[i]);
            c.pictures = pictures;

            EditorUtility.SetDirty(c);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, LevelScenePath);
        }

        static LevelSetting BuildClassroom(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.Classroom, "Classroom", out var shaking);
            setting.shaker.wobblers = new[]
            {
                BuildLevelAnimation(shaking, "Ceiling Fan", Frames("Level/fan", 2), 204, 0, 6f, wobbles: true),
                BuildLevelAnimation(shaking, "Tall Cabinet", Frames("Level/cabinet", 2), 364, 64, 5f, wobbles: true),
            };
            return setting;
        }

        static LevelSetting BuildCorridor(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.Corridor, "Corridor", out var shaking);
            // Keep the pots in frame on wide screens and the Goal clear of Bantay's bubble.
            setting.alignArtTop = true;
            setting.goalPosition = new Vector2(-212f, -252f);
            var pots = Frames("Level/pots", 2);
            setting.shaker.wobblers = new[]
            {
                BuildLevelAnimation(shaking, "Hanging Pot Left", pots, 116, 12, 4f, wobbles: true),
                BuildLevelAnimation(shaking, "Hanging Pot Right", pots, 236, 12, 4f, wobbles: true),
            };
            return setting;
        }

        static LevelSetting BuildClassroomAfter(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.ClassroomAfter, "Classroom After", out var foreground, shakes: false);
            // Keep Bantay and the Hint clear of the teacher in the doorway.
            setting.guidanceOffsetX = -880f;
            BuildLevelAnimation(foreground, "Teacher", Frames("Level/teacher", 2), 418, 126, 3f, wobbles: false);
            return setting;
        }

        // A still copy behind the foreground keeps the edges covered during shaking.
        static LevelSetting BuildSetting(RectTransform world, string key, string name, out RectTransform foreground, bool shakes = true)
        {
            var group = Stretch(Rect(name, world));
            var setting = group.gameObject.AddComponent<LevelSetting>();
            setting.key = key;

            var background = S("Level/bg_" + key);
            ArtImage(group, "Backing", background, 0, 0);
            foreground = Stretch(Rect(shakes ? "Shaking" : "Still", group));
            ArtImage(foreground, name, background, 0, 0);

            if (shakes)
            {
                setting.shaker = foreground.gameObject.AddComponent<SceneShaker>();
                setting.shaker.target = foreground;
            }
            return setting;
        }

        static SpriteAnimator BuildLevelAnimation(RectTransform parent, string name, Sprite[] frames, int x, int y, float fps, bool wobbles)
        {
            var image = ArtImage(parent, name, frames[0], x, y);
            var animator = image.gameObject.AddComponent<SpriteAnimator>();
            animator.frames = frames;
            animator.framesPerSecond = fps;
            animator.randomStart = wobbles;
            animator.enabled = !wobbles;
            return animator;
        }

        static void BuildLevelTopBar(RectTransform ui, LevelController c)
        {
            // Level label chip, top left
            var chip = Rect("Level Label", ui);
            TopLeft(chip, 32f, -32f, 420f, 88f);
            Img(chip, S("UI/button_white"), Color.white, sliced: true);
            var chipText = Stretch(Rect("Text", chip));
            chipText.offsetMin = new Vector2(16f, 16f);
            chipText.offsetMax = new Vector2(-16f, 0f);
            c.levelLabel = Label(chipText, string.Empty, f3, Ink, TextAnchor.MiddleCenter, shadow: false);

            // Goal banner, top centre
            var goal = Rect("Goal", ui);
            c.goalBanner = goal;
            goal.anchorMin = goal.anchorMax = new Vector2(0.5f, 1f);
            goal.pivot = new Vector2(0.5f, 1f);
            goal.sizeDelta = new Vector2(900f, 120f);
            goal.anchoredPosition = new Vector2(0f, -24f);
            Img(goal, S("UI/button_teal"), Color.white, sliced: true);
            var goalText = Stretch(Rect("Text", goal));
            goalText.offsetMin = new Vector2(24f, 16f);
            goalText.offsetMax = new Vector2(-24f, 0f);
            c.goalText = Label(goalText, string.Empty, f4, Color.white, TextAnchor.MiddleCenter);
            c.goalText.horizontalOverflow = HorizontalWrapMode.Wrap;

            // Pause, top right
            c.pauseButton = MakeButton(ui, "Pause", "blue", null, null, S("Icons/icon_pause"), Sfx.Open);
            var pr = (RectTransform)c.pauseButton.transform;
            pr.anchorMin = pr.anchorMax = new Vector2(1f, 1f);
            pr.pivot = new Vector2(1f, 1f);
            pr.sizeDelta = new Vector2(128f, 128f);
            pr.anchoredPosition = new Vector2(-32f, -28f);
        }

        static void BuildHintButton(RectTransform ui, LevelController c)
        {
            c.hintButton = MakeButton(ui, "Hint", "yellow", UIStrings.Hint, f6, S("Icons/icon_hint"), Sfx.Click);
            var hr = (RectTransform)c.hintButton.transform;
            hr.anchorMin = hr.anchorMax = new Vector2(1f, 0f);
            hr.pivot = new Vector2(1f, 0f);
            hr.sizeDelta = new Vector2(290f, 124f);
            hr.anchoredPosition = new Vector2(-250f, 400f);
            // Dark label on yellow reads better than white.
            var label = hr.GetComponentInChildren<Text>();
            label.color = Ink;
            Object.DestroyImmediate(label.GetComponent<Shadow>());
        }

        static void BuildChoices(RectTransform ui, LevelController c)
        {
            var holder = Stretch(Rect("Choices", ui));
            c.choicesPanel = Panel(holder, ScreenPanel.Style.Slide);
            var row = Rect("Row", holder);
            row.anchorMin = row.anchorMax = new Vector2(0.5f, 0f);
            row.pivot = new Vector2(0.5f, 0f);
            row.sizeDelta = new Vector2(1720f, 340f);
            row.anchoredPosition = new Vector2(0f, 28f);
            var layout = row.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 28f;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = layout.childControlHeight = false;
            layout.childForceExpandWidth = layout.childForceExpandHeight = false;
            c.choicesPanel.content = row;

            c.choiceCards = new ChoiceCard[4];
            for (var i = 0; i < 4; i++)
            {
                var b = MakeButton(row, "Choice " + (i + 1), "white", null, null, null, Sfx.Click);
                var r = (RectTransform)b.transform;
                r.sizeDelta = new Vector2(400f, 340f);
                var face = b.GetComponent<PressBounce>().face;

                var picR = Rect("Picture", face);
                picR.anchorMin = picR.anchorMax = new Vector2(0.5f, 1f);
                picR.pivot = new Vector2(0.5f, 1f);
                picR.sizeDelta = new Vector2(72f * Px, 52f * Px);
                picR.anchoredPosition = new Vector2(0f, -20f);
                var pic = Img(picR, S("Level/choice_desk"), Color.white);

                var labelR = Rect("Label", face);
                labelR.anchorMin = labelR.anchorMax = new Vector2(0.5f, 0f);
                labelR.pivot = new Vector2(0.5f, 0f);
                labelR.sizeDelta = new Vector2(372f, 96f);
                labelR.anchoredPosition = new Vector2(0f, 4f);
                var label = Label(labelR, string.Empty, f4, Ink, TextAnchor.MiddleCenter, shadow: false);
                label.horizontalOverflow = HorizontalWrapMode.Wrap;
                label.lineSpacing = 0.9f;

                // Soft grey "Tried" chip for an Unsafe Choice already picked (never a red X).
                var tried = Rect("Tried", face);
                tried.anchorMin = tried.anchorMax = new Vector2(1f, 1f);
                tried.pivot = new Vector2(1f, 1f);
                tried.sizeDelta = new Vector2(150f, 64f);
                tried.anchoredPosition = new Vector2(-8f, -8f);
                Img(tried, S("UI/button_gray"), Color.white, sliced: true);
                var triedText = Stretch(Rect("Text", tried));
                triedText.offsetMin = new Vector2(0f, 10f);
                Label(triedText, UIStrings.Tried, f3, Color.white, TextAnchor.MiddleCenter);

                var card = b.gameObject.AddComponent<ChoiceCard>();
                card.button = b;
                card.picture = pic;
                card.label = label;
                card.triedMark = tried.gameObject;
                card.group = b.gameObject.AddComponent<CanvasGroup>();
                tried.gameObject.SetActive(false);
                c.choiceCards[i] = card;
            }
        }

        static void BuildFeedback(RectTransform ui, LevelController c)
        {
            var box = Dialog(ui, "Feedback", 1400f, 660f, out c.feedbackPanel);

            var banner = Rect("Banner", box);
            banner.anchorMin = banner.anchorMax = new Vector2(0.5f, 1f);
            banner.pivot = new Vector2(0.5f, 1f);
            banner.sizeDelta = new Vector2(1320f, 124f);
            banner.anchoredPosition = new Vector2(0f, -32f);
            c.feedbackBanner = Img(banner, S("UI/button_green"), Color.white, sliced: true);
            c.safeBanner = S("UI/button_green");
            c.unsafeBanner = S("UI/button_orange");
            var headingR = Stretch(Rect("Heading", banner));
            headingR.offsetMin = new Vector2(0f, 16f);
            c.feedbackHeading = Label(headingR, UIStrings.SafeHeading, f8, Color.white, TextAnchor.MiddleCenter);

            var holder = Rect("Picture", box);
            Centered(holder, -400f, -10f, 400f, 296f);
            Img(holder, S("UI/input_box"), Color.white, sliced: true);
            var pic = Rect("Art", holder);
            Centered(pic, 0f, 0f, 72f * 5f, 52f * 5f);
            c.feedbackPicture = Img(pic, S("Level/choice_desk"), Color.white);

            var textR = Rect("Text", box);
            Centered(textR, 250f, -10f, 780f, 290f);
            c.feedbackText = Label(textR, string.Empty, f4, Ink, TextAnchor.MiddleLeft, shadow: false);
            c.feedbackText.horizontalOverflow = HorizontalWrapMode.Wrap;
            c.feedbackText.lineSpacing = 1.15f;

            c.tryAgainButton = DialogButton(box, "Try Again", "blue", UIStrings.TryAgain, S("Icons/icon_replay"), Sfx.Back, 0f, 460f);
            c.finishButton = DialogButton(box, "Finish", "green", UIStrings.Finish, S("Icons/icon_check"), Sfx.Confirm, 0f, 460f);
            ((RectTransform)c.tryAgainButton.transform).anchoredPosition = new Vector2(0f, 36f);
            ((RectTransform)c.finishButton.transform).anchoredPosition = new Vector2(0f, 36f);
        }

        static void BuildLevelCompleted(RectTransform ui, LevelController c)
        {
            var box = Dialog(ui, "Level Completed", 1560f, 860f, out c.completedPanel);

            var ribbon = Rect("Title", box);
            ribbon.anchorMin = ribbon.anchorMax = new Vector2(0.5f, 1f);
            ribbon.pivot = new Vector2(0.5f, 1f);
            ribbon.sizeDelta = new Vector2(900f, 124f);
            ribbon.anchoredPosition = new Vector2(0f, -24f);
            Img(ribbon, S("UI/button_teal"), Color.white, sliced: true);
            var tr = Stretch(Rect("Text", ribbon));
            tr.offsetMin = new Vector2(0f, 16f);
            Label(tr, UIStrings.LevelCompletedTitle, f6, Color.white, TextAnchor.MiddleCenter);

            // Big Stars, left of the cheering Bantay
            var starOn = S("Icons/icon_star");
            var starOff = S("Icons/icon_starEmpty");
            var reveal = box.gameObject.AddComponent<StarReveal>();
            reveal.starOn = starOn;
            reveal.starOff = starOff;
            reveal.stars = new Image[3];
            for (var i = 0; i < 3; i++)
            {
                var sr = Rect("Star " + (i + 1), box);
                sr.anchorMin = sr.anchorMax = new Vector2(0.5f, 1f);
                sr.pivot = new Vector2(0.5f, 0.5f);
                sr.sizeDelta = new Vector2(160f, 160f);
                sr.anchoredPosition = new Vector2(-560f + i * 190f, -300f + (i == 1 ? 24f : 0f));
                reveal.stars[i] = Img(sr, starOff, Color.white);
            }
            c.stars = reveal;

            // Bantay cheers inside the panel, with the Display Name in the bubble.
            c.cheerBantay = BuildBantay(box);
            var dog = (RectTransform)c.cheerBantay.transform;
            dog.anchorMin = dog.anchorMax = new Vector2(1f, 1f);
            dog.pivot = new Vector2(1f, 0f);
            dog.anchoredPosition = new Vector2(-150f, -480f);

            // The Safe Choice and its explanation
            var holder = Rect("Safe Picture", box);
            holder.anchorMin = holder.anchorMax = new Vector2(0f, 1f);
            holder.pivot = new Vector2(0f, 1f);
            holder.sizeDelta = new Vector2(320f, 232f);
            holder.anchoredPosition = new Vector2(70f, -476f);
            Img(holder, S("UI/input_box"), Color.white, sliced: true);
            var pic = Rect("Art", holder);
            Centered(pic, 0f, 0f, 72f * Px, 52f * Px);
            c.safePicture = Img(pic, S("Level/choice_desk"), Color.white);

            var choiceR = Rect("Safe Choice", box);
            choiceR.anchorMin = choiceR.anchorMax = new Vector2(0f, 1f);
            choiceR.pivot = new Vector2(0f, 1f);
            choiceR.sizeDelta = new Vector2(1080f, 64f);
            choiceR.anchoredPosition = new Vector2(420f, -486f);
            c.safeChoiceText = Label(choiceR, string.Empty, f4, new Color32(0x2a, 0x70, 0x34, 0xff), TextAnchor.MiddleLeft, shadow: false);

            var lessonR = Rect("Lesson", box);
            lessonR.anchorMin = lessonR.anchorMax = new Vector2(0f, 1f);
            lessonR.pivot = new Vector2(0f, 1f);
            lessonR.sizeDelta = new Vector2(1080f, 150f);
            lessonR.anchoredPosition = new Vector2(420f, -556f);
            c.lessonText = Label(lessonR, string.Empty, f4, Ink, TextAnchor.UpperLeft, shadow: false);
            c.lessonText.horizontalOverflow = HorizontalWrapMode.Wrap;
            c.lessonText.lineSpacing = 1.15f;

            // Buttons in a row; a hidden Next leaves no gap.
            var row = Rect("Buttons", box);
            row.anchorMin = row.anchorMax = new Vector2(0.5f, 0f);
            row.pivot = new Vector2(0.5f, 0f);
            row.sizeDelta = new Vector2(1480f, 124f);
            row.anchoredPosition = new Vector2(0f, 28f);
            var h = row.gameObject.AddComponent<HorizontalLayoutGroup>();
            h.spacing = 28f;
            h.childAlignment = TextAnchor.MiddleCenter;
            h.childControlWidth = h.childControlHeight = false;
            h.childForceExpandWidth = h.childForceExpandHeight = false;

            c.replayButton = RowButton(row, "Replay", "blue", UIStrings.Replay, "icon_replay", Sfx.Click);
            c.nextButton = RowButton(row, "Next", "green", UIStrings.NextLevel, "icon_play", Sfx.Confirm);
            c.levelsButton = RowButton(row, "Levels", "orange", UIStrings.LevelsButton, "icon_levels", Sfx.Back);
            c.menuButton = RowButton(row, "Menu", "teal", UIStrings.MainMenuButton, "icon_home", Sfx.Back);
            c.nextButton.gameObject.SetActive(false);
        }

        static Button RowButton(RectTransform row, string name, string color, string label, string icon, Sfx sfx)
        {
            var b = MakeButton(row, name, color, label, f6, S("Icons/" + icon), sfx);
            ((RectTransform)b.transform).sizeDelta = new Vector2(330f, 124f);
            return b;
        }

        static void BuildPause(RectTransform ui, LevelController c)
        {
            var box = Dialog(ui, "Pause", 900f, 720f, out c.pausePanel);
            DialogTitle(box, UIStrings.PauseTitle, f8);
            var br = Rect("Body", box);
            Centered(br, 0f, 170f, 800f, 60f);
            Label(br, UIStrings.PauseBody, f4, Ink, TextAnchor.MiddleCenter, shadow: false);

            c.resumeButton = PauseButton(box, "Resume", "green", UIStrings.Resume, "icon_play", Sfx.Click, 40f);
            c.restartButton = PauseButton(box, "Restart", "orange", UIStrings.Restart, "icon_replay", Sfx.Click, -100f);
            c.quitButton = PauseButton(box, "Quit", "teal", UIStrings.QuitToMenu, "icon_home", Sfx.Back, -240f);
        }

        static Button PauseButton(RectTransform box, string name, string color, string label, string icon, Sfx sfx, float y)
        {
            var b = MakeButton(box, name, color, label, f6, S("Icons/" + icon), sfx);
            Centered((RectTransform)b.transform, 0f, y, 640f, 120f);
            return b;
        }
    }
}
