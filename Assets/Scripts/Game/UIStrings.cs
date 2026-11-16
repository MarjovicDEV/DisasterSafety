using DisasterSafety.Core;

namespace DisasterSafety.Game
{
    /// <summary>
    /// Every sentence the Learner reads, in one place, so a Filipino version can be added
    /// without touching screens. Keep sentences short: the readers are Grade 3 to Grade 6.
    /// Only use characters the pixel font has (A-Z, a-z, ñ, Ñ, 0-9 and basic punctuation).
    /// </summary>
    public static class UIStrings
    {
        public const string Play = "PLAY";
        public const string Continue = "CONTINUE";
        public const string Settings = "SETTINGS";
        public const string HowToPlay = "HOW TO PLAY";
        public const string Back = "BACK";
        public const string Start = "START";
        public const string Ok = "OK";
        public const string Yes = "YES";
        public const string No = "NO";

        public const string SettingsTitle = "Settings";
        public const string Music = "Music";
        public const string Sounds = "Sounds";

        public const string Done = "DONE";
        public static string TutorialPageTitle(int index, string title) => "Step " + (index + 1) + ": " + title;

        /// <summary>
        /// How to Play Tutorial Pages, in order. Each page has a matching picture
        /// (Assets/Art/Tutorial/tutorial_&lt;Art&gt;_0/1.png) drawn by ArtSource/scripts/tutorial.lua.
        /// </summary>
        public static readonly TutorialPageText[] TutorialPages =
        {
            new TutorialPageText("topic", "Pick a Topic",
                "Tap a topic card. Start with Earthquake!",
                "Tip: Finish a topic to open the next one."),
            new TutorialPageText("difficulty", "Pick a Level",
                "Tap Easy first. Normal and Hard open later.",
                "Tip: More hard hats means a harder challenge."),
            new TutorialPageText("story", "Read the Story",
                "Something is happening! Read and look at the picture.",
                "Tip: Take your time. Read slowly."),
            new TutorialPageText("choice", "Tap the Safest Choice",
                "Pick the thing that keeps you safe.",
                "Tip: Earthquake? DROP, COVER, and HOLD ON!"),
            new TutorialPageText("stars", "Earn Stars",
                "Safe choices earn stars. Try to get all 3!",
                "Tip: Play again anytime to earn more stars."),
        };

        public const string ExitTitle = "Leave the game?";
        public const string ExitBody = "Your stars are saved.";

        public const string NameTitle = "What is your name?";
        public const string NamePlaceholder = "Type here";
        public const string NameEmpty = "Please type your name.";
        public const string NameTooLong = "Use 12 letters or less.";
        public const string NameInvalid = "Use letters and numbers only.";

        public const string EpisodeTitle = "Choose a Topic";
        public const string DifficultyTitle = "Choose a Level";
        public const string ComingSoon = "Coming soon!";

        public static string Levels(int count) => count + " levels";

        public static string EpisodeName(Episode e)
        {
            switch (e)
            {
                case Episode.Earthquake: return "Earthquake";
                case Episode.Fire: return "Fire";
                case Episode.Typhoon: return "Typhoon";
                case Episode.Flood: return "Flood";
                case Episode.Landslide: return "Landslide";
                default: return e.ToString();
            }
        }

        public static string DifficultyName(Difficulty d)
        {
            switch (d)
            {
                case Difficulty.Easy: return "Easy";
                case Difficulty.Normal: return "Normal";
                case Difficulty.Hard: return "Hard";
                default: return d.ToString();
            }
        }

        // Bantay's lines -----------------------------------------------------
        public const string BantayHello = "Hi! I am Bantay. Tap PLAY to start!";
        public static string BantayWelcomeBack(string name) => "Welcome back, " + name + "!";
        public static string BantayNiceToMeet(string name) => "Nice to meet you, " + name + "!";
        public const string BantayAskName = "Let me know your name!";
        public const string BantayPickEpisode = "Start with Earthquake!";
        public const string BantayPickDifficulty = "Easy is a good start!";
        public const string BantayLevelSoon = "This level is coming soon!";
        public const string BantayContinueLocked = "Finish a level first. Then you can continue!";
        public static string BantayEpisodeLocked(Episode needed) => "Finish " + EpisodeName(needed) + " first!";
        public static string BantayDifficultyLocked(Difficulty needed) => "Finish " + DifficultyName(needed) + " first!";
        public const string BantaySaveKept = "Your old save had a problem. I kept a copy safe.";

        /// <summary>Short, accurate safety tips Bantay says when tapped.</summary>
        public static readonly string[] Tips =
        {
            "Earthquake? DROP, COVER, and HOLD ON!",
            "Fire? Get out and stay out!",
            "Smoke? Stay low and go!",
            "Typhoon? Stay inside, away from windows.",
            "Flood? Never walk or play in floodwater.",
            "Heavy rain on a hill? Watch for landslides!",
            "Know your school's evacuation area.",
            "Listen to your teacher during drills.",
            "Keep a go bag with water and a flashlight.",
        };
    }
}
