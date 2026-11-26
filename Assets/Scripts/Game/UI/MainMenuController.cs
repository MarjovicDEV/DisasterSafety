using System;
using System.Collections;
using System.IO;
using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// Connects the Main Menu scene to MenuNavigator (Figures 1 and 2 of Chapter III).
    /// All decisions live in Core; this class only shows screens and forwards taps.
    /// </summary>
    public sealed class MainMenuController : MonoBehaviour, IMenuState
    {
        [Header("Screens")]
        public ScreenPanel mainPanel;
        public ScreenPanel settingsPanel;
        public ScreenPanel musicCreditsPanel;
        public ScreenPanel howToPlayPanel;
        public ScreenPanel exitPanel;
        public ScreenPanel namePanel;
        public ScreenPanel studentPanel;
        public ScreenPanel episodePanel;
        public ScreenPanel difficultyPanel;
        public ScreenPanel levelPanel;
        public ScreenPanel dimmer;

        [Header("Main menu")]
        public Button playButton;
        public Button continueButton;
        public Button settingsButton;
        public Button howToPlayButton;
        public Button exitButton;

        [Header("Settings")]
        public SwitchButton musicSwitch;
        public SwitchButton soundSwitch;
        public Button settingsBack;
        public Button musicCreditsButton;
        public Button musicCreditsBack;

        [Header("How to Play")]
        public Button howToPlayBack;
        public TutorialView tutorial;

        [Header("Exit")]
        public Button exitYes;
        public Button exitNo;

        [Header("Name entry")]
        public InputField nameInput;
        public Text nameError;
        public Button nameOk;
        public Button nameBack;

        [Header("Student pick")]
        public Button studentBoy;
        public Button studentGirl;

        [Header("Episode Selection")]
        public EpisodeCard[] episodeCards;
        public Button episodeBack;

        [Header("Difficulty Selection")]
        public DifficultyCard[] difficultyCards;
        public Button difficultyBack;
        public Text difficultyHeader;
        public Image difficultyEmblem;
        [Tooltip("Emblems in Episode order.")]
        public Sprite[] episodeEmblems;

        [Header("Level Selection")]
        public LevelSlot[] levelSlots;
        public Button levelBack;
        public Button startButton;
        public Text levelHeader;

        [Header("Guide")]
        public MenuStudent student;

        [Header("Backgrounds")]
        public MenuWorld world;

        MenuNavigator nav;
        int selectedLevel;

        // IMenuState ----------------------------------------------------------
        public bool HasDisplayName => GameServices.Progress.HasDisplayName;
        public bool HasStudent => GameServices.Progress.HasStudent;
        public bool CanContinue => GameServices.CanContinue;
        public (Episode, Difficulty) ContinueDestination => ContinueRule.Destination(GameServices.Progress);
        public bool IsOpen(Episode episode) => GameServices.Progress.IsOpen(episode);
        public bool IsOpen(Episode episode, Difficulty difficulty) => GameServices.Progress.IsOpen(episode, difficulty);

        void Awake()
        {
            GameServices.Initialize();
            AudioManager.EnsureExists();
            nav = new MenuNavigator(this);
            nav.Changed += OnScreenChanged;
            Wire();
        }

        IEnumerator Start()
        {
            foreach (var p in AllPanels()) p.SetInstant(false);
            mainPanel.SetInstant(true);
            student.SetKind(GameServices.Progress.Student);
            world.Show(nav.Background);
            RefreshMain();

            var entry = GameServices.MenuDestination;
            GameServices.MenuDestination = null;
            if (entry.HasValue)
            {
                var (episode, difficulty) = entry.Value;
                var opened = difficulty.HasValue
                    ? nav.OpenLevelSelect(episode, difficulty.Value)
                    : nav.OpenEpisodeSelect(episode);
                if (opened) yield break;
            }

            // A save with a Display Name but no Student shows the picker once.
            if (nav.OfferStudentPick()) yield break;

            yield return new WaitForSeconds(0.8f);
            if (GameServices.StartupLoadStatus == SaveLoadStatus.Preserved)
                student.Greet(UIStrings.StudentSaveKept);
            else if (HasDisplayName)
                student.Greet(UIStrings.StudentWelcomeBack(GameServices.Progress.DisplayName));
            else
                student.Greet(UIStrings.StudentHello);
        }

        void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            {
                AudioManager.Play(Sfx.Back);
                // Inside How to Play, Back turns to the previous Tutorial Page first.
                if (nav.Current == MenuScreen.HowToPlay && tutorial != null && tutorial.Back()) return;
                nav.Back();
            }
        }

        ScreenPanel[] AllPanels() => new[]
        {
            mainPanel, settingsPanel, musicCreditsPanel, howToPlayPanel, exitPanel, namePanel, studentPanel,
            episodePanel, difficultyPanel, levelPanel, dimmer
        };

        void Wire()
        {
            playButton.onClick.AddListener(() =>
            {
                Juice.Sparkles((RectTransform)playButton.transform, 12);
                AudioManager.Play(Sfx.Sparkle, 0.6f);
                nav.Play();
            });
            continueButton.onClick.AddListener(() => nav.Continue());
            var relay = continueButton.GetComponent<DisabledClickRelay>();
            if (relay != null)
            {
                relay.onDisabledClick.AddListener(() =>
                {
                    AudioManager.Play(Sfx.Locked);
                    Juice.Shake((RectTransform)continueButton.transform);
                    student.Say(UIStrings.StudentContinueLocked);
                });
            }
            settingsButton.onClick.AddListener(nav.OpenSettings);
            howToPlayButton.onClick.AddListener(nav.OpenHowToPlay);
            exitButton.onClick.AddListener(nav.RequestExit);

            settingsBack.onClick.AddListener(nav.Back);
            musicCreditsButton.onClick.AddListener(nav.OpenMusicCredits);
            musicCreditsBack.onClick.AddListener(nav.Back);
            musicSwitch.button.onClick.AddListener(() =>
            {
                GameServices.Audio.MusicOn = !GameServices.Audio.MusicOn;
                musicSwitch.Show(GameServices.Audio.MusicOn);
            });
            soundSwitch.button.onClick.AddListener(() =>
            {
                GameServices.Audio.SoundOn = !GameServices.Audio.SoundOn;
                soundSwitch.Show(GameServices.Audio.SoundOn);
            });

            howToPlayBack.onClick.AddListener(nav.Back);

            exitYes.onClick.AddListener(() =>
            {
                if (nav.ConfirmExit()) Quit();
            });
            exitNo.onClick.AddListener(nav.CancelExit);

            nameInput.characterLimit = DisplayNameRule.MaxLength + 4; // room for spaces that get collapsed
            nameInput.onValidateInput = (text, index, c) => DisplayNameRule.IsAllowed(c) ? c : '\0';
            nameInput.onValueChanged.AddListener(_ =>
            {
                AudioManager.Play(Sfx.Type, 0.5f);
                nameError.text = string.Empty;
            });
            nameOk.onClick.AddListener(SubmitName);
            nameBack.onClick.AddListener(nav.Back);

            studentBoy.onClick.AddListener(() => PickStudent(StudentKind.Boy));
            studentGirl.onClick.AddListener(() => PickStudent(StudentKind.Girl));

            foreach (var card in episodeCards)
            {
                var c = card;
                c.button.onClick.AddListener(() => OnEpisode(c));
            }
            episodeBack.onClick.AddListener(nav.Back);

            foreach (var card in difficultyCards)
            {
                var c = card;
                c.button.onClick.AddListener(() => OnDifficulty(c));
            }
            difficultyBack.onClick.AddListener(nav.Back);

            for (var i = 0; i < levelSlots.Length; i++)
            {
                var n = i + 1;
                levelSlots[i].button.onClick.AddListener(() => OnLevel(n));
            }
            levelBack.onClick.AddListener(nav.Back);
            startButton.onClick.AddListener(OnStart);
        }

        // Screen changes ---------------------------------------------------------
        static bool IsDialog(MenuScreen s) =>
            s == MenuScreen.Settings || s == MenuScreen.HowToPlay || s == MenuScreen.ExitConfirm || s == MenuScreen.NameEntry
            || s == MenuScreen.StudentPick || s == MenuScreen.MusicCredits;

        ScreenPanel PanelFor(MenuScreen s)
        {
            switch (s)
            {
                case MenuScreen.Main: return mainPanel;
                case MenuScreen.Settings: return settingsPanel;
                case MenuScreen.MusicCredits: return musicCreditsPanel;
                case MenuScreen.HowToPlay: return howToPlayPanel;
                case MenuScreen.ExitConfirm: return exitPanel;
                case MenuScreen.NameEntry: return namePanel;
                case MenuScreen.StudentPick: return studentPanel;
                case MenuScreen.EpisodeSelect: return episodePanel;
                case MenuScreen.DifficultySelect: return difficultyPanel;
                case MenuScreen.LevelSelect: return levelPanel;
                default: throw new ArgumentOutOfRangeException(nameof(s));
            }
        }

        void OnScreenChanged(MenuScreen from, MenuScreen to)
        {
            if (from != to && !(from == MenuScreen.Main && IsDialog(to))) PanelFor(from).Hide();

            if (IsDialog(to))
            {
                dimmer.Show();
                if (mainPanel.IsVisible) mainPanel.SetInteractable(false);
            }
            else
            {
                dimmer.Hide();
            }

            if (to == MenuScreen.Main)
            {
                mainPanel.Show();
                mainPanel.SetInteractable(true);
            }
            else if (!IsDialog(to))
            {
                mainPanel.Hide();
            }

            PanelFor(to).Show();
            world.Show(nav.Background);
            OnEnter(to);
        }

        void OnEnter(MenuScreen screen)
        {
            switch (screen)
            {
                case MenuScreen.Main:
                    RefreshMain();
                    break;
                case MenuScreen.Settings:
                    musicSwitch.Show(GameServices.Audio.MusicOn);
                    soundSwitch.Show(GameServices.Audio.SoundOn);
                    break;
                case MenuScreen.HowToPlay:
                    if (tutorial != null) tutorial.Open();
                    break;
                case MenuScreen.NameEntry:
                    nameInput.text = string.Empty;
                    nameError.text = string.Empty;
                    nameInput.ActivateInputField();
                    break;
                case MenuScreen.EpisodeSelect:
                    RefreshEpisodes();
                    if (!GameServices.Progress.HasSavedProgress) student.Say(UIStrings.StudentPickEpisode);
                    break;
                case MenuScreen.DifficultySelect:
                    RefreshDifficulties();
                    if (!GameServices.Progress.HasSavedProgress) student.Say(UIStrings.StudentPickDifficulty);
                    break;
                case MenuScreen.LevelSelect:
                    selectedLevel = 0;
                    RefreshLevels();
                    break;
            }
        }

        /// <summary>The Continue check from Figure 1, repeated every time the Main Menu shows.</summary>
        void RefreshMain()
        {
            continueButton.interactable = CanContinue;
        }

        void RefreshEpisodes()
        {
            foreach (var card in episodeCards)
            {
                var open = IsOpen(card.episode);
                card.Show(open);
                SetSound(card.button, open ? Sfx.Click : Sfx.Locked);
            }
        }

        void RefreshDifficulties()
        {
            var e = nav.SelectedEpisode;
            difficultyHeader.text = UIStrings.EpisodeName(e);
            if (episodeEmblems != null && (int)e < episodeEmblems.Length) difficultyEmblem.sprite = episodeEmblems[(int)e];
            foreach (var card in difficultyCards)
            {
                var open = IsOpen(e, card.difficulty);
                card.Show(open);
                SetSound(card.button, open ? Sfx.Click : Sfx.Locked);
            }
        }

        void RefreshLevels()
        {
            var e = nav.SelectedEpisode;
            var d = nav.SelectedDifficulty;
            levelHeader.text = UIStrings.EpisodeName(e) + " - " + UIStrings.DifficultyName(d);
            var count = Curriculum.LevelCount(d);
            for (var i = 0; i < levelSlots.Length; i++)
            {
                var slot = levelSlots[i];
                var used = i < count;
                slot.gameObject.SetActive(used);
                if (!used) continue;
                var id = new LevelId(e, d, i + 1);
                slot.Show(i + 1, GameServices.Progress.StarsFor(id), selectedLevel == i + 1);
            }
            startButton.interactable = selectedLevel > 0 && GameServices.Content.IsPlayable(new LevelId(e, d, selectedLevel));
        }

        static void SetSound(Button button, Sfx sfx)
        {
            var bounce = button.GetComponent<PressBounce>();
            if (bounce != null) bounce.sound = sfx;
        }

        // Taps -----------------------------------------------------------------
        void SubmitName()
        {
            var error = DisplayNameRule.Check(nameInput.text, out var name);
            if (error != DisplayNameError.None)
            {
                nameError.text = error == DisplayNameError.TooLong ? UIStrings.NameTooLong
                    : error == DisplayNameError.InvalidCharacter ? UIStrings.NameInvalid
                    : UIStrings.NameEmpty;
                AudioManager.Play(Sfx.Locked);
                Juice.Shake((RectTransform)nameInput.transform);
                return;
            }

            GameServices.Progress.SetDisplayName(name);
            SaveQuietly("the Display Name");

            AudioManager.Play(Sfx.Confirm);
            Juice.Sparkles((RectTransform)nameOk.transform, 10);
            nav.NameSaved();
            student.Say(UIStrings.StudentNiceToMeet(name));
        }

        void PickStudent(StudentKind kind)
        {
            GameServices.Progress.SetStudent(kind);
            SaveQuietly("the Student");
            student.SetKind(kind);
            AudioManager.Play(Sfx.Confirm);
            var button = kind == StudentKind.Boy ? studentBoy : studentGirl;
            Juice.Sparkles((RectTransform)button.transform, 10);
            nav.StudentPicked();
        }

        static void SaveQuietly(string what)
        {
            try
            {
                GameServices.SaveProgress();
            }
            catch (IOException ex)
            {
                Debug.LogError("Could not save " + what + ": " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.LogError("Could not save " + what + ": " + ex.Message);
            }
        }

        void OnEpisode(EpisodeCard card)
        {
            if (nav.SelectEpisode(card.episode)) return;
            Juice.Shake((RectTransform)card.transform);
            var index = Mathf.Max(0, (int)card.episode - 1);
            student.Say(UIStrings.StudentEpisodeLocked(Curriculum.Episodes[index]));
        }

        void OnDifficulty(DifficultyCard card)
        {
            if (nav.SelectDifficulty(card.difficulty)) return;
            Juice.Shake((RectTransform)card.transform);
            var index = Mathf.Max(0, (int)card.difficulty - 1);
            student.Say(UIStrings.StudentDifficultyLocked(Curriculum.Difficulties[index]));
        }

        void OnLevel(int number)
        {
            selectedLevel = number;
            RefreshLevels();
            var id = new LevelId(nav.SelectedEpisode, nav.SelectedDifficulty, number);
            if (!GameServices.Content.IsPlayable(id))
            {
                AudioManager.Play(Sfx.Soon);
                student.Say(UIStrings.StudentLevelSoon);
            }
        }

        void OnStart()
        {
            if (selectedLevel <= 0) return;
            var id = new LevelId(nav.SelectedEpisode, nav.SelectedDifficulty, selectedLevel);
            if (!GameServices.Content.IsPlayable(id) || !IsOpen(id.Episode, id.Difficulty)) return;
            GameScenes.PlayLevel(id);
        }

        static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
