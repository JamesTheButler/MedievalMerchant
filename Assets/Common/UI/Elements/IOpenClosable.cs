using System;

namespace Common.UI.Elements
{
    public interface IOpenClosable
    {
        public event Action Opened, Closed;

        void Open();
        void Close();
    }
}