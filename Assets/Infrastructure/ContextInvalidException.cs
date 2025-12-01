using System;

namespace Infrastructure
{
    public sealed class ContextInvalidException : Exception
    {
        public ContextInvalidException(string name) : base($"Context '{name}' is invalid") { }
    }
}