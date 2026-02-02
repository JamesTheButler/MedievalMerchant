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

            OnOpen();
            Opened?.Invoke();
            IsOpen = true;
        }

        public virtual void Close()
        {
            if (!IsOpen)
                return;

            OnClose();
            Closed?.Invoke();
            IsOpen = false;
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