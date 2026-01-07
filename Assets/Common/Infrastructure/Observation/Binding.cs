using System;

namespace Common.Infrastructure.Observation
{
    public class Binding : IBinding
    {
        private readonly Action _unbindAction;

        public Binding(Action unbindAction)
        {
            _unbindAction = unbindAction;
        }

        public void Unbind()
        {
            _unbindAction.Invoke();
        }
    }
}