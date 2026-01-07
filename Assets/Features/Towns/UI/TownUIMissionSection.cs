using System.Collections.Generic;
using Common.UI.Elements;
using Common.Utility;
using Features.Towns.Missions;
using Features.Trade;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Towns.UI
{
    public sealed class TownUIMissionSection : TownUISection
    {
        [SerializeField]
        private UnityEvent<GoodCell, TradeType> goodCellClicked;

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

            upgradeMissionSection.SetActive(false);
            upgradeMissionContainer.DestroyChildren();
            tradeMissionContainer.DestroyChildren();
        }

        private void OnMissionAdded(Mission mission)
        {
            var isUpgradeMission = mission.Type == MissionType.UpgradeMission;

            if (mission.Type == MissionType.UpgradeMission)
            {
                upgradeMissionSection.SetActive(true);
            }

            var container = isUpgradeMission
                ? upgradeMissionContainer
                : tradeMissionContainer;

            var uiElement = Instantiate(missionPrefab, container.transform);
            var uiElementScript = uiElement.GetComponentInChildren<TownUIMissionSectionItem>();

            uiElementScript.GoodCellClicked += cell => goodCellClicked.Invoke(cell, TradeType.Sell);
            uiElementScript.Initialize();
            uiElementScript.Bind(mission);
            _missionUiElements.Add(mission, uiElementScript);
        }

        private void OnMissionRemoved(Mission mission)
        {
            if (mission.Type == MissionType.UpgradeMission)
            {
                upgradeMissionSection.SetActive(false);
            }

            if (!_missionUiElements.Remove(mission, out var uiElement))
                return;

            uiElement.Unbind(mission);

            Destroy(uiElement.gameObject);
        }
    }
}