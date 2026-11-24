using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    /// <summary>
    /// Checks the authored Scenario files the game ships, so a content mistake fails a test
    /// instead of reaching a Learner. Every safety line must cite a finding in the research notes.
    /// </summary>
    public class ScenarioContentTests
    {
        const string ScenarioFolder = "Assets/Resources/Scenarios";
        const string ResearchFolder = "docs/research";
        const string FontScript = "ArtSource/scripts/font.lua";
        const int MaxWordsPerSentence = 12;

        static IEnumerable<string> Files() =>
            Directory.Exists(ScenarioFolder)
                ? Directory.GetFiles(ScenarioFolder, "*.json", SearchOption.AllDirectories)
                : Enumerable.Empty<string>();

        static Scenario Load(string path)
        {
            var s = Scenario.Parse(File.ReadAllText(path));
            Assert.That(s, Is.Not.Null, path + " is not valid JSON");
            return s;
        }

        static IEnumerable<string> AllText(Scenario s)
        {
            yield return s.goal?.text;
            yield return s.intro?.text;
            yield return s.hint?.text;
            yield return s.lesson?.text;
            foreach (var c in s.choices ?? new ScenarioChoice[0])
            {
                yield return c.label;
                yield return c.heading;
                yield return c.feedback;
            }
        }

        static IEnumerable<string> AllSources(Scenario s)
        {
            var lines = new[] { s.goal, s.hint, s.lesson, s.intro }.Where(l => l?.sources != null).SelectMany(l => l.sources);
            var choices = (s.choices ?? new ScenarioChoice[0]).Where(c => c.sources != null).SelectMany(c => c.sources);
            return lines.Concat(choices);
        }

        [Test]
        public void EasyLevels_AllExist()
        {
            var levels = Files().Select(Load).Select(s => s.level).ToList();
            Assert.That(levels, Is.SupersetOf(new[] { "Earthquake/Easy/1", "Earthquake/Easy/2", "Earthquake/Easy/3" }));
        }

        [Test]
        public void NormalLevels_AllExist()
        {
            var levels = Files().Select(Load).Select(s => s.level).ToList();
            Assert.That(levels, Is.SupersetOf(new[]
            {
                "Earthquake/Normal/1", "Earthquake/Normal/2", "Earthquake/Normal/3",
                "Earthquake/Normal/4", "Earthquake/Normal/5",
            }));
        }

        [Test]
        public void HardLevels_AllExist()
        {
            var levels = Files().Select(Load).Select(s => s.level).ToList();
            Assert.That(levels, Is.SupersetOf(new[]
            {
                "Earthquake/Hard/1", "Earthquake/Hard/2", "Earthquake/Hard/3", "Earthquake/Hard/4",
                "Earthquake/Hard/5", "Earthquake/Hard/6", "Earthquake/Hard/7",
            }));
        }

        [Test]
        public void EveryChoicePicture_HasArt()
        {
            foreach (var path in Files())
            foreach (var choice in Load(path).choices)
            {
                var picture = "Assets/Art/Level/" + choice.picture + ".png";
                Assert.That(File.Exists(picture), Is.True, path + ": missing " + picture);
            }
        }

        [Test]
        public void EveryNewFlowIcon_HasArt()
        {
            foreach (var path in Files())
            {
                var s = Load(path);
                if (!s.UsesActions) continue;
                foreach (var choice in s.choices)
                {
                    var icon = "Assets/Art/UI/" + choice.icon + ".png";
                    Assert.That(File.Exists(icon), Is.True, path + ": missing " + icon);
                }
            }
        }

        [Test]
        public void OnlyTheMovedLevels_UseTheStudentFlow()
        {
            var levels = Files().Select(Load).Where(s => s.UsesActions).Select(s => s.level).OrderBy(l => l).ToList();
            Assert.That(levels, Is.EqualTo(new[]
            {
                "Earthquake/Easy/1", "Earthquake/Easy/2", "Earthquake/Easy/3", "Earthquake/Hard/1", "Earthquake/Hard/2",
                "Earthquake/Hard/3", "Earthquake/Hard/4", "Earthquake/Hard/5", "Earthquake/Hard/6",
                "Earthquake/Hard/7",
                "Earthquake/Normal/1", "Earthquake/Normal/2", "Earthquake/Normal/3", "Earthquake/Normal/4",
                "Earthquake/Normal/5",
            }));
            foreach (var path in Files())
            {
                var s = Load(path);
                if (s.UsesActions) continue;
                Assert.That(s.ChoicesWithAction, Is.EqualTo(0), path + " must use the old flow, with no action on any Choice");
            }
        }

        [Test]
        public void EasyLevel2_UsesTheStudentFlow_InTheCorridor_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Easy/2.json"));
            Assert.That(s.UsesActions, Is.True);
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.Corridor));
            Assert.That(ScenarioSettings.ActionSpots(ScenarioSettings.Corridor),
                Is.EquivalentTo(new[] { "stairs", "window", "cover", "plants" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(new[] { "stairs", "window", "cover", "plants" }));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_stairs", "icon_window", "icon_cover", "icon_plants" }));
            // The rollout changes no Scenario text (docs/specs/earthquake-rollout.md, Data).
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Run down the stairs", "Stand by the window", "Duck and cover your head", "Stand under the hanging plants",
            }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Stay where you are while it shakes. Walk out when it stops.",
                "Window glass can break. Stay away from windows.",
                "Duck down low. Cover your head with your arms or a book.",
                "Hanging pots can swing and fall. Stay away from them.",
            }));
        }

        [Test]
        public void EasyLevel3_UsesTheStudentFlow_InTheClassroomAfter_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Easy/3.json"));
            Assert.That(s.UsesActions, Is.True);
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.After));
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.ClassroomAfter));
            Assert.That(ScenarioSettings.ActionSpots(s.setting), Is.EquivalentTo(new[] { "stay", "walk", "push" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(new[] { "stay", "walk", "push" }));
            Assert.That(s.choices.Select(c => c.icon), Is.EqualTo(new[] { "icon_stay", "icon_teacher_walk", "icon_push" }));
            Assert.That(s.SafeChoice.id, Is.EqualTo("walk"));
            Assert.That(s.choices.Select(c => c.picture), Is.EqualTo(new[] { "choice_stay", "choice_walk", "choice_push" }));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Stay under the desk", "Walk out with your teacher", "Run and push to the door",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[] { "Time to go", "", "" }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "The shaking stopped. Now leave the classroom with your teacher.",
                "Walk out with your class. Follow your teacher to the evacuation area.",
                "Walk. Do not run. Do not push.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-19", "EQ-34", "EQ-35" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-34", "EQ-35", "EQ-37" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-20", "EQ-36" }));
        }

        [Test]
        public void NormalLevel1_UsesTheStudentFlow_InTheBedroom_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Normal/1.json"));
            Assert.That(s.UsesActions, Is.True);
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.Before));
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.Bedroom));
            Assert.That(ScenarioSettings.CanShake(s.setting), Is.False);
            Assert.That(ScenarioSettings.ActionSpots(s.setting),
                Is.EquivalentTo(new[] { "picture", "low_shelf", "bed_window", "books_top" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(new[] { "picture", "low_shelf", "bed_window", "books_top" }));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_picture", "icon_low_shelf", "icon_bed", "icon_books" }));
            Assert.That(s.SafeChoice.id, Is.EqualTo("low_shelf"));
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_picture_bed", "choice_low_shelf", "choice_bed_window", "choice_books_top" }));
            // The rollout changes no Scenario text (docs/specs/earthquake-rollout.md, Data).
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Hang a big picture over the bed", "Put heavy things on a low shelf",
                "Move the bed under the window", "Put books on top of the tall cabinet",
            }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "A heavy picture can fall on your bed. Hang it somewhere else.",
                "Keep heavy things on low shelves, not high ones.",
                "Window glass can break. Keep your bed away from windows.",
                "Things up high can fall when it shakes. Keep them low.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-62" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-61" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-62", "EQ-14" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-61", "EQ-31" }));
        }

        [Test]
        public void NormalLevel2_UsesTheStudentFlow_InTheHome_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Normal/2.json"));
            Assert.That(s.UsesActions, Is.True);
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.Home));
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.Before));
            Assert.That(s.Shakes, Is.False);
            Assert.That(ScenarioSettings.ActionSpots(s.setting), Is.EqualTo(new[]
            {
                "later", "hidden", "go_bag", "toys",
                "run", "glass", "sofa", "table",
                "own_way", "meeting_place", "meet_home", "phone_numbers",
            }));
            Assert.That(s.choices.Select(c => c.id), Is.EqualTo(new[] { "later", "hidden", "go_bag", "toys" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(s.choices.Select(c => c.id)));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_pack_later", "icon_bag_hidden", "icon_go_bag", "icon_snack_toy" }));
            Assert.That(s.SafeChoice.id, Is.EqualTo("go_bag"));
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_pack_later", "choice_bag_hidden", "choice_go_bag", "choice_bag_toys" }));
            Assert.That(s.goal.text, Is.EqualTo("Get your go bag ready."));
            Assert.That(s.intro.text, Is.EqualTo("Your family is packing a go bag. Let's help!"));
            Assert.That(s.hint.text, Is.EqualTo("Think of what you need for 3 days."));
            Assert.That(s.lesson.text, Is.EqualTo("Pack a go bag before an earthquake. It should last 3 days and be easy to reach."));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Pack it later, when it shakes",
                "Hide it under the boxes",
                "Pack water, food, a flashlight and a whistle",
                "Pack only snacks and toys",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[] { "Not ready yet", "", "", "Not ready yet" }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Get it ready now. Keep it where you can grab it.",
                "Keep your go bag in a place that is easy to find.",
                "These help your family for 3 days. Keep the bag easy to reach.",
                "A go bag needs water, food and a light for 3 days.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-52", "EQ-68" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-68" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-68", "EQ-69" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-68", "EQ-69" }));
        }

        [Test]
        public void NormalLevel3_UsesTheStudentFlow_InTheHome_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Normal/3.json"));
            Assert.That(s.UsesActions, Is.True);
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.Home));
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.During));
            Assert.That(s.Shakes, Is.True);
            Assert.That(s.choices.Select(c => c.id), Is.EqualTo(new[] { "run", "glass", "sofa", "table" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(s.choices.Select(c => c.id)));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_door", "icon_glass_cabinet", "icon_sofa", "icon_strong_table" }));
            Assert.That(s.SafeChoice.id, Is.EqualTo("table"));
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_run_home", "choice_glass_cabinet", "choice_sofa", "choice_table" }));
            Assert.That(s.goal.text, Is.EqualTo("Stay safe when the ground shakes at home."));
            Assert.That(s.intro.text, Is.EqualTo("You are in the living room. The ground is shaking!"));
            Assert.That(s.hint.text, Is.EqualTo("Is there something strong to hide under?"));
            Assert.That(s.lesson.text, Is.EqualTo("At home, Duck, Cover, and Hold under a strong table. Stay away from glass."));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Run out the door",
                "Stand by the glass cabinet",
                "Lie down beside the sofa",
                "Duck under the strong table",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[] { "Not safe yet", "", "", "" }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Do not run while it shakes. Stay inside and take cover.",
                "Glass and shelves can fall and break. Stay away from them.",
                "Lying beside furniture is not the safe way. Get under a strong table.",
                "Duck, Cover, and Hold under the table. Stay until the shaking stops.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-77", "EQ-78", "EQ-74" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-76" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-79", "EQ-75" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-74", "EQ-75", "EQ-13" }));
        }

        [Test]
        public void HardLevel1_UsesTheStudentFlow_InTheHome_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Hard/1.json"));
            Assert.That(s.UsesActions, Is.True);
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.Home));
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.Before));
            Assert.That(s.Shakes, Is.False);
            Assert.That(s.choices.Select(c => c.id), Is.EqualTo(new[] { "own_way", "meeting_place", "meet_home", "phone_numbers" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(s.choices.Select(c => c.id)));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_own_way", "icon_meet_park", "icon_meet_house", "icon_phone_numbers" }));
            Assert.That(s.SafeChoice.id, Is.EqualTo("meeting_place"));
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_own_way", "choice_meeting_place", "choice_meet_home", "choice_phone_numbers" }));
            Assert.That(s.goal.text, Is.EqualTo("Make a family plan for earthquakes."));
            Assert.That(s.intro.text, Is.EqualTo("Your family is making an earthquake plan. Join in!"));
            Assert.That(s.hint.text, Is.EqualTo("Where will your family meet after it shakes?"));
            Assert.That(s.lesson.text, Is.EqualTo("Plan with your family. Agree on a meeting place and know a family number."));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Each of us finds our own way",
                "Agree on a meeting place",
                "Plan to meet back at home",
                "Keep numbers only in the phone",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[] { "Not ready yet", "", "", "Not ready yet" }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Agree on one place to meet. Then you can find each other.",
                "Good plan. Also remember one family phone number.",
                "Your house may be damaged. Meet at an open place.",
                "Phones can be lost. Remember or write down a family number.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-64", "EQ-65" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-64", "EQ-67" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-94", "EQ-55" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-67" }));
        }

        [Test]
        public void NormalLevel4_UsesTheStudentFlow_InTheSchoolyard_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Normal/4.json"));
            Assert.That(s.UsesActions, Is.True);
            Assert.That(s.level, Is.EqualTo("Earthquake/Normal/4"));
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.Schoolyard));
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.During));
            Assert.That(s.Shakes, Is.True);
            Assert.That(ScenarioSettings.ActionSpots(s.setting), Is.EqualTo(new[]
            {
                "tree", "field", "building", "post",
                "route_post", "route_tree", "route_field", "route_later",
            }));
            Assert.That(s.choices.Select(c => c.id), Is.EqualTo(new[] { "tree", "field", "building", "post" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(s.choices.Select(c => c.id)));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_tree", "icon_field", "icon_building", "icon_post" }));
            Assert.That(s.SafeChoice.id, Is.EqualTo("field"));
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_tree", "choice_open_field", "choice_building", "choice_post" }));
            Assert.That(s.goal.text, Is.EqualTo("Stay safe when it shakes outside."));
            Assert.That(s.goal.sources, Is.EqualTo(new[] { "EQ-80" }));
            Assert.That(s.intro.text, Is.EqualTo("It is recess. You are outside when the ground shakes!"));
            Assert.That(s.intro.sources, Is.EqualTo(System.Array.Empty<string>()));
            Assert.That(s.hint.text, Is.EqualTo("Where is there nothing that can fall on you?"));
            Assert.That(s.hint.sources, Is.EqualTo(new[] { "EQ-80", "EQ-81" }));
            Assert.That(s.lesson.text,
                Is.EqualTo("Outside? Move to an open area, away from trees, posts and buildings."));
            Assert.That(s.lesson.sources, Is.EqualTo(new[] { "EQ-80", "EQ-81" }));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Hide under the big tree",
                "Go to the open field",
                "Run back into the building",
                "Stand by the electric post",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[]
            {
                "",
                "",
                "Not safe yet",
                "",
            }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Stay away from trees when the ground shakes.",
                "Move to an open area. Stay away from trees, posts and buildings.",
                "Stay away from buildings while it shakes. Go to an open area.",
                "Stay away from posts and power lines.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-81" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-80", "EQ-81" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-81", "EQ-80" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-81" }));
        }

        [Test]
        public void NormalLevel5_UsesTheStudentFlow_OnTheBeach_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Normal/5.json"));
            Assert.That(s.UsesActions, Is.True, "Normal 5 uses the Student flow");
            Assert.That(s.level, Is.EqualTo("Earthquake/Normal/5"));
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.Beach));
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.After));
            Assert.That(s.Shakes, Is.False);
            Assert.That(ScenarioSettings.ActionSpots(s.setting), Is.EqualTo(new[] { "fish", "watch", "high_ground", "wait" }));
            Assert.That(s.choices.Select(c => c.id), Is.EqualTo(new[] { "fish", "watch", "high_ground", "wait" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(s.choices.Select(c => c.id)));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_fish", "icon_watch_waves", "icon_high_ground", "icon_wait_beach" }));
            Assert.That(s.SafeChoice.id, Is.EqualTo("high_ground"));
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_fish", "choice_watch_waves", "choice_high_ground", "choice_wait_beach" }));
            Assert.That(s.goal.text, Is.EqualTo("Stay safe near the sea after a strong earthquake."));
            Assert.That(s.goal.sources, Is.EqualTo(new[] { "EQ-84" }));
            Assert.That(s.intro.text, Is.EqualTo("You are at the beach. The shaking stopped. The sea is going out!"));
            Assert.That(s.intro.sources, Is.EqualTo(new[] { "EQ-85" }));
            Assert.That(s.hint.text, Is.EqualTo("The water can come back fast. Where can it not reach?"));
            Assert.That(s.hint.sources, Is.EqualTo(new[] { "EQ-86", "EQ-87" }));
            Assert.That(s.lesson.text,
                Is.EqualTo("Near the sea, a strong earthquake can bring a tsunami. Go to high ground and stay until the all clear."));
            Assert.That(s.lesson.sources, Is.EqualTo(new[] { "EQ-84", "EQ-86", "EQ-89" }));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Pick up the fish on the sand",
                "Stay and watch the waves",
                "Go to high ground right away",
                "Wait on the beach for news",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[] { "", "", "", "" }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Never go down the beach for fish. Go to high ground.",
                "Never stay to watch for a tsunami. Go to high ground.",
                "A strong earthquake near the sea can bring a tsunami. Move to high ground now.",
                "Do not stay in low places by the sea. A warning may not come in time.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-88", "EQ-86" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-88" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-84", "EQ-86" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-87" }));
        }

        [Test]
        public void HardLevel2_UsesTheStudentFlow_InTheSchoolyard_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Hard/2.json"));
            Assert.That(s.UsesActions, Is.True);
            Assert.That(s.level, Is.EqualTo("Earthquake/Hard/2"));
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.Schoolyard));
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.Before));
            Assert.That(s.Shakes, Is.False);
            Assert.That(ScenarioSettings.ActionSpots(s.setting), Is.EqualTo(new[]
            {
                "tree", "field", "building", "post",
                "route_post", "route_tree", "route_field", "route_later",
            }));
            Assert.That(s.choices.Select(c => c.id), Is.EqualTo(new[] { "route_post", "route_tree", "route_field", "route_later" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(s.choices.Select(c => c.id)));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_route_post", "icon_route_tree", "icon_route_field", "icon_route_later" }));
            Assert.That(s.SafeChoice.id, Is.EqualTo("route_field"));
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_route_post", "choice_route_tree", "choice_route_field", "choice_route_later" }));
            Assert.That(s.goal.text, Is.EqualTo("Know your way out of school."));
            Assert.That(s.goal.sources, Is.EqualTo(new[] { "EQ-53", "EQ-54" }));
            Assert.That(s.intro.text, Is.EqualTo("Your class is learning the evacuation route today."));
            Assert.That(s.intro.sources, Is.EqualTo(System.Array.Empty<string>()));
            Assert.That(s.hint.text, Is.EqualTo("Which way has nothing that can fall?"));
            Assert.That(s.hint.sources, Is.EqualTo(new[] { "EQ-56" }));
            Assert.That(s.lesson.text,
                Is.EqualTo("Know your school exits. Learn the safe route to the evacuation area."));
            Assert.That(s.lesson.sources, Is.EqualTo(new[] { "EQ-53", "EQ-54" }));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Take the path by the post",
                "Walk the path under the tree",
                "Take the route across the field",
                "Find the way when it happens",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[]
            {
                "",
                "",
                "",
                "Not ready yet",
            }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Posts can fall when it shakes. Pick a route away from them.",
                "Trees can fall too. Choose a route in the open.",
                "This route goes to the evacuation area. Nothing can fall on it.",
                "Learn the route now, in drills. Then you will know it.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-56", "EQ-81" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-81", "EQ-56" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-54", "EQ-56" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-53", "EQ-71" }));
        }

        [Test]
        public void HardLevel3_UsesTheStudentFlow_InTheMallFoodCourt_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Hard/3.json"));
            Assert.That(s.UsesActions, Is.True, "Hard 3 uses the Student flow");
            Assert.That(s.level, Is.EqualTo("Earthquake/Hard/3"));
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.MallA));
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.During));
            Assert.That(s.Shakes, Is.True);
            var ids = new[] { "run_exit", "elevator", "shop_window", "table" };
            Assert.That(ScenarioSettings.ActionSpots(s.setting), Is.EqualTo(ids));
            Assert.That(s.choices.Select(c => c.id), Is.EqualTo(ids));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(ids));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_door", "icon_elevator", "icon_shop_window", "icon_strong_table" }));
            Assert.That(s.SafeChoice.id, Is.EqualTo("table"));
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_run_exit", "choice_elevator_during", "choice_shop_window", "choice_food_court_table" }));
            Assert.That(s.goal.text,
                Is.EqualTo("Stay safe when it shakes at the mall."));
            Assert.That(s.goal.sources, Is.EqualTo(new[] { "EQ-16" }));
            Assert.That(s.intro.text,
                Is.EqualTo("You are at the mall food court. The ground is shaking!"));
            Assert.That(s.intro.sources, Is.EqualTo(System.Array.Empty<string>()));
            Assert.That(s.hint.text,
                Is.EqualTo("Is there a strong table near you?"));
            Assert.That(s.hint.sources, Is.EqualTo(new[] { "EQ-75" }));
            Assert.That(s.lesson.text,
                Is.EqualTo("In the mall, Duck, Cover, and Hold. Stay away from glass and elevators."));
            Assert.That(s.lesson.sources, Is.EqualTo(new[] { "EQ-74", "EQ-76", "EQ-78" }));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Run to the mall exit",
                "Ride the elevator down",
                "Stand by the glass shop window",
                "Duck under the food court table",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[]
            {
                "Not safe yet",
                "",
                "",
                "",
            }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Do not run while it shakes. Take cover where you are.",
                "Never use the elevator in an earthquake. Take cover first.",
                "Glass can break and fall. Stay away from shop windows.",
                "Duck, Cover, and Hold. Stay until the shaking stops.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-77", "EQ-16", "EQ-74" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-78" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-14", "EQ-76" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-74", "EQ-75", "EQ-13" }));
        }

        [Test]
        public void HardLevel4_UsesTheStudentFlow_OnTheWayOutOfTheMall_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Hard/4.json"));
            Assert.That(s.UsesActions, Is.True, "Hard 4 uses the Student flow");
            Assert.That(s.level, Is.EqualTo("Earthquake/Hard/4"));
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.MallB));
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.After));
            Assert.That(s.Shakes, Is.False);
            var ids = new[] { "stairs", "elevator", "run_push", "mall_wall" };
            Assert.That(ScenarioSettings.ActionSpots(s.setting), Is.EqualTo(ids));
            Assert.That(s.choices.Select(c => c.id), Is.EqualTo(ids));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(ids));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_stairs", "icon_elevator", "icon_run_push", "icon_mall_wall" }));
            Assert.That(s.SafeChoice.id, Is.EqualTo("stairs"));
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_mall_stairs", "choice_elevator_after", "choice_run_push", "choice_mall_wall" }));
            Assert.That(s.goal.text,
                Is.EqualTo("Leave the mall safely after the shaking."));
            Assert.That(s.goal.sources, Is.EqualTo(new[] { "EQ-92" }));
            Assert.That(s.intro.text,
                Is.EqualTo("The shaking stopped. Everyone is leaving the mall."));
            Assert.That(s.intro.sources, Is.EqualTo(System.Array.Empty<string>()));
            Assert.That(s.hint.text,
                Is.EqualTo("What is the safe way down after an earthquake?"));
            Assert.That(s.hint.sources, Is.EqualTo(new[] { "EQ-92" }));
            Assert.That(s.lesson.text,
                Is.EqualTo("After the shaking, use the stairs and walk out. Wait in an open area."));
            Assert.That(s.lesson.sources, Is.EqualTo(new[] { "EQ-92", "EQ-38" }));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Walk down the stairs and out",
                "Take the elevator down",
                "Run and push to get out",
                "Wait by the mall wall outside",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[]
            {
                "",
                "",
                "Not safe yet",
                "Not safe yet",
            }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Walk, do not run. Go to the open parking area.",
                "Do not use elevators after an earthquake. Use the stairs.",
                "Walk fast, but do not run or push.",
                "Things can fall from the building. Move to the open area.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-92", "EQ-36", "EQ-38" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-92" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-92", "EQ-36" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-94", "EQ-81" }));
        }

        [Test]
        public void HardLevel5_UsesTheStudentFlow_InTheHomeAfter_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Hard/5.json"));
            Assert.That(s.UsesActions, Is.True, "Hard 5 uses the Student flow");
            Assert.That(s.level, Is.EqualTo("Earthquake/Hard/5"));
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.HomeAfter));
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.After));
            Assert.That(s.Shakes, Is.False);
            var ids = new[] { "pull_up", "call_adult", "fix_pipe", "leave" };
            Assert.That(ScenarioSettings.ActionSpots(s.setting),
                Is.EqualTo(new[]
                {
                    "pull_up", "call_adult", "fix_pipe", "leave",
                    "no_go_bag", "toys", "no_note", "go_bag",
                }));
            Assert.That(s.choices.Select(c => c.id), Is.EqualTo(ids));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(ids));
            Assert.That(s.SafeChoice.id, Is.EqualTo("call_adult"));
            Assert.That(s.choices.Select(c => c.icon), Is.Unique);
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_pull_up", "icon_call_adult", "icon_fix_pipe", "icon_door" }));
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_pull_up", "choice_call_adult", "choice_fix_pipe", "choice_leave_hurt" }));
            Assert.That(s.goal.text, Is.EqualTo("Help when someone is hurt after an earthquake."));
            Assert.That(s.goal.sources, Is.EqualTo(new[] { "EQ-90" }));
            Assert.That(s.intro.text, Is.EqualTo("The shaking stopped. Someone in your family hurt their leg."));
            Assert.That(s.intro.sources, Is.EqualTo(System.Array.Empty<string>()));
            Assert.That(s.hint.text, Is.EqualTo("Who can help them the right way?"));
            Assert.That(s.hint.sources, Is.EqualTo(new[] { "EQ-90" }));
            Assert.That(s.lesson.text, Is.EqualTo("After the shaking, check yourself and others. Get help for anyone who is hurt."));
            Assert.That(s.lesson.sources, Is.EqualTo(new[] { "EQ-90" }));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Pull them up to walk",
                "Call a grown-up for help",
                "Fix the broken pipe yourself",
                "Leave them and go outside",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[]
            {
                "Not safe yet",
                "",
                "",
                "",
            }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Do not move them. Keep them still and get help.",
                "Good. Keep them still and get help from an adult.",
                "Adults check water and electric lines. Help them first.",
                "Stay and call for help. They need an adult.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-90" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-90", "EQ-72" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-91", "EQ-72" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-90" }));
        }

        [Test]
        public void HomeAfter_ActOutLines_MatchTheSpec()
        {
            var strings = System.Type.GetType("DisasterSafety.Game.UIStrings, DisasterSafety.Game");
            Assert.That(strings, Is.Not.Null);
            Assert.That(strings.GetField("ActOutStayStill")?.GetRawConstantValue(),
                Is.EqualTo("Stay still, Mom."), "the Act-out line by Mom matches the spec");
            Assert.That(strings.GetField("ActOutHelp")?.GetRawConstantValue(), Is.EqualTo("Help!"));
        }

        [Test]
        public void HardLevel6_UsesTheStudentFlow_InTheHomeAfter_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Hard/6.json"));
            Assert.That(s.UsesActions, Is.True, "Hard 6 uses the Student flow");
            Assert.That(s.level, Is.EqualTo("Earthquake/Hard/6"));
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.HomeAfter));
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.After));
            Assert.That(s.Shakes, Is.False);
            var ids = new[] { "no_go_bag", "toys", "no_note", "go_bag" };
            Assert.That(ScenarioSettings.ActionSpots(s.setting),
                Is.EqualTo(new[]
                {
                    "pull_up", "call_adult", "fix_pipe", "leave",
                    "no_go_bag", "toys", "no_note", "go_bag",
                }));
            Assert.That(s.choices.Select(c => c.id), Is.EqualTo(ids));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(ids));
            Assert.That(s.SafeChoice.id, Is.EqualTo("go_bag"));
            Assert.That(s.choices.Select(c => c.icon), Is.Unique);
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_no_go_bag", "icon_toy_car", "icon_no_note", "icon_go_bag" }));
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_no_go_bag", "choice_back_for_toys", "choice_no_note", "choice_go_bag_out" }));
            Assert.That(s.goal.text, Is.EqualTo("Leave home safely after the earthquake."));
            Assert.That(s.goal.sources, Is.EqualTo(new[] { "EQ-92" }));
            Assert.That(s.intro.text, Is.EqualTo("Your family must leave the house. What do you bring?"));
            Assert.That(s.intro.sources, Is.EqualTo(System.Array.Empty<string>()));
            Assert.That(s.hint.text, Is.EqualTo("What did your family pack for 3 days?"));
            Assert.That(s.hint.sources, Is.EqualTo(new[] { "EQ-68", "EQ-93" }));
            Assert.That(s.lesson.text, Is.EqualTo("Bring your go bag and walk out. Expect aftershocks."));
            Assert.That(s.lesson.sources, Is.EqualTo(new[] { "EQ-93", "EQ-92", "EQ-42" }));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Leave without the go bag",
                "Run back in for your toys",
                "Leave without a note",
                "Grab the go bag and walk out",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[]
            {
                "Not ready yet",
                "Not safe yet",
                "Not ready yet",
                "",
            }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Your go bag has what you need. Bring it with you.",
                "Do not go back in. Aftershocks can come. Walk out now.",
                "Leave a message so others know where you went.",
                "Bring the go bag. Leave a note saying where you went.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-93", "EQ-68" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-43", "EQ-42", "EQ-92" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-93" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-93" }));
        }

        [Test]
        public void HardLevel7_UsesTheStudentFlow_InTheEvacuationArea_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Hard/7.json"));
            Assert.That(s.UsesActions, Is.True, "Hard 7 uses the Student flow");
            Assert.That(s.level, Is.EqualTo("Earthquake/Hard/7"));
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.EvacuationArea));
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.After));
            Assert.That(s.Shakes, Is.False);
            var actions = new[] { "go_home", "radio", "rumor", "phone_all_day" };
            Assert.That(ScenarioSettings.ActionSpots(s.setting), Is.EqualTo(actions));
            Assert.That(s.choices.Select(c => c.id), Is.EqualTo(new[] { "go_home", "radio", "rumor", "phone" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(actions));
            Assert.That(s.SafeChoice.id, Is.EqualTo("radio"));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_go_home", "icon_radio", "icon_rumor", "icon_phone_all_day" }));
            foreach (var icon in s.choices.Select(c => c.icon))
                Assert.That(File.Exists("Assets/Art/UI/" + icon + ".png"), Is.True, icon + " has art");
            Assert.That(s.choices.Select(c => c.picture),
                Is.EqualTo(new[] { "choice_go_home", "choice_radio", "choice_rumor", "choice_phone_all_day" }));
            Assert.That(s.goal.text, Is.EqualTo("Stay safe at the evacuation area."));
            Assert.That(s.goal.sources, Is.EqualTo(new[] { "EQ-54" }));
            Assert.That(s.intro.text, Is.EqualTo("You are at the evacuation area. Your family is here!"));
            Assert.That(s.intro.sources, Is.EqualTo(System.Array.Empty<string>()));
            Assert.That(s.hint.text, Is.EqualTo("Where can you get true news?"));
            Assert.That(s.hint.sources, Is.EqualTo(new[] { "EQ-95" }));
            Assert.That(s.lesson.text,
                Is.EqualTo("Stay with your family. Listen to trusted news and do not go back inside."));
            Assert.That(s.lesson.sources, Is.EqualTo(new[] { "EQ-64", "EQ-95", "EQ-94" }));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Go home now to get things",
                "Listen to the radio for news",
                "Share news you heard from others",
                "Call friends on the phone all day",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[]
            {
                "Not safe yet",
                "",
                "",
                "Not ready yet",
            }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Do not go back into a damaged house. Wait until it is safe.",
                "Listen to official news. Wait until it is safe.",
                "Get news from trusted sources. Do not spread rumors.",
                "Use the phone only when you need it.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-94", "EQ-43" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-95", "EQ-94" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-95" }));
            Assert.That(s.choices[3].sources, Is.EqualTo(new[] { "EQ-95" }));
        }

        [Test]
        public void EvacuationArea_ActOutLine_MatchesTheSpec()
        {
            var strings = System.Type.GetType("DisasterSafety.Game.UIStrings, DisasterSafety.Game");
            Assert.That(strings, Is.Not.Null);
            Assert.That(strings.GetField("ActOutDidYouHear")?.GetRawConstantValue(), Is.EqualTo("Did you hear...?"),
                "the rumor Act-out line matches spec item 7");
        }

        [Test]
        public void EverySetting_HasArt()
        {
            foreach (var path in Files())
            {
                var background = "Assets/Art/Level/bg_" + Load(path).setting + ".png";
                Assert.That(File.Exists(background), Is.True, path + ": missing " + background);
            }
        }

        [Test]
        public void EveryScenario_PassesTheScenarioRules()
        {
            foreach (var path in Files())
                Assert.That(ScenarioRules.Problems(Load(path)), Is.Empty, path);
        }

        [Test]
        public void EveryScenario_IsStoredUnderItsOwnLevelPath()
        {
            foreach (var path in Files())
            {
                var s = Load(path);
                var expected = Path.Combine(ScenarioFolder, s.level + ".json").Replace('\\', '/');
                Assert.That(path.Replace('\\', '/'), Is.EqualTo(expected));
            }
        }

        [Test]
        public void EverySource_IsAFindingInTheResearchNotes()
        {
            var notes = string.Join("\n", Directory.GetFiles(ResearchFolder, "*.md").Select(File.ReadAllText));
            var known = new HashSet<string>(Regex.Matches(notes, @"^- ([A-Z]{2}-\d{2}):", RegexOptions.Multiline)
                .Cast<Match>().Select(m => m.Groups[1].Value));
            Assert.That(known, Is.Not.Empty, "no findings found in " + ResearchFolder);

            foreach (var path in Files())
            foreach (var id in AllSources(Load(path)))
                Assert.That(known, Does.Contain(id), path + " cites an unknown finding");
        }

        [Test]
        public void EveryCharacter_IsInThePixelFont()
        {
            var glyphs = new HashSet<char>(Regex.Matches(File.ReadAllText(FontScript), "def\\(\"(.)\"")
                .Cast<Match>().Select(m => m.Groups[1].Value[0]));
            foreach (var path in Files())
            foreach (var text in AllText(Load(path)).Where(t => t != null))
            foreach (var ch in text)
                Assert.That(glyphs, Does.Contain(ch), path + ": '" + ch + "' in \"" + text + "\"");
        }

        [Test]
        public void Sentences_StayShortForYoungReaders()
        {
            foreach (var path in Files())
            foreach (var text in AllText(Load(path)).Where(t => !string.IsNullOrWhiteSpace(t)))
            foreach (var sentence in Regex.Split(text, @"(?<=[.!?])\s+"))
            {
                var words = sentence.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries).Length;
                Assert.That(words, Is.LessThanOrEqualTo(MaxWordsPerSentence), path + ": \"" + sentence + "\"");
            }
        }

        [Test]
        public void SafetyWording_IsDuckCoverAndHold()
        {
            foreach (var path in Files())
            foreach (var text in AllText(Load(path)).Where(t => t != null))
            {
                // The slogan is "Duck, Cover, and Hold". "Hold on to it" alone is PHIVOLCS wording (EQ-11).
                Assert.That(text, Does.Not.Match(@"(?i)\bdrop\b"), path);
                Assert.That(text, Does.Not.Match(@"(?i)cover,? and hold on"), path);
            }
        }
    }
}
