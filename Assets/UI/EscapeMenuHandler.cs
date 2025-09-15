using UI.Popups;
using UnityEngine;

namespace UI
{
    public class EscapeMenuHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject escapeMenuRoot;

        private bool IsActive => escapeMenuRoot.activeSelf;

        private void Start()
        {
            escapeMenuRoot.SetActive(false);
        }

        private void OnCancel()
        {
            // TODO: this is a bit hacky
            if (PopupManager.Instance.HasActivePopup)
            {
                PopupManager.Instance.ActivePopup.Hide();
                return;
            }

            escapeMenuRoot.SetActive(!IsActive);
        }
    }
}