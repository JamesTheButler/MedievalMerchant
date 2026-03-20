using System;
using Common.Types;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [Serializable]
    public sealed class AllyEffectData : EffectData
    {
        [field: SerializeField]
        public float StartReputationAlly { get; private set; }

        [field: SerializeField]
        public float StartReputationOpponent { get; private set; }

        public Region? AllyRegion { get; private set; }

        public override string Description => Loc.AllyEffectDescription;

        public void SetRegion(Region region)
        {
            AllyRegion = region;
        }
    }
}