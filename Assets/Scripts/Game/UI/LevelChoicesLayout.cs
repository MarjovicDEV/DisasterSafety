using UnityEngine;

namespace DisasterSafety.Game.UI
{
    /// <summary>Makes room for Setting art by shortening the Choice row on wider screens.</summary>
    public sealed class LevelChoicesLayout : MonoBehaviour
    {
        const float ReferenceAspect = 16f / 9f;
        const float WideAspect = 20f / 9f;

        public RectTransform row;
        public ChoiceCard[] cards;
        public float compactHeight;
        public float compactBottomInset;
        public float compactPictureScale;
        public float compactPictureTopInset;

        bool compactOnWideScreens;
        float appliedAmount = float.NaN;
        float regularHeight;
        float regularBottomInset;
        Vector2 regularPictureSize;
        float regularPictureY;

        public void UseCompactLayoutOnWideScreens(bool enabled)
        {
            compactOnWideScreens = enabled;
            ApplyScreenAspect();
        }

        void OnEnable() => ApplyScreenAspect();

        void OnRectTransformDimensionsChange()
        {
            if (isActiveAndEnabled) ApplyScreenAspect();
        }

        void ApplyScreenAspect() => Apply((float)Screen.width / Screen.height);

        public void Apply(float aspect)
        {
            if (row == null || cards == null || cards.Length == 0) return;
            var amount = compactOnWideScreens ? Mathf.InverseLerp(ReferenceAspect, WideAspect, aspect) : 0f;
            if (amount == appliedAmount) return;

            if (float.IsNaN(appliedAmount))
            {
                regularHeight = row.sizeDelta.y;
                regularBottomInset = row.anchoredPosition.y;
                var picture = cards[0].picture.rectTransform;
                regularPictureSize = picture.sizeDelta;
                regularPictureY = picture.anchoredPosition.y;
            }
            appliedAmount = amount;

            var height = Mathf.Lerp(regularHeight, compactHeight, amount);
            row.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            row.anchoredPosition = new Vector2(row.anchoredPosition.x, Mathf.Lerp(regularBottomInset, compactBottomInset, amount));
            var pictureSize = Vector2.Lerp(regularPictureSize, regularPictureSize * compactPictureScale, amount);
            var pictureY = Mathf.Lerp(regularPictureY, -compactPictureTopInset, amount);
            foreach (var card in cards)
            {
                var rect = (RectTransform)card.transform;
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                var picture = card.picture.rectTransform;
                picture.sizeDelta = pictureSize;
                picture.anchoredPosition = new Vector2(picture.anchoredPosition.x, pictureY);
            }
        }
    }
}
