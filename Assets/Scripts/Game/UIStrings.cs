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
        public const string MusicCredits = "Music Credits";
        public const string MusicCreator = "Kevin MacLeod (incompetech.com)";
        public const string MusicLicence = "Licensed under Creative Commons: By Attribution 4.0";
        public const string MusicLicenceLink = "https://creativecommons.org/licenses/by/4.0/";
        public const string MusicFormatCredit = "All tracks were loudness matched and converted to WAV.";
        public const string MusicTrimCredit = "Cherry Blossom, Prelude and Action, Machinations and Piano Between were trimmed.";
        public const string MusicFadeCredit = "Loops have short edge fades. Piano Between fades at the end.";
        public const string MusicSourceLink = "https://incompetech.com/music/royalty-free/index.html?isrc=";

        public static readonly (MusicTrack Track, string Title, string Isrc)[] MusicRecordings =
        {
            (MusicTrack.SomewhereSunny, "Somewhere Sunny", "USUAN1100493"),
            (MusicTrack.EasyLemon, "Easy Lemon (60 second)", "USUAN1200077"),
            (MusicTrack.CherryBlossom, "Cherry Blossom", "USUAN1100382"),
            (MusicTrack.HappyAlley, "Happy Alley", "USUAN1100482"),
            (MusicTrack.PreludeAndAction, "Prelude and Action", "USUAN1100887"),
            (MusicTrack.CheeryMonday, "Cheery Monday", "USUAN1700065"),
            (MusicTrack.BRoll, "B-Roll", "USUAN1100315"),
            (MusicTrack.Machinations, "Machinations", "USUAN1100510"),
            (MusicTrack.PianoBetween, "Piano Between", "USUAN1100490")
        };

        /// <summary>The CC0 recordings the game plays as music besides the Kevin MacLeod tracks.</summary>
        public static readonly (MusicTrack Track, string Title)[] OtherMusicRecordings =
        {
            (MusicTrack.UkuleleShortTheme, "Ukulele Short Theme")
        };

        public const string SoundCredit = "Button sounds: Kenney (kenney.nl), CC0.";
        public const string CompletedCredit = "Level Completed: ukulele by Tetoszka (freesound.org), CC0.";

        public const string Done = "DONE";
        public static string TutorialPageTitle(int index, string title) => "Step " + (index + 1) + ": " + title;

        /// <summary>
        /// How to Play Tutorial Pages, in order. Each page has a matching picture
        /// (Assets/Art/Tutorial/tutorial_&lt;Art&gt;_0/1.png). The pilot pages (topic, difficulty, stars) are
        /// drawn by ArtSource/scripts/tutorial.lua; the v2 pages that show the Action Bar flow are drawn by
        /// ArtSource/scripts/tutorial_v2.lua. Captions only explain how to play; the Tips on pages 3 and 4
        /// are existing Tips (the Duck, Cover, and Hold Tip traces to EQ-09).
        /// </summary>
        public static readonly TutorialPageText[] TutorialPages =
        {
            new TutorialPageText("topic", "Pick a Topic",
                "Tap a topic card. Start with Earthquake!",
                "Tip: Finish a topic to open the next one."),
            new TutorialPageText("difficulty", "Pick a Level",
                "Tap Easy first. Normal and Hard open later.",
                "Tip: More hard hats means a harder challenge."),
            new TutorialPageText("v2_actionbar", "Pick an Action",
                "Look at the room. Then tap a button on the Action Bar.",
                "Tip: Take your time. Read slowly."),
            new TutorialPageText("v2_actout", "Watch Your Student",
                "Your Student acts out the Choice you picked.",
                "Tip: Earthquake? DUCK, COVER, and HOLD!"),
            new TutorialPageText("v2_feedback", "Read the Feedback",
                "Find out if your Choice was safe, and why.",
                "Tip: Not safe? Tap " + TryAgain + " and pick again."),
            new TutorialPageText("v2_hint", "Use the Hint",
                "Stuck? Tap " + Hint + ". The Hint shows at the top.",
                "Tip: The Hint helps you find the safe Choice."),
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

        // The Student's menu lines ------------------------------------------
        public const string StudentHello = "Hi! Tap PLAY to start!";
        public static string StudentWelcomeBack(string name) => "Welcome back, " + name + "!";
        public static string StudentNiceToMeet(string name) => "Nice to meet you, " + name + "!";
        public const string StudentPickEpisode = "Start with Earthquake!";
        public const string StudentPickDifficulty = "Easy is a good start!";
        public const string StudentLevelSoon = "This level is coming soon!";
        public const string StudentContinueLocked = "Finish a level first. Then you can continue!";
        public static string StudentEpisodeLocked(Episode needed) => "Finish " + EpisodeName(needed) + " first!";
        public static string StudentDifficultyLocked(Difficulty needed) => "Finish " + DifficultyName(needed) + " first!";
        public const string StudentSaveKept = "Your old save had a problem. I kept a copy safe.";

        /// <summary>
        /// Short safety facts the Student says when tapped on the Main Menu. Only lines that trace
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
        public const string PauseMainMenu = "MAIN MENU";
        public static string PauseLevel(LevelId id) =>
            EpisodeName(id.Episode) + " - " + DifficultyName(id.Difficulty) + " " + id.Number;

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
        public static string StudentOpenedSoon(string what) => what + " is open! New levels are coming soon.";
        public static string StudentOpened(string what) => what + " is open! Let's play!";
        public const string Replay = "REPLAY";
        public const string NextLevel = "NEXT";
        public const string NextEpisode = "Next Episode";
        public const string LevelsButton = "LEVELS";
        public const string MainMenuButton = "MENU";

        public static string LevelLabel(LevelId id) =>
            EpisodeName(id.Episode) + " - " + DifficultyName(id.Difficulty) + " - " + id.Number;

        public static string HintLine(string hint) => "Hint: " + hint;

        // The Student's Reactions in a Level. They never judge a Choice and never mention Stars.
        public const string ReactionWhoa = "Whoa!";
        public const string ReactionWait = "Hmm, wait...";
        public const string ReactionSafe = "I'm safe!";
        /// <summary>The Safe Reaction in a "before" Level, where the Student gets ready rather than takes cover.</summary>
        public const string ReactionReady = "All ready!";
        public const string ActOutStayStill = "Stay still, Mom.";
        public const string ActOutHelp = "Help!";
        /// <summary>The Act-out line of Hard 7's rumor Choice, said to the neighbours (spec item 7).</summary>
        public const string ActOutDidYouHear = "Did you hear...?";
    }
}
