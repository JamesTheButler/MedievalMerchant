using System.Collections.Generic;
using Common.Infrastructure.Observation;
using Common.Types;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionModel
    {
        public CompanionType CompanionType { get; }
        public IReadOnlyObservable<int> Level => _level;

        private readonly Observable<int> _level = new();

        public Observable<CompanionMission> ActiveMission { get; } = new();

        public CompanionModel(CompanionType companionType)
        {
            CompanionType = companionType;
        }

        public void SetLevel(int newLevel)
        {
            _level.Value = newLevel;
        }

        public void StartMission(int coinCost, IReadOnlyDictionary<Good, int> targetGoods)
        {
            if (ActiveMission.Value != null)
            {
                Debug.LogWarning("Tried starting a companion mission while one is already active.");
                return;
            }

            ActiveMission.Value = new CompanionMission(coinCost, targetGoods);
        }
    }
}