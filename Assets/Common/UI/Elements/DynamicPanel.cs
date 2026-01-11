using System;

namespace Common.UI.Elements
{
    public abstract class DynamicPanel : InitializableBehavior, IOpenClosable
    {
        public event Action Opened, Closed;

        private bool _isOpen;

        private bool _isInitialized;

        public override void Initialize()
        {
            _isOpen = gameObject.activeSelf;

            if (_isInitialized)
                return;

            OnInitialize();

            _isInitialized = true;
        }

        public void Open()
        {
            if (_isOpen)
                return;

            _isOpen = true;

            OnOpen();

            Opened?.Invoke();
        }

        public virtual void Close()
        {
            if (!_isOpen)
                return;

            _isOpen = false;

            OnClose();

            Closed?.Invoke();
        }

        public void Toggle()
        {
            if (_isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        protected virtual void OnInitialize() { }

        protected abstract void OnOpen();
        protected abstract void OnClose();
    }
}