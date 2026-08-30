using System.Collections.Generic;
using Common.Utility;

namespace Common.Infrastructure.Observation
{
    public sealed class Bindings : IBinding
    {
        private readonly HashSet<IBinding> _bindings = new();

        public void Track(IBinding binding)
        {
            _bindings.Add(binding);
        }

        public void Track(params IBinding[] binding)
        {
            _bindings.Add(binding);
        }

        public void Unbind()
        {
            foreach (var binding in _bindings)
            {
                binding.Unbind();
            }

            _bindings.Clear();
        }
    }
}