using UnityEngine;

namespace Data
{
    public static class RandomUtility
    {
        public static bool GetBool(float chanceForTrue)
        {
            return Random.value < chanceForTrue;
        }
    }
}