using NaughtyAttributes;
using UnityEngine;

namespace UI.Popups
{
    public sealed class PopupManager : MonoBehaviour
    {
        public static PopupManager Instance;

        public bool HasActivePopup => ActivePopup != null;

        [field: SerializeField, ReadOnly]
        public Popup ActivePopup { get; private set; }

        public void Show(Popup popup)
        {
            if (ActivePopup == popup)
                return;

            ActivePopup?.Hide();
            ActivePopup = popup;
        }

        public void Hide(Popup popup)
        {
            if (ActivePopup == popup)
            {
                ActivePopup = null;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}