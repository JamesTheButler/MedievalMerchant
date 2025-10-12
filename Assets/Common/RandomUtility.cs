using UnityEngine;

namespace Common
{
    public static class RandomUtility
    {
        public static bool GetBool(float chanceForTrue)
        {
            return Random.value < chanceForTrue;
        }
    }
}