using System;
using Common.UI.Elements.Panels;
using UnityEngine;

namespace Common.UI.Popups
{
    public abstract class Popup : MonoBehaviour, IOpenClosable
    {
        public event Action Opened;
        public event Action Closed;

        public virtual void Open()
        {
            gameObject.SetActive(true);
            PopupManager.Instance.Show(this);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
            PopupManager.Instance.Hide(this);
        }
    }
}