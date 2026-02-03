using System;
using Features.Goods.Selector;
using UnityEngine;

namespace Features.Towns.Initialization
{
    [Serializable]
    public sealed class UpgradeMissionInitData : InitData
    {
        [SerializeField]
        private GoodSelectorData upgradeMissionGoodSelector;

        [SerializeField]
        private int enforcedMissionLength = -1;

        public override void Initialize(Town town)
        {
            town.Missions.LimitGoodSelection(upgradeMissionGoodSelector.Selector);
            if (enforcedMissionLength > 0)
            {
                town.Missions.OverrideMissionLength(enforcedMissionLength);
            }
        }
    }
}