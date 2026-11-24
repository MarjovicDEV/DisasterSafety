using System.Collections;
using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// The picture tutorial inside How to Play: one Tutorial Page at a time with an animated
    /// picture, a caption and a Tip from the Learner's Student. Arrows and swipes change pages; star dots show
    /// which page is open.
    /// </summary>
    public sealed class TutorialView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Text title;
        public Text caption;
        public Text tip;
        public Image picture;
        public RectTransform pictureHolder;
        public Button previous;
        public Button next;
        public Text closeLabel;
        public Image[] dots;
        public Sprite dotOn;
        public Sprite dotOff;
        [Tooltip("Two frames per page, in page order.")]
        public Sprite[] frames;
        [Tooltip("The Learner's Student beside the Tip. It shows the picked kind, and the boy until a pick exists.")]
        public StudentActor student;

        const float SwipeDistance = 120f;

        TutorialPager pager;
        float dragX;
        float frameClock;
        Coroutine slide;

        public int PageIndex => Pager.Index;

        TutorialPager Pager
        {
            get
            {
                if (pager == null)
                {
                    pager = new TutorialPager(UIStrings.TutorialPages.Length);
                    pager.Changed += OnPageChanged;
                }
                return pager;
            }
        }

        void Awake()
        {
            previous.onClick.AddListener(() => Pager.Previous());
            next.onClick.AddListener(NextPage);
        }

        public void NextPage() => Pager.Next();

        /// <summary>Always starts at the first page.</summary>
        public void Open()
        {
            if (student != null)
            {
                student.SetKind(GameServices.Ready ? GameServices.Progress.Student : StudentKind.None);
                student.Play(StudentPose.Idle);
            }
            Pager.Restart();
            Render();
        }

        /// <summary>Android Back inside the tutorial: previous page, or false on the first page.</summary>
        public bool Back() => Pager.Previous();

        void OnPageChanged(int index)
        {
            Render();
            if (slide != null) StopCoroutine(slide);
            if (Application.isPlaying && isActiveAndEnabled) slide = StartCoroutine(SlideIn());
        }

        void Render()
        {
            var i = Pager.Index;
            var page = UIStrings.TutorialPages[i];
            title.text = UIStrings.TutorialPageTitle(i, page.Title);
            caption.text = page.Caption;
            tip.text = page.Tip;
            previous.interactable = !Pager.IsFirst;
            next.interactable = !Pager.IsLast;
            if (closeLabel != null) closeLabel.text = Pager.IsLast ? UIStrings.Done : UIStrings.Back;
            for (var d = 0; d < dots.Length; d++) dots[d].sprite = d == i ? dotOn : dotOff;
            frameClock = 0f;
            ShowFrame(0);
        }

        void ShowFrame(int f)
        {
            var index = Pager.Index * 2 + f;
            if (frames != null && index < frames.Length) picture.sprite = frames[index];
        }

        void Update()
        {
            frameClock += Time.unscaledDeltaTime;
            ShowFrame(Mathf.FloorToInt(frameClock * 2.5f) % 2);
        }

        IEnumerator SlideIn()
        {
            for (var t = 0f; t < 0.25f; t += Time.unscaledDeltaTime)
            {
                var x = Mathf.LerpUnclamped(80f, 0f, Ease.OutBack(t / 0.25f));
                pictureHolder.anchoredPosition = new Vector2(Mathf.Round(x / 4f) * 4f, pictureHolder.anchoredPosition.y);
                yield return null;
            }
            pictureHolder.anchoredPosition = new Vector2(0f, pictureHolder.anchoredPosition.y);
            slide = null;
        }

        public void OnBeginDrag(PointerEventData eventData) => dragX = 0f;
        public void OnDrag(PointerEventData eventData) => dragX += eventData.delta.x;

        public void OnEndDrag(PointerEventData eventData)
        {
            var moved = dragX < -SwipeDistance ? Pager.Next() : dragX > SwipeDistance && Pager.Previous();
            if (moved) AudioManager.Play(Sfx.Click);
        }
    }
}
