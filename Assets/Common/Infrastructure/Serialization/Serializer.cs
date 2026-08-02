using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Common.Infrastructure.Serialization
{
    public sealed class Serializer : ISerializer
    {
        public T Deserialize<T>(string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to deserialize save data: {ex.Message}");
                return default;
            }
        }

        public string Serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input);
        }
    }
}