using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>Lets a disabled button still explain itself when tapped.</summary>
    public sealed class DisabledClickRelay : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent onDisabledClick = new UnityEvent();
        Selectable selectable;

        void Awake() => selectable = GetComponent<Selectable>();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (selectable != null && !selectable.IsInteractable()) onDisabledClick.Invoke();
        }
    }
}
