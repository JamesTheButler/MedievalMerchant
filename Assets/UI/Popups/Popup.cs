using UnityEngine;

namespace UI.Popups
{
    public abstract class Popup : MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
            PopupManager.Instance.Show(this);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            PopupManager.Instance.Hide(this);
        }
    }
}