using UnityEngine;

namespace Data
{
    public static class FloatExtensions
    {
        private const float DefaultTolerance = 0.0001f;

        public static string Sign(this float value)
        {
            return value switch
            {
                > DefaultTolerance => "+",
                < -DefaultTolerance => "",
                _ => "+/-",
            };
        }

        public static bool IsApproximately(this float self, float other, float tolerance = DefaultTolerance)
        {
            return Mathf.Abs(self - other) < tolerance;
        }
    }
}