using System.Collections.Generic;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Player.Retinue.Logic.Modifiers;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionModel
    {
        public CompanionType CompanionType { get; }
        public IReadOnlyObservable<int> Level => _level;
        public IReadOnlyObservable<float> Upkeep => _upkeep;
        public Observable<CompanionMission> ActiveMission { get; } = new();
        public CompanionUpkeepModifier UpkeepModifier { get; }

        private readonly Observable<int> _level = new();
        private readonly Observable<float> _upkeep = new();

        public CompanionModel(CompanionType companionType)
        {
            CompanionType = companionType;
            UpkeepModifier = new CompanionUpkeepModifier(companionType);
        }

        public void SetLevel(int newLevel)
        {
            _level.Value = newLevel;
            UpkeepModifier.SetLevel(newLevel);
        }

        public void StartMission(int coinCost, IReadOnlyDictionary<Good, int> targetGoods)
        {
            ActiveMission.Value = new CompanionMission(coinCost, targetGoods);
        }
    }
}