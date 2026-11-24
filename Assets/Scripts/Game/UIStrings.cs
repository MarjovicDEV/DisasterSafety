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
                "Tip: Earthquake? DUCK, COVER, and HOLD!"),
            new TutorialPageText("stars", "Earn Stars",
                "Pick the safe choice first, with no Hint, for 3 stars!",
                "Tip: Play again anytime to earn more stars."),
        };

        public const string ExitTitle = "Leave the game?";
        public const string ExitBody = "Your stars are saved.";

        public const string NameTitle = "What is your name?";
        public const string NamePlaceholder = "Type here";
        public const string NameEmpty = "Please type your name.";
        public const string NameTooLong = "Use 12 letters or less.";
        public const string NameInvalid = "Use letters and numbers only.";

        public const string StudentTitle = "Who are you?";
        public const string StudentBoy = "Boy";
        public const string StudentGirl = "Girl";

        public const string EpisodeTitle = "Choose a Topic";

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
        public const string BantayPickEpisode = "Start with Earthquake!";
        public const string BantayPickDifficulty = "Easy is a good start!";
        public const string BantayLevelSoon = "This level is coming soon!";
        public const string BantayContinueLocked = "Finish a level first. Then you can continue!";
        public static string BantayEpisodeLocked(Episode needed) => "Finish " + EpisodeName(needed) + " first!";
        public static string BantayDifficultyLocked(Difficulty needed) => "Finish " + DifficultyName(needed) + " first!";
        public const string BantaySaveKept = "Your old save had a problem. I kept a copy safe.";

        /// <summary>
        /// Short safety facts Bantay says when tapped on the Main Menu. Only lines that trace
        /// to a cited finding in docs/research appear here (EQ-09, EQ-37, EQ-35). Tips for the
        /// other Episodes return when each Episode has been researched.
        /// </summary>
        public static readonly string[] Tips =
        {
            "Earthquake? DUCK, COVER, and HOLD!",
            "Know your school's evacuation area.",
            "Listen to your teacher during drills.",
        };

        // Level screen ----------------------------------------------------------
        public const string Hint = "HINT";
        public const string Pause = "PAUSE";
        public const string PauseTitle = "Paused";
        public const string Resume = "RESUME";
        public const string Restart = "RESTART";
        public const string QuitToMenu = "QUIT TO MENU";
        public const string PauseBody = "Stars you earned are kept.";

        public const string SafeHeading = "Safe!";
        public const string UnsafeHeading = "Not safe";
        public const string TryAgain = "TRY AGAIN";
        public const string Finish = "FINISH";
        public const string Tried = "Tried";

        public const string LevelCompletedTitle = "Level Completed!";
        public static string LevelCompletedHeadline(string name) =>
            string.IsNullOrEmpty(name) ? "You did it!" : "You did it, " + name + "!";
        public static string SaveProblem(string name) => LevelCompletedHeadline(name) + " I could not save this time.";
        public static string SafeChoiceLine(string label) => "Safe choice: " + label;
        public static string Opened(string what) => what + " is open!";
        public static string BantayOpenedSoon(string what) => what + " is open! New levels are coming soon.";
        public static string BantayOpened(string what) => what + " is open! Let's play!";
        public const string Replay = "REPLAY";
        public const string NextLevel = "NEXT";
        public const string LevelsButton = "LEVELS";
        public const string MainMenuButton = "MENU";

        public static string LevelLabel(LevelId id) =>
            EpisodeName(id.Episode) + " - " + DifficultyName(id.Difficulty) + " - " + id.Number;

        public static string HintLine(string hint) => "Hint: " + hint;
        public const string BantayThinkAgain = "Let's think again together!";
        public const string BantayHintReady = "Need help? Tap HINT!";

        // The Student's Reactions in a Level. They never judge a Choice and never mention Stars.
        public const string ReactionWhoa = "Whoa!";
        public const string ReactionWait = "Hmm, wait...";
        public const string ReactionSafe = "I'm safe!";
        /// <summary>The Safe Reaction in a "before" Level, where the Student gets ready rather than takes cover.</summary>
        public const string ReactionReady = "All ready!";
        public const string ActOutStayStill = "Stay still, Mom.";
        public const string ActOutHelp = "Help!";
        public static string BantayCheer(string name) =>
            string.IsNullOrEmpty(name) ? "Woof woof! Great job!" : "Woof woof! Great job, " + name + "!";
    }
}
