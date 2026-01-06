using System;
using Features;
using UnityEngine;

namespace Common.UI.Elements
{
    public abstract class DynamicPanel : MonoBehaviour
    {
        public event Action Opened, Closed;

        private bool _isOpen;

        private bool _isInitialized;

        /// <summary>
        /// One-time setup
        /// </summary>
        public void Initialize()
        {
            _isOpen = gameObject.activeSelf;
            if (_isInitialized) return;

            OnInitialize();

            _isInitialized = true;
        }

        public void Toggle()
        {
            Toggle(_isOpen);
        }

        public void Toggle(bool isOpen)
        {
            if (isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public void Open()
        {
            if (_isOpen) return;

            _isOpen = true;
            OnOpen();
            Opened?.Invoke();
        }

        public virtual void Close()
        {
            if (!_isOpen) return;

            _isOpen = false;
            OnClose();
            Closed?.Invoke();
        }

        protected virtual void OnInitialize() { }
        protected abstract void OnOpen();
        protected abstract void OnClose();
    }
}