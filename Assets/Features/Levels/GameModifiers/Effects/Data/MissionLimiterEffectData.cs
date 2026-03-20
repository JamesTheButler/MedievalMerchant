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
                var regionName = ResourceManager.Instance.RegionResources.Data[MissionRegion].Name;
                return Loc.MissionLimiterEffect(regionName);
            }
        }
    }
}