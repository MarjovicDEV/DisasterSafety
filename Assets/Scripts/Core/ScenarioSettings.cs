using System;
using System.Collections.Generic;
using System.Linq;

namespace DisasterSafety.Core
{
    /// <summary>The supported Settings and their shaking capabilities.</summary>
    public static class ScenarioSettings
    {
        public const string Classroom = "classroom";
        public const string Corridor = "corridor";
        public const string ClassroomAfter = "classroom_after";
        public const string Bedroom = "bedroom";
        public const string Home = "home";
        public const string Schoolyard = "schoolyard";
        public const string Beach = "beach";
        public const string MallA = "mall_a";
        public const string MallB = "mall_b";
        public const string HomeAfter = "home_after";
        public const string EvacuationArea = "evacuation_area";

        public static IReadOnlyList<string> All { get; } =
            Array.AsReadOnly(new[]
            {
                Classroom, Corridor, ClassroomAfter, Bedroom, Home, Schoolyard, Beach,
                MallA, MallB, HomeAfter, EvacuationArea,
            });

        public static bool IsKnown(string key) => All.Contains(key);

        /// <summary>The art group the Student's flow uses for a Setting, e.g. "classroom" becomes "classroom_v2".</summary>
        public static string ActionArtKey(string setting) => setting + "_v2";

        /// <summary>The action keys (one Act-out each) a Scenario with actions may name in a Setting.</summary>
        public static IReadOnlyList<string> ActionSpots(string setting)
        {
            switch (setting)
            {
                case Classroom: return ClassroomSpots;
                case Corridor: return CorridorSpots;
                case ClassroomAfter: return ClassroomAfterSpots;
                case Bedroom: return BedroomSpots;
                case Home: return HomeSpots;
                case Schoolyard: return SchoolyardSpots;
                case Beach: return BeachSpots;
                case MallA: return MallASpots;
                case MallB: return MallBSpots;
                case HomeAfter: return HomeAfterSpots;
                case EvacuationArea: return EvacuationAreaSpots;
                default: return System.Array.Empty<string>();
            }
        }

        public static bool HasActionSpot(string setting, string action) => ActionSpots(setting).Contains(action);

        static readonly string[] ClassroomSpots = { "door", "desk" };
        static readonly string[] CorridorSpots = { "cover", "plants" };
        static readonly string[] ClassroomAfterSpots = { "stay", "walk" };
        static readonly string[] BedroomSpots = { "picture", "low_shelf", "books_top" };

        static readonly string[] HomeSpots =
        {
            "hidden", "go_bag", "toys",
            "glass", "sofa", "table",
            "own_way", "meeting_place", "meet_home", "phone_numbers",
        };

        static readonly string[] SchoolyardSpots =
        {
            "tree", "field", "building",
            "route_post", "route_tree", "route_field", "route_later",
        };

        static readonly string[] BeachSpots = { "watch", "high_ground", "wait" };

        static readonly string[] MallASpots = { "run_exit", "elevator", "shop_window", "table" };

        static readonly string[] MallBSpots = { "stairs", "elevator", "run_push", "mall_wall" };

        static readonly string[] HomeAfterSpots =
        {
            "pull_up", "call_adult", "wait_for_mom", "leave",
            "no_go_bag", "toys", "push_past_mom", "go_bag",
        };

        static readonly string[] EvacuationAreaSpots = { "go_home", "radio", "rumor", "phone_all_day" };

        public static bool CanShake(string key) =>
            key == Classroom || key == Corridor || key == Home || key == Schoolyard || key == MallA;
    }
}
