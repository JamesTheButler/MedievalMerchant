using System.Collections.Generic;

namespace Common.Infrastructure.Observation
{
    public sealed class Bindings
    {
        private readonly List<IBinding> _bindings = new();

        public void Track(IBinding binding)
        {
            _bindings.Add(binding);
        }

        public void Track(params IBinding[] binding)
        {
            _bindings.AddRange(binding);
        }

        public void UnbindAll()
        {
            foreach (var binding in _bindings)
            {
                binding.Unbind();
            }

            _bindings.Clear();
        }
    }
}