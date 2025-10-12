using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Features.Towns.Development.Config
{
    /// <summary>
    /// Contains all milestone config data for a single Tier.
    /// The key is the activation threshold from 0-100.
    /// </summary>
    [Serializable]
    public sealed class DevelopmentMilestoneDataSet
    {
        [field: SerializeField, SerializedDictionary("Activation Threshold", "Milestone Data")]
        public SerializedDictionary<float, DevelopmentMilestoneData> MilestoneData { get; private set; }
    }
}