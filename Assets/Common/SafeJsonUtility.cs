using System;
using UnityEngine;

namespace Common
{
    public static class SafeJsonUtility
    {
        public static T FromJson<T>(string json, T defaultValue = default)
        {
            try
            {
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}