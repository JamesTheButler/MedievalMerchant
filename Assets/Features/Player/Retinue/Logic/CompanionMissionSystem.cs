using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Goods;
using Features.Player.Retinue.Config;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionMissionSystem : ISystem
    {
        private readonly CompanionType _companionType;

        private CompanionConfig _companionConfig;
        private CompanionModel _companionModel;
        private RetinueModel _retinueModel;
        private GoodPool _goodPool;

        public CompanionMissionSystem(CompanionType companionType)
        {
            _companionType = companionType;
        }

        public void Initialize()
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            _retinueModel = GameplayContext.Instance.Model.Player.RetinueModel;
            _companionModel = _retinueModel.Companions[_companionType];
            _goodPool = GameplayContext.Instance.Model.GoodPool;

            _companionModel.Level.Observe(OnLevelChanged);
        }

        public void CleanUp()
        {
            _companionModel.Level.StopObserving(OnLevelChanged);
        }

        private void OnLevelChanged(int level)
        {
            _companionModel.ActiveMission.Value = null;

            var missionConfig = _companionConfig.Get(_companionModel.CompanionType).MissionConfig;
            var nextMissionConfig = missionConfig.ConfigsPerLevel.ElementAtOrDefault(level);

            if (nextMissionConfig == null)
                return;

            var coinCost = ApplyNegotiatorDiscount(nextMissionConfig.Cost);

            var missionTargets = new Dictionary<Good, int>();
            foreach (var (goodTier, countData) in nextMissionConfig.ItemsPerTier)
            {
                var poolSize = _goodPool.GetSize(goodTier);
                var pickedGoods = new HashSet<Good>();
                while (pickedGoods.Count < countData.AmountOfDifferentGoods && pickedGoods.Count < poolSize)
                {
                    var pickedGood = _goodPool.GetRandom(goodTier);
                    if (!pickedGoods.Add(pickedGood))
                        continue;

                    missionTargets.Add(pickedGood, countData.CountPerGood);
                }
            }

            _companionModel.StartMission(coinCost, missionTargets);
            _companionModel.ActiveMission.Value.Completed.Observe(OnMissionCompleted);
        }

        private int ApplyNegotiatorDiscount(int baseCost)
        {
            var negotiatorLevel = _retinueModel.Companions[CompanionType.Negotiator].Level.Value;
            if (negotiatorLevel <= 0)
                return baseCost;

            var levelData = _companionConfig.NegotiatorData.GetTypedLevelData(negotiatorLevel);
            if (levelData == null)
                return baseCost;

            var discountedCost = baseCost * (1f - levelData.UpgradeCostReduction);
            return Mathf.RoundToInt(discountedCost);
        }

        private void OnMissionCompleted()
        {
            var newLevel = _companionModel.Level.Value + 1;
            Debug.Log($"Companion {_companionType} mission completed. Upgrading to level {newLevel}.");
            _companionModel.SetLevel(newLevel);
        }
    }
}