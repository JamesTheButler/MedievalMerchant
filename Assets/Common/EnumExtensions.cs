using System;
using Random = UnityEngine.Random;

namespace Common
{
    public static class EnumExtensions
    {
        public static T GetRandom<T>() where T : Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            var index = Random.Range(0, values.Length);
            return values[index];
        }
    }
}