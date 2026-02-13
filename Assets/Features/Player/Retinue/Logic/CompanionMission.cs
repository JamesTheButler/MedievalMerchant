using System.Collections.Generic;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionMission
    {
        public Dictionary<Good, CompanionMissionItem> MissionItems { get; } = new();

        private readonly HashSet<CompanionMissionItem> _incompleteItems = new();

        public ObservableEvent Completed { get; } = new();

        public CompanionMission(IReadOnlyDictionary<Good, int> goods)
        {
            foreach (var (good, amount) in goods)
            {
                var item = new CompanionMissionItem(good, amount);

                MissionItems.Add(good, item);
                item.IsCompleted.Observe(isComplete => OnMissionCompleted(item, isComplete), false);
                _incompleteItems.Add(item);
            }
        }

        public void Deliver(Good good, int amount)
        {
            if (!MissionItems.TryGetValue(good, out var item))
            {
                Debug.LogWarning($"This companion mission does not required good '{good}'.");
                return;
            }

            item.Deliver(amount);
        }

        private void OnMissionCompleted(CompanionMissionItem item, bool isComplete)
        {
            if (isComplete)
            {
                _incompleteItems.Remove(item);
            }
            else
            {
                _incompleteItems.Add(item);
            }

            if (_incompleteItems.IsEmpty())
            {
                Completed.Invoke();
            }
        }
    }
}