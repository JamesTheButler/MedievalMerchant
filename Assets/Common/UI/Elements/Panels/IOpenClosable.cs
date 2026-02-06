using System;

namespace Common.UI.Elements.Panels
{
    public interface IOpenClosable
    {
        public event Action Opened, Closed;

        void Open();
        void Close();
    }
}