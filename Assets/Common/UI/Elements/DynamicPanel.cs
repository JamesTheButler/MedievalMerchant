using System;

namespace Common.UI.Elements
{
    public abstract class DynamicPanel : InitializableBehavior, IOpenClosable
    {
        public event Action Opened, Closed;

        public bool IsOpen { get; private set; }

        private bool _isInitialized;

        public override void Initialize()
        {
            IsOpen = gameObject.activeSelf;

            if (_isInitialized)
                return;

            OnInitialize();

            _isInitialized = true;
        }

        public void Open()
        {
            if (IsOpen)
                return;

            IsOpen = true;

            OnOpen();

            Opened?.Invoke();
        }

        public virtual void Close()
        {
            if (!IsOpen)
                return;

            IsOpen = false;

            OnClose();

            Closed?.Invoke();
        }

        public void Toggle()
        {
            if (IsOpen)
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