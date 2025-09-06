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
            escapeMenuRoot.SetActive(!IsActive);
        }
    }
}