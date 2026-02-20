using System;
using Common.Infrastructure;
using Common.Types;
using Features.Goods.Selector;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [Serializable]
    public sealed class MissionLimiterEffectData : EffectData
    {
        [field: SerializeField, InfoBox("Regions that towns will ONLY request goods from.")]
        public Region MissionRegion { get; private set; }

        [field: SerializeField]
        public GoodSelectorData GoodSelector { get; private set; }

        public override string Description
        {

            get
            {
                var missionResources = ResourceManager.Instance.RegionResources;
                var missionName = missionResources.Data[MissionRegion].Name;
                return $"Non-{missionName} towns will only request goods from {missionName} towns.";
            }
        }
    }
}