using UnityEngine;

namespace Common.Utility
{
    public static class RandomUtility
    {
        public static bool GetBool(float chanceForTrue)
        {
            return Random.value < chanceForTrue;
        }
    }
}