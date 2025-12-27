using System;
using System.Collections.Generic;
using Common.Types;
using Features.Towns.Missions;
using UnityEngine;

namespace Features.Towns
{
    public sealed class MissionModel
    {
        public event Action<Mission> MissionAdded, MissionRemoved;

        public IReadOnlyDictionary<Good, Mission> Missions => _missions;

        private readonly Dictionary<Good, Mission> _missions = new();

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
    }
}