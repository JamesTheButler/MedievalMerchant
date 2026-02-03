using System;
using System.Collections.Generic;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Goods.Selector;
using UnityEngine;

namespace Features.Towns.Missions
{
    public sealed class MissionModel
    {
        public ObservableEvent<Mission> MissionAdded { get; } = new();
        public ObservableEvent<Mission> MissionRemoved { get; } = new();

        public event Action GoodSelectorChanged;

        public IReadOnlyDictionary<Good, Mission> Missions => _missions;

        // used for tutorial to set an arbitrary length of upgrade mission
        public int? MissionLengthOverride { get; private set; }

        private readonly Dictionary<Good, Mission> _missions = new();

        public IGoodSelector PermittedGoodsSelector { get; private set; } = IGoodSelector.All;

        public void AddMission(Mission mission)
        {
            if (!_missions.TryAdd(mission.Good, mission))
            {
                Debug.LogError($"Failed to add mission. Mission for {mission.Good} is already added.");
                return;
            }

            MissionAdded?.Invoke(mission);
        }

        public void RemoveMission(Mission mission)
        {
            if (!_missions.Remove(mission.Good))
            {
                Debug.LogError($"Failed to remove mission. No mission found for {mission.Good}.");
                return;
            }

            MissionRemoved?.Invoke(mission);
        }

        public void LimitGoodSelection(IGoodSelector selector)
        {
            PermittedGoodsSelector = selector;
            GoodSelectorChanged?.Invoke();
        }

        public void OverrideMissionLength(int length)
        {
            MissionLengthOverride = length;
        }
    }
}