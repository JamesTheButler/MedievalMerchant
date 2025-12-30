using System.Collections.Generic;
using Common.Utility;
using Features.Towns.Missions;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns.UI
{
    public sealed class TownUIMissionSection : TownUISection
    {
        [SerializeField, Required]
        private GameObject missionPrefab;

        [SerializeField, Required]
        private GameObject tradeMissionContainer, upgradeMissionSection, upgradeMissionContainer;

        private readonly Dictionary<Mission, TownUIMissionSectionItem> _missionUiElements = new();

        public override void Initialize() { }
        public override void CleanUp() { }

        public override void Bind(Town town)
        {
            var missionModel = town.Missions;
            missionModel.MissionAdded += OnMissionAdded;
            missionModel.MissionRemoved += OnMissionRemoved;

            foreach (var mission in missionModel.Missions.Values)
            {
                OnMissionAdded(mission);
            }
        }

        public override void Unbind(Town town)
        {
            var missionModel = town.Missions;
            missionModel.MissionAdded -= OnMissionAdded;
            missionModel.MissionRemoved -= OnMissionRemoved;

            foreach (var mission in missionModel.Missions.Values)
            {
                OnMissionRemoved(mission);
            }

            tradeMissionContainer.DestroyChildren();
        }

        private void OnMissionAdded(Mission mission)
        {
            var uiElement = Instantiate(missionPrefab, tradeMissionContainer.transform);
            var uiElementScript = uiElement.GetComponentInChildren<TownUIMissionSectionItem>();

            uiElementScript.Bind(mission);
            _missionUiElements.Add(mission, uiElementScript);
        }

        private void OnMissionRemoved(Mission mission)
        {
            if (!_missionUiElements.Remove(mission, out var uiElement))
                return;

            uiElement.Unbind(mission);

            Destroy(uiElement.gameObject);
        }
    }
}