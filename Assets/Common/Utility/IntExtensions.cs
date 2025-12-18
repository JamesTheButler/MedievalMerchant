using UnityEngine;

namespace Common.Utility
{
    public static class IntExtensions
    {
        public static int Clamp(this int value, int min, int max)
        {
            return Mathf.Clamp(value, min, max);
        }
    }
}