using System.Linq;
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
            "choice_low_shelf", "choice_picture_bed", "choice_bed_window", "choice_books_top",
            "choice_go_bag", "choice_pack_later", "choice_bag_hidden", "choice_bag_toys",
            "choice_table", "choice_run_home", "choice_glass_cabinet", "choice_sofa",
            "choice_open_field", "choice_tree", "choice_post", "choice_building",
            "choice_high_ground", "choice_fish", "choice_watch_waves", "choice_wait_beach",
            "choice_meeting_place", "choice_own_way", "choice_meet_home", "choice_phone_numbers",
            "choice_route_field", "choice_route_post", "choice_route_tree", "choice_route_later",
            "choice_food_court_table", "choice_run_exit", "choice_elevator_during", "choice_shop_window",
            "choice_mall_stairs", "choice_elevator_after", "choice_run_push", "choice_mall_wall",
            "choice_call_adult", "choice_pull_up", "choice_leave_hurt", "choice_fix_pipe",
            "choice_go_bag_out", "choice_back_for_toys", "choice_no_go_bag", "choice_no_note",
            "choice_radio", "choice_go_home", "choice_rumor", "choice_phone_all_day",
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

            c.settings = new[]
            {
                BuildClassroom(world), BuildCorridor(world), BuildClassroomAfter(world),
                BuildBedroom(world), BuildHome(world), BuildSchoolyard(world), BuildBeach(world),
                BuildMallA(world), BuildMallB(world), BuildHomeAfter(world), BuildEvacuationArea(world),
                BuildClassroomV2(world), BuildCorridorV2(world), BuildClassroomAfterV2(world),
                BuildBedroomV2(world), BuildHomeV2(world),
            };
            for (var i = 1; i < c.settings.Length; i++)
                c.settings[i].gameObject.SetActive(false);

            var ui = Stretch(Rect("SafeArea", root));
            ui.gameObject.AddComponent<SafeAreaFitter>();

            BuildLevelTopBar(ui, c);
            BuildChoices(ui, c);
            BuildCaption(ui, c);
            BuildActionBar(ui, c);
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
                BuildLevelAnimation(shaking, "Ceiling Fan", Frames("Level/fan", 2), 204, 0, 6f, onlyWhileShaking: true),
                BuildLevelAnimation(shaking, "Tall Cabinet", Frames("Level/cabinet", 2), 364, 64, 5f, onlyWhileShaking: true),
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
                BuildLevelAnimation(shaking, "Hanging Pot Left", pots, 116, 12, 4f, onlyWhileShaking: true),
                BuildLevelAnimation(shaking, "Hanging Pot Right", pots, 236, 12, 4f, onlyWhileShaking: true),
            };
            return setting;
        }

        static LevelSetting BuildClassroomAfter(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.ClassroomAfter, "Classroom After", out var foreground);
            // Keep Bantay and the Hint clear of the teacher in the doorway.
            setting.guidanceOffsetX = -880f;
            BuildLevelAnimation(foreground, "Teacher", Frames("Level/teacher", 2), 418, 126, 3f, onlyWhileShaking: false);
            return setting;
        }

        static LevelSetting BuildBedroom(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.Bedroom, "Bedroom", out _);
            // Stand Bantay and the Hint between the bed and the low shelf, clear of the bookcase.
            setting.guidanceOffsetX = -760f;
            setting.compactChoicesOnWideScreens = true;
            return setting;
        }

        static LevelSetting BuildHome(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.Home, "Home", out var shaking);
            // Keep the Goal off the hanging lamp, and Bantay and the Hint in front of the sofa,
            // clear of the table, the glass cabinet and the door.
            setting.goalPosition = new Vector2(-490f, -120f);
            setting.guidanceOffsetX = -1230f;
            setting.compactChoicesOnWideScreens = true;
            setting.shaker.wobblers = new[]
            {
                BuildLevelAnimation(shaking, "Hanging Lamp", Frames("Level/lamp", 2), 232, 0, 4f, onlyWhileShaking: true),
                BuildLevelAnimation(shaking, "Glass Cabinet", Frames("Level/glass_cabinet", 2), 330, 64, 5f, onlyWhileShaking: true),
            };
            return setting;
        }

        static LevelSetting BuildSchoolyard(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.Schoolyard, "Schoolyard", out var shaking);
            // Keep the tree clear of the Goal, the post clear of Pause, and the field clear of Bantay.
            // The tree and post fit above the Choices at three UI units per art pixel.
            setting.alignArtTop = true;
            setting.guidanceOffsetX = -1212f;
            setting.shaker.wobblers = new[]
            {
                BuildLevelAnimation(shaking, "Big Tree", Frames("Level/tree", 2), 334, 34, 3f, onlyWhileShaking: true, artScale: 0.75f),
                BuildLevelAnimation(shaking, "Electric Post", Frames("Level/post", 2), 390, 14, 4f, onlyWhileShaking: true, artScale: 0.75f),
            };
            return setting;
        }

        static LevelSetting BuildBeach(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.Beach, "Beach", out var foreground);
            // Nudge the Hint right of the last fish on the sand; the hill path stays clear.
            setting.guidanceOffsetX = 56f;
            setting.compactChoicesOnWideScreens = true;
            BuildLevelAnimation(foreground, "Waves", Frames("Level/waves", 2), 140, 128, 2.5f, onlyWhileShaking: false);
            return setting;
        }

        static LevelSetting BuildMallA(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.MallA, "Mall Food Court", out var shaking);
            // Stand Bantay and the Hint in front of the food stalls, clear of the tables and the exit.
            setting.guidanceOffsetX = -120f;
            setting.compactChoicesOnWideScreens = true;
            setting.shaker.wobblers = new[]
            {
                BuildLevelAnimation(shaking, "Hanging Sign", Frames("Level/mall_sign", 2), 380, 0, 4f, onlyWhileShaking: true),
                BuildLevelAnimation(shaking, "Glass Display", Frames("Level/shop_display", 2), 66, 112, 5f, onlyWhileShaking: true),
            };
            return setting;
        }

        static LevelSetting BuildMallB(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.MallB, "Mall Outside", out _);
            // Stand Bantay and the Hint on the pavement left of the stair, clear of the stair,
            // the parking area and the waiting family; the bubble may hide the top of the doors.
            setting.guidanceOffsetX = -960f;
            setting.compactChoicesOnWideScreens = true;
            return setting;
        }

        static LevelSetting BuildHomeAfter(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.HomeAfter, "Home After", out _);
            // Bantay and the Hint stand in front of the sofa, as in the home. The Goal keeps the top
            // centre: the still lamp may sit behind it, and two-line Goals stay clear of Bantay's bubble.
            setting.guidanceOffsetX = -1230f;
            setting.compactChoicesOnWideScreens = true;
            return setting;
        }

        static LevelSetting BuildEvacuationArea(RectTransform world)
        {
            var setting = BuildSetting(world, ScenarioSettings.EvacuationArea, "Evacuation Area", out _);
            // Stand Bantay and the Hint by the tent, clear of the families, the radio and the go bag.
            setting.guidanceOffsetX = -40f;
            setting.compactChoicesOnWideScreens = true;
            return setting;
        }

        // A still copy behind the foreground keeps the edges covered during shaking.
        static LevelSetting BuildSetting(RectTransform world, string key, string name, out RectTransform foreground)
        {
            var group = Stretch(Rect(name, world));
            var setting = group.gameObject.AddComponent<LevelSetting>();
            setting.key = key;
            var shakes = ScenarioSettings.CanShake(key);

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

        static SpriteAnimator BuildLevelAnimation(RectTransform parent, string name, Sprite[] frames, int x, int y, float fps, bool onlyWhileShaking, float artScale = 1f)
        {
            var image = ArtImage(parent, name, frames[0], x, y, artScale);
            var animator = image.gameObject.AddComponent<SpriteAnimator>();
            animator.frames = frames;
            animator.framesPerSecond = fps;
            animator.randomStart = onlyWhileShaking;
            animator.enabled = !onlyWhileShaking;
            return animator;
        }

        // The Student flow's classroom: the v2 art, the Student, and the desk front drawn over a ducking Student.
        // Placements and spots are in ArtSource/README.md (v2 classroom and the Student).
        static LevelSetting BuildClassroomV2(RectTransform world)
        {
            var group = Stretch(Rect("Classroom V2", world));
            var setting = group.gameObject.AddComponent<LevelSetting>();
            setting.key = ScenarioSettings.ActionArtKey(ScenarioSettings.Classroom);
            // On wide screens keep the floor, the Student and the Action Bar apart: crop the ceiling, not the floor.
            setting.artAnchorY = 0f;

            var background = S("Level/v2/bg_classroom");
            ArtImage(group, "Backing", background, 0, 0);
            var shaking = Stretch(Rect("Shaking", group));
            ArtImage(shaking, "Classroom V2", background, 0, 0);
            setting.shaker = shaking.gameObject.AddComponent<SceneShaker>();
            setting.shaker.target = shaking;
            setting.shaker.shakeSeconds = 2.5f;
            setting.shaker.wobblers = new[]
            {
                BuildLevelAnimation(shaking, "Ceiling Fan", Frames("Level/v2/fan", 2), 204, 0, 6f, onlyWhileShaking: true),
                BuildLevelAnimation(shaking, "Tall Cabinet", Frames("Level/v2/cabinet", 2), 424, 100, 5f, onlyWhileShaking: true),
            };

            setting.student = BuildStudent(shaking, withBubble: true);
            var deskFront = ArtImage(shaking, "Desk Front", S("Level/v2/desk_front"), 198, 188);
            deskFront.gameObject.SetActive(false);

            setting.startX = 126f;
            setting.actOuts = new[]
            {
                // Unsafe Choices stop clear of the desk and its chair (x 198 to 302), the door and the window:
                // short of the door (46), the window (372) and the cabinet (440).
                new ActOut { key = "door", stopAfter = 0, steps = new[] { ActOutStep.Walk(94f) } },
                // The Safe Choice walks up beside the desk (184), crouches and shuffles in, then holds the
                // left front leg (x 200-206) with its reaching hand at 222, and the shaking calms.
                new ActOut
                {
                    key = "desk",
                    steps = new[]
                    {
                        ActOutStep.Walk(184f),
                        ActOutStep.Overlay(deskFront.gameObject),
                        ActOutStep.Crouch(222f),
                        ActOutStep.Pose(StudentPose.Hold),
                        ActOutStep.Calm(),
                    },
                },
                new ActOut { key = "window", stopAfter = 0, steps = new[] { ActOutStep.Walk(322.8f) } },
                new ActOut { key = "cabinet", stopAfter = 0, steps = new[] { ActOutStep.Walk(361.5f) } },
            };
            return setting;
        }

        // Easy 2's corridor in the v2 art: hanging pots (left), the hall window, the start spot by the
        // Grade 4 door, the plain inner wall (x 240-331) and the stairs going down (top step at x 404).
        // Placements and Act-out points are in ArtSource/README-rollout.md (corridor).
        static LevelSetting BuildCorridorV2(RectTransform world)
        {
            var group = Stretch(Rect("Corridor V2", world));
            var setting = group.gameObject.AddComponent<LevelSetting>();
            setting.key = ScenarioSettings.ActionArtKey(ScenarioSettings.Corridor);
            // As in the classroom: on wide screens crop the ceiling, not the floor.
            setting.artAnchorY = 0f;

            var background = S("Level/v2/bg_corridor");
            ArtImage(group, "Backing", background, 0, 0);
            var shaking = Stretch(Rect("Shaking", group));
            ArtImage(shaking, "Corridor V2", background, 0, 0);
            setting.shaker = shaking.gameObject.AddComponent<SceneShaker>();
            setting.shaker.target = shaking;
            setting.shaker.shakeSeconds = 2.5f;
            setting.shaker.wobblers = new[]
            {
                BuildLevelAnimation(shaking, "Hanging Pots", Frames("Level/v2/corridor_pots", 2), 6, 15, 4f, onlyWhileShaking: true),
            };

            setting.student = BuildStudent(shaking, withBubble: true);
            setting.startX = 214f;
            setting.actOuts = new[]
            {
                // Run-start toward the stairs and stop at the top step (the front of the first tread is at x 404).
                new ActOut { key = "stairs", stopAfter = 0, steps = new[] { ActOutStep.Run(394f) } },
                // Walk toward the hall window (its sill ends at x 180) and stop by it.
                new ActOut { key = "window", stopAfter = 0, steps = new[] { ActOutStep.Walk(188f) } },
                // The Safe Choice: walk to the inner wall, away from the glass and the pots, crouch low,
                // then kneel and cover the head and neck with both arms, and hold until the shaking calms.
                new ActOut
                {
                    key = "cover",
                    steps = new[]
                    {
                        ActOutStep.Walk(270f),
                        ActOutStep.Crouch(286f),
                        ActOutStep.Pose(StudentPose.Cover, 1.5f),
                        ActOutStep.Calm(),
                    },
                },
                // Walk toward the spot under the pots (x 38) and stop just before it (the right pot ends at x 63).
                new ActOut { key = "plants", stopAfter = 0, steps = new[] { ActOutStep.Walk(82f) } },
            };
            return setting;
        }

        static LevelSetting BuildClassroomAfterV2(RectTransform world)
        {
            var group = Stretch(Rect("Classroom After V2", world));
            var setting = group.gameObject.AddComponent<LevelSetting>();
            setting.key = ScenarioSettings.ActionArtKey(ScenarioSettings.ClassroomAfter);
            setting.artAnchorY = 0f;
            ArtImage(group, "Classroom After V2", S("Level/v2/bg_classroom_after"), 0, 0);

            var teacher = BuildTeacher(group, 77f);
            setting.grownUps = new[] { teacher };
            setting.student = BuildStudent(group, withBubble: true);
            // Keep the Student's Reaction clear of the Teacher while they stand near the doorway.
            var reaction = (RectTransform)setting.student.bubble.transform;
            reaction.anchoredPosition += new Vector2(44f * Px, 0f);
            ArtImage(group, "Desk Front", S("Level/v2/classroom_after_desk_front"), 198, 188);

            // Continue the Hold from Easy 1. Walk back to the same desk edge before crouching on Try Again.
            const float deskEdge = 182f;
            setting.startX = 222f;
            setting.startPose = StudentPose.Hold;
            setting.startCrouchFromX = deskEdge;
            setting.actOuts = new[]
            {
                new ActOut
                {
                    key = "stay",
                    stopAfter = 1,
                    stopPose = StudentPose.Hold,
                    steps = new[]
                    {
                        ActOutStep.Pose(StudentPose.Hold, 0.45f),
                        ActOutStep.Crouch(220f, lookSeconds: 0.3f),
                    },
                },
                new ActOut
                {
                    key = "walk",
                    steps = new[]
                    {
                        ActOutStep.Crouch(deskEdge),
                        ActOutStep.Pose(StudentPose.Idle, 0.16f),
                        ActOutStep.Walk(113f),
                        ActOutStep.GrownUpTurn(teacher, left: true),
                        ActOutStep.WalkWith(teacher, 51f, 21f, new Vector2(36f, 35f)),
                        ActOutStep.Pose(StudentPose.Happy),
                    },
                },
                new ActOut
                {
                    key = "push",
                    stopAfter = 2,
                    steps = new[]
                    {
                        ActOutStep.Crouch(deskEdge),
                        ActOutStep.Pose(StudentPose.Idle, 0.16f),
                        ActOutStep.Run(110f),
                    },
                },
            };
            return setting;
        }

        // A prop the Act-outs move, drawn at its art place (top-left, art pixels). holdOffset is where
        // its bottom-left corner sits in the Student's body while held, in UI units.
        static LevelProp BuildProp(RectTransform parent, string key, string sprite, int artX, int artY,
            Vector2 holdOffset, bool visible = true)
        {
            var r = ArtImage(parent, key, S(sprite), artX, artY);
            var prop = r.gameObject.AddComponent<LevelProp>();
            prop.key = key;
            prop.image = r.GetComponent<Image>();
            prop.holdOffset = holdOffset;
            r.gameObject.SetActive(visible);
            return prop;
        }

        // UI units (bottom-left of a prop in the Setting) for a top-left art place of a sprite h art pixels tall.
        static Vector2 PropPlace(int artX, int artY, int artHeight) =>
            new Vector2(artX * Px, 1080f - artY * Px - artHeight * Px);

        // Normal 1's bedroom in the v2 art (phase before, nothing shakes). Left to right: the picture
        // leaning under the window, the bed (slides toward the window), the tall cabinet, the start spot
        // with the books beside it, the heavy box and the low shelf. Placements and Act-out points are in
        // ArtSource/README-rollout.md (Bedroom) and ArtSource/scripts/bedroom_v2.lua (M.LAYOUT).
        static LevelSetting BuildBedroomV2(RectTransform world)
        {
            var group = Stretch(Rect("Bedroom V2", world));
            var setting = group.gameObject.AddComponent<LevelSetting>();
            setting.key = ScenarioSettings.ActionArtKey(ScenarioSettings.Bedroom);
            setting.artAnchorY = 0f;
            ArtImage(group, "Bedroom V2", S("Level/v2/bg_bedroom"), 0, 0);

            // Held props sit centred on the carry pose's hands, about (23, 30) in the 32x50 frame.
            var picture = BuildProp(group, "picture", "Level/v2/bedroom_picture", 10, 183, new Vector2(20f, 22f));
            var bed = BuildProp(group, "bed", "Level/v2/bedroom_bed", 104, 194, Vector2.zero);
            var books = BuildProp(group, "books", "Level/v2/bedroom_books", 322, 201, new Vector2(60f, 56f));
            var box = BuildProp(group, "box", "Level/v2/bedroom_box", 350, 196, new Vector2(44f, 46f));
            var placed = BuildProp(group, "box_placed", "Level/v2/bedroom_box_placed", 397, 193, Vector2.zero, visible: false);
            setting.props = new[] { picture, bed, books, box, placed };

            setting.student = BuildStudent(group, withBubble: true);
            setting.startX = 306f;
            setting.startPose = StudentPose.Idle;

            // The reach-up hands are about (28, 3) in the frame: art x 152 at x 140 facing right, and
            // art x 259 at x 272 facing left, both at art y 183. The prop is held up just above them.
            setting.actOuts = new[]
            {
                // Pick up the picture under the window, carry it to the bed and reach up toward the wall over it.
                new ActOut
                {
                    key = "picture",
                    stopAfter = 7,
                    stopPose = StudentPose.ReachUp,
                    steps = new[]
                    {
                        ActOutStep.Walk(34f),
                        ActOutStep.Face(left: true),
                        ActOutStep.Pose(StudentPose.Reach, 0.3f),
                        ActOutStep.PropPickUp(picture.gameObject),
                        ActOutStep.Carry(140f),
                        ActOutStep.Pose(StudentPose.ReachUp),
                        ActOutStep.PropPlace(picture.gameObject, PropPlace(134, 155, 29)),
                        ActOutStep.Wait(0.4f),
                    },
                },
                // The Safe Choice: walk to the box, crouch and lift it, carry it to the low shelf, crouch and
                // set it down; the box shows in the shelf's lower left compartment.
                new ActOut
                {
                    key = "low_shelf",
                    steps = new[]
                    {
                        ActOutStep.Walk(343f),
                        ActOutStep.Pose(StudentPose.Crouch, 0.35f),
                        ActOutStep.PropPickUp(box.gameObject),
                        ActOutStep.Carry(384f),
                        ActOutStep.PropHide(box.gameObject),
                        ActOutStep.PropShow(placed.gameObject),
                        ActOutStep.Pose(StudentPose.Crouch, 0.35f),
                        ActOutStep.Pose(StudentPose.Idle),
                        ActOutStep.Calm(),
                    },
                },
                // Walk to the bed's headboard (x 215), push, and the bed slides 8 pixels toward the window
                // with the Student stepping after it, palms on the headboard (the stop is at x 221).
                new ActOut
                {
                    key = "bed_window",
                    stopAfter = 3,
                    stopPose = StudentPose.Push,
                    steps = new[]
                    {
                        ActOutStep.Walk(229f),
                        ActOutStep.Face(left: true),
                        ActOutStep.Pose(StudentPose.Push, 0.3f),
                        ActOutStep.Push(bed.gameObject, -8f, 0.8f),
                    },
                },
                // Pick up the books beside the start, carry them to the tall cabinet and reach up on tiptoe.
                new ActOut
                {
                    key = "books_top",
                    stopAfter = 6,
                    stopPose = StudentPose.ReachUp,
                    steps = new[]
                    {
                        ActOutStep.Walk(316f),
                        ActOutStep.Pose(StudentPose.Crouch, 0.35f),
                        ActOutStep.PropPickUp(books.gameObject),
                        ActOutStep.Carry(272f),
                        ActOutStep.Pose(StudentPose.ReachUp),
                        ActOutStep.PropPlace(books.gameObject, PropPlace(251, 172, 12)),
                        ActOutStep.Wait(0.4f),
                    },
                },
            };
            return setting;
        }

        static LevelSetting BuildHomeV2(RectTransform world)
        {
            var group = Stretch(Rect("Home V2", world));
            var setting = group.gameObject.AddComponent<LevelSetting>();
            setting.key = ScenarioSettings.ActionArtKey(ScenarioSettings.Home);
            setting.artAnchorY = 0f;

            var background = S("Level/v2/bg_home");
            ArtImage(group, "Backing", background, 0, 0);
            var shaking = Stretch(Rect("Shaking", group));
            ArtImage(shaking, "Living Room", background, 0, 0);
            setting.shaker = shaking.gameObject.AddComponent<SceneShaker>();
            setting.shaker.target = shaking;
            setting.shaker.shakeSeconds = 2.5f;
            setting.shaker.wobblers = new[]
            {
                BuildLevelAnimation(shaking, "Hanging Lamp", Frames("Level/v2/home_lamp", 2),
                    384, 14, 6.25f, onlyWhileShaking: true),
            };

            var packing = Stretch(Rect("Normal 2 Cast", shaking));
            var mom = BuildMom(packing, 254f);
            var openBag = BuildHomeProp(packing, "bag_open", 302, 191, 22, 23);
            var closedBag = BuildHomeProp(packing, "bag_closed", 303, 191, 20, 23, visible: false);
            var toy = BuildHomeProp(packing, "toy", 327, 203, 15, 11);
            var water = BuildHomeProp(packing, "water", 345, 195, 8, 19);
            var can = BuildHomeProp(packing, "can", 357, 202, 9, 12);
            var flashlight = BuildHomeProp(packing, "flashlight", 370, 206, 12, 8);
            var whistle = BuildHomeProp(packing, "whistle", 386, 207, 11, 7);
            var snack = BuildHomeProp(packing, "snack", 401, 201, 9, 13);

            var meeting = Stretch(Rect("Hard 1 Cast", shaking));
            var meetingMom = BuildMom(meeting, 266f);
            var park = BuildHomeProp(meeting, "park_card", 213, 176, 16, 13);
            var house = BuildHomeProp(meeting, "house_card", 232, 176, 16, 13);

            setting.student = BuildStudent(shaking, withBubble: true);
            setting.student.boy.reachUp = Frames("Student/boy_home_reachup", 2);
            setting.student.girl.reachUp = Frames("Student/girl_home_reachup", 2);
            var tableFront = ArtImage(shaking, "Table Front", S("Level/v2/table_front"), 174, 188).gameObject;
            tableFront.SetActive(false);

            // Leave the board and boxes visible without raising the table Reaction into Hint.
            var raisedReaction = new Vector2(0f, 132f);
            setting.levelStarts = new[]
            {
                new LevelStart
                {
                    level = "Earthquake/Normal/2", x = 281f, cast = packing.gameObject,
                    reactionPosition = raisedReaction,
                    props = new[] { openBag, closedBag, toy, water, can, flashlight, whistle, snack },
                    grownUps = new[] { mom },
                },
                new LevelStart { level = "Earthquake/Normal/3", x = 294f },
                new LevelStart
                {
                    level = "Earthquake/Hard/1", x = 154f, cast = meeting.gameObject,
                    reactionPosition = raisedReaction,
                    props = new[] { park, house }, grownUps = new[] { meetingMom },
                },
            };
            setting.actOuts = new[]
            {
                new ActOut
                {
                    key = "later", stopAfter = 0,
                    steps = new[] { ActOutStep.Walk(130f) },
                },
                new ActOut
                {
                    key = "hidden", stopAfter = 5, stopPose = StudentPose.Crouch,
                    steps = new[]
                    {
                        ActOutStep.Walk(308f),
                        ActOutStep.PropPickUp(openBag.gameObject),
                        ActOutStep.Carry(425f),
                        ActOutStep.Pose(StudentPose.Crouch),
                        ActOutStep.PropPlace(openBag.gameObject, PropPlace(437, 191, 23)),
                        ActOutStep.Wait(0.36f),
                    },
                },
                new ActOut
                {
                    key = "go_bag",
                    steps = new[]
                    {
                        ActOutStep.Walk(308f),
                        ActOutStep.Pose(StudentPose.Pack, 0.28f),
                        ActOutStep.PropHide(water.gameObject),
                        ActOutStep.Pose(StudentPose.Pack, 0.28f),
                        ActOutStep.PropHide(can.gameObject),
                        ActOutStep.Pose(StudentPose.Pack, 0.28f),
                        ActOutStep.PropHide(flashlight.gameObject),
                        ActOutStep.Pose(StudentPose.Pack, 0.28f),
                        ActOutStep.PropHide(whistle.gameObject),
                        ActOutStep.PropHide(openBag.gameObject),
                        ActOutStep.PropShow(closedBag.gameObject),
                        ActOutStep.Wait(0.22f),
                        ActOutStep.PropPickUp(closedBag.gameObject),
                        ActOutStep.Carry(108f),
                        ActOutStep.Pose(StudentPose.Crouch),
                        ActOutStep.PropPlace(closedBag.gameObject, PropPlace(73, 191, 23)),
                        ActOutStep.Wait(0.22f),
                        ActOutStep.Pose(StudentPose.Idle),
                        ActOutStep.GrownUpPoseStep(mom, GrownUpPose.Happy),
                    },
                },
                new ActOut
                {
                    key = "toys", stopAfter = 5, stopPose = StudentPose.Pack,
                    steps = new[]
                    {
                        ActOutStep.Walk(324f),
                        ActOutStep.Pose(StudentPose.Crouch, 0.22f),
                        ActOutStep.PropPickUp(toy.gameObject),
                        ActOutStep.Carry(308f),
                        ActOutStep.Pose(StudentPose.Pack),
                        ActOutStep.Wait(0.36f),
                    },
                },

                new ActOut
                {
                    key = "run", stopAfter = 0,
                    steps = new[] { ActOutStep.Run(91f) },
                },
                new ActOut
                {
                    key = "glass", stopAfter = 0,
                    steps = new[] { ActOutStep.Walk(387f) },
                },
                new ActOut
                {
                    key = "sofa", stopAfter = 3, stopPose = StudentPose.Crouch,
                    steps = new[]
                    {
                        ActOutStep.Walk(307f),
                        ActOutStep.Face(left: false),
                        ActOutStep.Pose(StudentPose.Crouch),
                        ActOutStep.Wait(0.30f),
                    },
                },
                new ActOut
                {
                    key = "table",
                    steps = new[]
                    {
                        ActOutStep.Walk(282f),
                        ActOutStep.Overlay(tableFront),
                        ActOutStep.Crouch(240f),
                        ActOutStep.Face(left: true),
                        ActOutStep.Pose(StudentPose.Hold, 0.65f),
                        ActOutStep.Calm(),
                    },
                },

                new ActOut
                {
                    key = "own_way", stopAfter = 0,
                    steps = new[] { ActOutStep.Walk(91f) },
                },
                new ActOut
                {
                    key = "meeting_place",
                    steps = new[]
                    {
                        ActOutStep.Walk(207f),
                        ActOutStep.Pose(StudentPose.Reach, 0.22f),
                        ActOutStep.PropPickUp(park.gameObject),
                        ActOutStep.Carry(136f),
                        ActOutStep.Pose(StudentPose.ReachUp),
                        ActOutStep.PropPlace(park.gameObject, PropPlace(109, 172, 13)),
                        ActOutStep.Wait(0.4f),
                        ActOutStep.GrownUpPoseStep(meetingMom, GrownUpPose.Happy),
                    },
                },
                new ActOut
                {
                    key = "meet_home", stopAfter = 6, stopPose = StudentPose.ReachUp,
                    steps = new[]
                    {
                        ActOutStep.Walk(226f),
                        ActOutStep.Pose(StudentPose.Reach, 0.22f),
                        ActOutStep.PropPickUp(house.gameObject),
                        ActOutStep.Carry(136f),
                        ActOutStep.Pose(StudentPose.ReachUp),
                        ActOutStep.PropPlace(house.gameObject, PropPlace(109, 172, 13)),
                        ActOutStep.Wait(0.4f),
                    },
                },
                new ActOut
                {
                    key = "phone_numbers", stopAfter = 1, stopPose = StudentPose.Phone,
                    steps = new[] { ActOutStep.Pose(StudentPose.Phone), ActOutStep.Wait(0.45f) },
                },
            };
            return setting;
        }

        // Centres a held prop on the Carry pose's hands at (23, 30) in the 32x50 frame.
        static LevelProp BuildHomeProp(RectTransform parent, string key, int x, int y, int width, int height,
            bool visible = true)
        {
            var grip = new Vector2(23f - width / 2f, 20f - height / 2f) * Px;
            return BuildProp(parent, key, "Level/v2/home_" + key, x, y, grip, visible);
        }

        static GrownUpActor BuildMom(RectTransform parent, float startX)
        {
            var r = Rect("Mom", parent);
            r.anchorMin = r.anchorMax = Vector2.zero;
            r.pivot = new Vector2(0.5f, 0f);
            r.sizeDelta = new Vector2(36f * Px, 62f * Px);

            var mom = r.gameObject.AddComponent<GrownUpActor>();
            mom.kind = GrownUpKind.Mom;
            mom.unitsPerPixel = Px;
            mom.feetY = 1080f - 230f * Px;
            mom.startX = startX;
            mom.frames = new GrownUpFrames
            {
                idle = Frames("GrownUp/mom_idle", 2),
                happy = Frames("GrownUp/mom_happy", 2),
                walk = Frames("GrownUp/mom_walk", 4),
                sit = Frames("GrownUp/mom_sit", 2),
                hurtSit = Frames("GrownUp/mom_hurtsit", 2),
            };
            mom.body = Img(Stretch(Rect("Body", r)), mom.frames.idle[0], Color.white);
            mom.PlaceAtStart();
            return mom;
        }

        static GrownUpActor BuildTeacher(RectTransform parent, float startX)
        {
            var r = Rect("Teacher", parent);
            r.anchorMin = r.anchorMax = Vector2.zero;
            r.pivot = new Vector2(0.5f, 0f);
            r.sizeDelta = new Vector2(36f * Px, 62f * Px);

            var teacher = r.gameObject.AddComponent<GrownUpActor>();
            teacher.kind = GrownUpKind.Teacher;
            teacher.unitsPerPixel = Px;
            teacher.feetY = 1080f - 230f * Px;
            teacher.startX = startX;
            teacher.startPose = GrownUpPose.Wave;
            teacher.frames = new GrownUpFrames
            {
                idle = Frames("GrownUp/teacher_idle", 2),
                wave = Frames("GrownUp/teacher_wave", 2),
                walk = Frames("GrownUp/teacher_walk", 4),
            };
            teacher.body = Img(Stretch(Rect("Body", r)), teacher.frames.wave[0], Color.white);
            teacher.PlaceAtStart();
            return teacher;
        }

        static StudentFrames StudentSet(string kind) => new StudentFrames
        {
            idle = Frames("Student/" + kind + "_idle", 2),
            surprised = Frames("Student/" + kind + "_surprised", 2),
            walk = Frames("Student/" + kind + "_walk", 4),
            duck = Frames("Student/" + kind + "_duck", 2),
            wait = Frames("Student/" + kind + "_wait", 2),
            happy = Frames("Student/" + kind + "_happy", 2),
            crouch = Frames("Student/" + kind + "_crouch", 2),
            hold = Frames("Student/" + kind + "_hold", 2),
            carry = Frames("Student/" + kind + "_carry", 4),
            reach = Frames("Student/" + kind + "_reach", 2),
            reachUp = Frames("Student/" + kind + "_reachup", 2),
            push = Frames("Student/" + kind + "_push", 2),
            call = Frames("Student/" + kind + "_call", 2),
            cover = Frames("Student/" + kind + "_cover", 2),
            pack = Frames("Student/" + kind + "_pack", 2),
            phone = Frames("Student/" + kind + "_phone", 2),
            sit = Frames("Student/" + kind + "_sit", 2),
            portrait = S("Student/" + kind + "_portrait"),
        };

        // The frame's bottom-left corner is the origin, so the feet stand on the floor line at y = 230 art pixels.
        static StudentActor BuildStudent(RectTransform parent, bool withBubble)
        {
            var r = Rect("Student", parent);
            r.anchorMin = r.anchorMax = Vector2.zero;
            r.pivot = new Vector2(0.5f, 0f);
            r.sizeDelta = new Vector2(32f * Px, 50f * Px);
            r.anchoredPosition = new Vector2(126f * Px, 1080f - 230f * Px);

            var actor = r.gameObject.AddComponent<StudentActor>();
            actor.boy = StudentSet("boy");
            actor.girl = StudentSet("girl");
            actor.feetY = 1080f - 230f * Px;
            actor.unitsPerPixel = Px;
            var body = Stretch(Rect("Body", r));
            actor.body = Img(body, actor.boy.idle[0], Color.white);

            if (withBubble)
            {
                var bubble = BuildSpeechBubble(r, 340f, 104f);
                var br = (RectTransform)bubble.transform;
                br.anchorMin = br.anchorMax = new Vector2(0.5f, 1f);
                br.pivot = new Vector2(0.5f, 0f);
                br.anchoredPosition = new Vector2(0f, 8f);
                bubble.Hide();
                actor.bubble = bubble;
            }
            return actor;
        }

        static SpeechBubble BuildSpeechBubble(RectTransform parent, float w, float h)
        {
            var r = Rect("Speech Bubble", parent);
            r.sizeDelta = new Vector2(w, h);
            Img(r, S("UI/speech_bubble"), Color.white, sliced: true);
            var textR = Stretch(Rect("Text", r));
            textR.offsetMin = new Vector2(24f, 24f);
            textR.offsetMax = new Vector2(-24f, -12f);
            var label = Label(textR, string.Empty, f4, Ink, TextAnchor.MiddleCenter, shadow: false);
            label.horizontalOverflow = HorizontalWrapMode.Wrap;
            var bubble = r.gameObject.AddComponent<SpeechBubble>();
            bubble.text = label;
            return bubble;
        }

        static void BuildCaption(RectTransform ui, LevelController c)
        {
            var cap = Rect("Caption", ui);
            cap.anchorMin = cap.anchorMax = new Vector2(0.5f, 1f);
            cap.pivot = new Vector2(0.5f, 1f);
            cap.sizeDelta = new Vector2(960f, 204f);
            cap.anchoredPosition = new Vector2(0f, -24f);
            Img(cap, S("UI/caption_banner"), Color.white, sliced: true);

            var goal = Rect("Goal", cap);
            goal.anchorMin = goal.anchorMax = new Vector2(0.5f, 1f);
            goal.pivot = new Vector2(0.5f, 1f);
            goal.sizeDelta = new Vector2(880f, 64f);
            goal.anchoredPosition = new Vector2(0f, -28f);
            c.captionGoal = Label(goal, string.Empty, f4, new Color32(0x2a, 0x3a, 0x72, 0xff), TextAnchor.MiddleCenter, shadow: false);
            c.captionGoal.horizontalOverflow = HorizontalWrapMode.Wrap;

            var body = Rect("Body", cap);
            body.anchorMin = body.anchorMax = new Vector2(0.5f, 1f);
            body.pivot = new Vector2(0.5f, 1f);
            body.sizeDelta = new Vector2(880f, 96f);
            body.anchoredPosition = new Vector2(0f, -96f);
            c.captionBody = Label(body, string.Empty, f4, Ink, TextAnchor.UpperCenter, shadow: false);
            c.captionBody.horizontalOverflow = HorizontalWrapMode.Wrap;
            c.captionBody.lineSpacing = 1f;

            c.caption = cap;
            cap.gameObject.SetActive(false);
        }

        static void BuildActionBar(RectTransform ui, LevelController c)
        {
            var holder = Stretch(Rect("Action Bar", ui));
            var bar = holder.gameObject.AddComponent<ActionBar>();
            bar.panel = Panel(holder, ScreenPanel.Style.Slide);
            var slide = Stretch(Rect("Slide", holder));
            bar.panel.content = slide;

            var row = Rect("Row", slide);
            row.anchorMin = row.anchorMax = new Vector2(0.5f, 0f);
            row.pivot = new Vector2(0.5f, 0f);
            row.sizeDelta = new Vector2(1780f, 124f);
            row.anchoredPosition = new Vector2(0f, 12f);
            var layout = row.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 20f;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = layout.childControlHeight = false;
            layout.childForceExpandWidth = layout.childForceExpandHeight = false;

            bar.buttons = new ActionButton[4];
            for (var i = 0; i < 4; i++)
            {
                var b = Rect("Action " + (i + 1), row);
                b.sizeDelta = new Vector2(430f, 124f);
                var img = Img(b, S("UI/action_button"), Color.white, sliced: true, raycast: true);
                var button = b.gameObject.AddComponent<Button>();
                button.targetGraphic = img;
                button.transition = Selectable.Transition.None;
                button.navigation = new Navigation { mode = Navigation.Mode.None };

                var face = Stretch(Rect("Face", b));
                face.offsetMin = new Vector2(0f, 12f); // sit above the 3-pixel lip
                var h = face.gameObject.AddComponent<HorizontalLayoutGroup>();
                h.padding = new RectOffset(20, 16, 8, 8);
                h.spacing = 16f;
                h.childAlignment = TextAnchor.MiddleLeft;
                h.childControlWidth = h.childControlHeight = true;
                h.childForceExpandWidth = h.childForceExpandHeight = false;

                var ir = Rect("Icon", face);
                var iconImg = Img(ir, S("UI/icon_desk"), Color.white);
                var ile = ir.gameObject.AddComponent<LayoutElement>();
                ile.preferredWidth = ile.preferredHeight = 16f * Px;

                var lr = Rect("Label", face);
                var label = Label(lr, string.Empty, f4, Ink, TextAnchor.MiddleLeft, shadow: false);
                label.horizontalOverflow = HorizontalWrapMode.Wrap;
                label.lineSpacing = 0.9f;
                var lle = lr.gameObject.AddComponent<LayoutElement>();
                lle.flexibleWidth = 1f;
                lle.preferredWidth = 280f;

                var bounce = b.gameObject.AddComponent<PressBounce>();
                bounce.face = face;
                bounce.sound = Sfx.Click;

                var tried = Rect("Tried", b);
                tried.anchorMin = tried.anchorMax = new Vector2(1f, 1f);
                tried.pivot = new Vector2(1f, 1f);
                tried.sizeDelta = new Vector2(130f, 52f);
                tried.anchoredPosition = new Vector2(-8f, -8f);
                Img(tried, S("UI/button_gray"), Color.white, sliced: true);
                var triedText = Stretch(Rect("Text", tried));
                triedText.offsetMin = new Vector2(0f, 8f);
                Label(triedText, UIStrings.Tried, f3, Color.white, TextAnchor.MiddleCenter);
                tried.gameObject.SetActive(false);

                var action = b.gameObject.AddComponent<ActionButton>();
                action.button = button;
                action.icon = iconImg;
                action.label = label;
                action.triedMark = tried.gameObject;
                action.group = b.gameObject.AddComponent<CanvasGroup>();
                bar.buttons[i] = action;
            }

            // Every 16x16 Choice icon under Assets/Art/UI (the pilot's and each rollout slice's), matched by name.
            bar.icons = System.IO.Directory.GetFiles("Assets/Art/UI", "icon_*.png")
                .Select(path => S("UI/" + System.IO.Path.GetFileNameWithoutExtension(path)))
                .Where(sprite => sprite != null)
                .OrderBy(sprite => sprite.name, System.StringComparer.Ordinal)
                .ToArray();
            bar.smallFont = f3;
            // 430 wide, less the padding (20 + 16), the icon (64) and the spacing (16)
            bar.labelWidth = 430f - 20f - 16f - 16f * Px - 16f;
            c.actionBar = bar;
            holder.gameObject.SetActive(false);
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
            // Animate a parent so the Choice row can resize without moving the slide's rest position.
            var slide = Stretch(Rect("Slide", holder));
            var row = Rect("Row", slide);
            row.anchorMin = row.anchorMax = new Vector2(0.5f, 0f);
            row.pivot = new Vector2(0.5f, 0f);
            row.sizeDelta = new Vector2(1720f, 340f);
            row.anchoredPosition = new Vector2(0f, 28f);
            var layout = row.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 28f;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = layout.childControlHeight = false;
            layout.childForceExpandWidth = layout.childForceExpandHeight = false;
            c.choicesPanel.content = slide;

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
                card.compactLabelFont = f3;
                card.compactLabelWidth = 384f;
                card.triedMark = tried.gameObject;
                card.group = b.gameObject.AddComponent<CanvasGroup>();
                tried.gameObject.SetActive(false);
                c.choiceCards[i] = card;
            }

            c.choicesLayout = holder.gameObject.AddComponent<LevelChoicesLayout>();
            c.choicesLayout.row = row;
            c.choicesLayout.cards = c.choiceCards;
            c.choicesLayout.compactHeight = 272f;
            c.choicesLayout.compactBottomInset = 10f;
            c.choicesLayout.compactPictureScale = 11f / 16f;
            c.choicesLayout.compactPictureTopInset = 8f;
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
            c.feedbackPictureHolder = holder.gameObject;
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

            var opened = Rect("Opened Ribbon", box);
            opened.anchorMin = opened.anchorMax = new Vector2(0.5f, 1f);
            opened.pivot = new Vector2(0.5f, 0.5f);
            opened.sizeDelta = new Vector2(520f, 80f);
            opened.anchoredPosition = new Vector2(-370f, -428f);
            Img(opened, S("UI/button_yellow"), Color.white, sliced: true);

            var hat = S("Icons/icon_hardhat");
            var hatR = Rect("Hard Hat", opened);
            Centered(hatR, -204f, 4f, hat.rect.width * Px, hat.rect.height * Px);
            Img(hatR, hat, Color.white);

            var openedLabel = Stretch(Rect("Text", opened));
            openedLabel.offsetMin = new Vector2(104f, 12f);
            openedLabel.offsetMax = new Vector2(-16f, 0f);
            c.openedText = Label(openedLabel, string.Empty, f4, Ink, TextAnchor.MiddleCenter, shadow: false);
            c.openedRibbon = opened.gameObject;
            c.openedRibbon.SetActive(false);

            // Leave room for Bantay's two-line bubble below the title.
            c.cheerBantay = BuildBantay(box);
            var dog = (RectTransform)c.cheerBantay.transform;
            dog.anchorMin = dog.anchorMax = new Vector2(1f, 1f);
            dog.pivot = new Vector2(1f, 0f);
            dog.anchoredPosition = new Vector2(-150f, -504f);

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

            // The Student flow shows the happy Student here, and a speech bubble where Bantay stands.
            var studentHolder = Rect("Student Holder", box);
            studentHolder.anchorMin = studentHolder.anchorMax = new Vector2(0f, 1f);
            studentHolder.pivot = new Vector2(0f, 1f);
            studentHolder.sizeDelta = new Vector2(320f, 232f);
            studentHolder.anchoredPosition = new Vector2(70f, -476f);
            Img(studentHolder, S("UI/input_box"), Color.white, sliced: true);
            var happy = BuildStudent(studentHolder, withBubble: false);
            var happyRect = (RectTransform)happy.transform;
            happyRect.anchorMin = happyRect.anchorMax = new Vector2(0.5f, 0f);
            happyRect.anchoredPosition = new Vector2(0f, 14f);
            happy.unitsPerPixel = Px;
            c.completedStudent = happy;
            studentHolder.gameObject.SetActive(false);

            var cheer = BuildSpeechBubble(box, 620f, 150f);
            var cheerR = (RectTransform)cheer.transform;
            cheerR.anchorMin = cheerR.anchorMax = new Vector2(1f, 1f);
            cheerR.pivot = new Vector2(1f, 1f);
            cheerR.anchoredPosition = new Vector2(-80f, -250f);
            cheer.Hide();
            c.completedBubble = cheer;

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
            c.lessonText.lineSpacing = 1f;

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
