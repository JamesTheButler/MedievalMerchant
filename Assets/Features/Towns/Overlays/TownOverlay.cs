using System;
using System.Collections.Generic;
using System.Linq;
using Common.Config;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.UI.Elements;
using Common.Utility;
using Features.Towns.Flags.UI;
using Features.Towns.Missions;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Overlays
{
    public sealed class TownOverlay : MonoBehaviour, IOpenClosable
    {
        public event Action Opened;
        public event Action Closed;

        [SerializeField]
        private float yOffset;

        [SerializeField, Required]
        private TownOverlayMissionCell missionCellPrefab;

        [SerializeField, Required]
        private TMP_Text nameText;

        [SerializeField, Required]
        private Image tierIcon;

        [SerializeField, Required]
        private FlagRenderer flagRenderer;

        [SerializeField, Required]
        private RectTransform allMissionsContainer, upgradeMissionContainer, tradeMissionContainer;

        private readonly Bindings _bindings = new();
        private readonly Dictionary<Mission, TownOverlayMissionCell> _missionCells = new();

        private TierResources _tierResources;
        private DateModel _dateModel;

        private Town _town;
        private Vector3 _worldPosition;

        public void SetUp(Town town)
        {
            _dateModel = GameplayContext.Instance.Model.DateModel;
            _tierResources = ResourceManager.Instance.TierResources;

            _town = town;
            _worldPosition = town.WorldLocation.FromXY();

            nameText.text = _town.Name;
            flagRenderer.SetFlag(_town.FlagInfo);
        }

        public void Open()
        {
            // bind all data
            _bindings.Track(
                _dateModel.GameDate.Observe(OnDateChanged, true),
                _town.Tier.Observe(OnTierChanged),
                _town.Missions.MissionAdded.Observe(OnMissionAdded),
                _town.Missions.MissionRemoved.Observe(OnMissionRemoved)
            );

            foreach (var mission in _town.Missions.Missions.Values)
            {
                OnMissionAdded(mission);
            }

            gameObject.SetActive(true);
            RefreshPosition();
        }

        public void Close()
        {
            _bindings.UnbindAll();

            foreach (var mission in _town.Missions.Missions.Values)
            {
                OnMissionRemoved(mission);
            }

            gameObject.SetActive(false);
        }

        public void RefreshPosition()
        {
            //var screenPosition = Camera.main!.WorldToScreenPoint(_worldPosition);
            gameObject.transform.position = _worldPosition;
        }

        private void OnDateChanged()
        {
            foreach (var (mission, missionCell) in _missionCells)
            {
                missionCell.SetDaysRemaining(mission.DaysLeft);
            }
        }

        private void OnTierChanged(Tier tier)
        {
            tierIcon.sprite = _tierResources.Icons[tier];
        }

        private void OnMissionAdded(Mission mission)
        {
            if (_missionCells.ContainsKey(mission))
                return;

            var container = mission.Type == MissionType.TradeMission
                ? tradeMissionContainer
                : upgradeMissionContainer;

            var missionCell = Instantiate(missionCellPrefab, container);
            missionCell.SetUp(mission.Good, mission.TotalLengthInDays, mission.Type, mission.DaysLeft);
            _missionCells.Add(mission, missionCell);
            RefreshMissionContainer();
        }

        private void OnMissionRemoved(Mission mission)
        {
            if (_missionCells.Remove(mission, out var cell))
            {
                Destroy(cell.gameObject);
            }

            RefreshMissionContainer();
        }

        private void RefreshMissionContainer()
        {
            allMissionsContainer.gameObject.SetActive(_town.Missions.Missions.Any());
        }
    }
}