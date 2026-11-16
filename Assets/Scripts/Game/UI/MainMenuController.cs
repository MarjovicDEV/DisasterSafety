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
        public ScreenPanel howToPlayPanel;
        public ScreenPanel exitPanel;
        public ScreenPanel namePanel;
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

        [Header("How to Play")]
        public Button howToPlayBack;

        [Header("Exit")]
        public Button exitYes;
        public Button exitNo;

        [Header("Name entry")]
        public InputField nameInput;
        public Text nameError;
        public Button nameOk;
        public Button nameBack;

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
        public BantayGuide bantay;

        MenuNavigator nav;
        int selectedLevel;

        // IMenuState ----------------------------------------------------------
        public bool HasDisplayName => GameServices.Progress.HasDisplayName;
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
            RefreshMain();

            yield return new WaitForSeconds(0.8f);
            if (GameServices.StartupLoadStatus == SaveLoadStatus.Preserved)
                bantay.Say(UIStrings.BantaySaveKept, true);
            else if (HasDisplayName)
                bantay.Say(UIStrings.BantayWelcomeBack(GameServices.Progress.DisplayName), true);
            else
                bantay.Say(UIStrings.BantayHello, true);
        }

        void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            {
                AudioManager.Play(Sfx.Back);
                nav.Back();
            }
        }

        ScreenPanel[] AllPanels() => new[]
        {
            mainPanel, settingsPanel, howToPlayPanel, exitPanel, namePanel, episodePanel, difficultyPanel, levelPanel, dimmer
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
                    bantay.Say(UIStrings.BantayContinueLocked);
                });
            }
            settingsButton.onClick.AddListener(nav.OpenSettings);
            howToPlayButton.onClick.AddListener(nav.OpenHowToPlay);
            exitButton.onClick.AddListener(nav.RequestExit);

            settingsBack.onClick.AddListener(nav.Back);
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
            s == MenuScreen.Settings || s == MenuScreen.HowToPlay || s == MenuScreen.ExitConfirm || s == MenuScreen.NameEntry;

        ScreenPanel PanelFor(MenuScreen s)
        {
            switch (s)
            {
                case MenuScreen.Main: return mainPanel;
                case MenuScreen.Settings: return settingsPanel;
                case MenuScreen.HowToPlay: return howToPlayPanel;
                case MenuScreen.ExitConfirm: return exitPanel;
                case MenuScreen.NameEntry: return namePanel;
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
                case MenuScreen.NameEntry:
                    nameInput.text = string.Empty;
                    nameError.text = string.Empty;
                    nameInput.ActivateInputField();
                    break;
                case MenuScreen.EpisodeSelect:
                    RefreshEpisodes();
                    if (!GameServices.Progress.HasSavedProgress) bantay.Say(UIStrings.BantayPickEpisode);
                    break;
                case MenuScreen.DifficultySelect:
                    RefreshDifficulties();
                    if (!GameServices.Progress.HasSavedProgress) bantay.Say(UIStrings.BantayPickDifficulty);
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
            try
            {
                GameServices.SaveProgress();
            }
            catch (IOException ex)
            {
                Debug.LogError("Could not save the Display Name: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.LogError("Could not save the Display Name: " + ex.Message);
            }

            AudioManager.Play(Sfx.Confirm);
            Juice.Sparkles((RectTransform)nameOk.transform, 10);
            nav.NameSaved();
            bantay.Say(UIStrings.BantayNiceToMeet(name), true);
        }

        void OnEpisode(EpisodeCard card)
        {
            if (nav.SelectEpisode(card.episode)) return;
            Juice.Shake((RectTransform)card.transform);
            var index = Mathf.Max(0, (int)card.episode - 1);
            bantay.Say(UIStrings.BantayEpisodeLocked(Curriculum.Episodes[index]));
        }

        void OnDifficulty(DifficultyCard card)
        {
            if (nav.SelectDifficulty(card.difficulty)) return;
            Juice.Shake((RectTransform)card.transform);
            var index = Mathf.Max(0, (int)card.difficulty - 1);
            bantay.Say(UIStrings.BantayDifficultyLocked(Curriculum.Difficulties[index]));
        }

        void OnLevel(int number)
        {
            selectedLevel = number;
            RefreshLevels();
            var id = new LevelId(nav.SelectedEpisode, nav.SelectedDifficulty, number);
            if (!GameServices.Content.IsPlayable(id))
            {
                AudioManager.Play(Sfx.Soon);
                bantay.Say(UIStrings.BantayLevelSoon);
            }
        }

        void OnStart()
        {
            // Scenario scenes arrive in the gameplay milestone; Start stays disabled until then.
            var id = new LevelId(nav.SelectedEpisode, nav.SelectedDifficulty, selectedLevel);
            Debug.Log("Start requested for " + id.Key);
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
