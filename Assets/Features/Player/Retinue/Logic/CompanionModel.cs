using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Player.Retinue.Config.CompanionDatas;
using Features.Player.Retinue.Logic.Modifiers;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionModel
    {
        public string Name { get; }
        public CompanionType CompanionType { get; }
        public IReadOnlyObservable<int> Level => _level;
        public ModifiableVariable Upkeep { get; }
        public Observable<CompanionMission> ActiveMission { get; } = new();
        public CompanionUpkeepModifier UpkeepModifier { get; }

        public int MaxLevel => _companionConfig.MaxLevel;

        private readonly Observable<int> _level = new();

        private readonly CompanionConfigData _companionConfig;

        public CompanionModel(CompanionType companionType)
        {
            var companionResource = ResourceManager.Instance.CompanionResources.Get(companionType);
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig.Get(companionType);


            Name = companionResource.Name;
            CompanionType = companionType;
            UpkeepModifier = new CompanionUpkeepModifier(companionType);

            var loc = ResourceManager.Instance.LocalizationResources;
            Upkeep = new ModifiableVariable(loc.Player.Companions.CompanionUpkeep(Name), false);

            Upkeep.AddModifier(UpkeepModifier);
        }

        public void SetLevel(int newLevel)
        {
            var clampedLevel = Mathf.Min(MaxLevel, newLevel);
            _level.Value = clampedLevel;
            UpkeepModifier.SetLevel(clampedLevel);
        }

        public void StartMission(int coinCost, IReadOnlyDictionary<Good, int> targetGoods)
        {
            ActiveMission.Value = new CompanionMission(coinCost, targetGoods);
            ActiveMission.Value.Completed.Observe(OnMissionCompleted);
        }

        private void OnMissionCompleted()
        {
            ActiveMission.Value.Completed.StopObserving(OnMissionCompleted);
            ActiveMission.Value = null;
        }
    }
}