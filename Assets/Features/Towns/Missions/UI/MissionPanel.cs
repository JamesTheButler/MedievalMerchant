using System;
using System.Collections.Generic;
using Features.Goods.Config;
using UnityEngine;
using ResourceManager = Common.Infrastructure.ResourceManager;

namespace Features.Towns.Missions.UI
{
    public sealed class MissionPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject missionElementPrefab;

        [SerializeField]
        private GameObject missionListParent;

        private GoodsResources _goodsResources;

        private readonly Dictionary<Mission, MissionUIElement> _missionUiElements = new();
        private readonly Dictionary<Mission, Action<int>> _updateHandlers = new();

        public void Initialize()
        {
            _goodsResources = ResourceManager.Instance.GoodsResources;
        }

        public void Bind(MissionModel missionModel)
        {
            missionModel.MissionAdded += OnMissionAdded;
            missionModel.MissionRemoved += OnMissionRemoved;

            foreach (var mission in missionModel.Missions.Values)
            {
                OnMissionAdded(mission);
            }
        }

        public void Unbind(MissionModel missionModel)
        {
            foreach (var mission in missionModel.Missions.Values)
            {
                OnMissionRemoved(mission);
            }

            missionModel.MissionAdded -= OnMissionAdded;
            missionModel.MissionRemoved -= OnMissionRemoved;
        }

        private void OnMissionAdded(Mission mission)
        {
            var uiElement = Instantiate(missionElementPrefab, missionListParent.transform);
            var uiElementScript = uiElement.GetComponentInChildren<MissionUIElement>();
            var goodIcon = _goodsResources.ResourceData[mission.Good].Icon;
            uiElementScript.Setup(goodIcon, mission.RemainingCount, mission.TotalCount);
            mission.RemainingCount.Observe(OnMissionCountChanged);

            _missionUiElements.Add(mission, uiElementScript);
            _updateHandlers.Add(mission, OnMissionCountChanged);

            void OnMissionCountChanged(int count)
            {
                uiElementScript.UpdateCurrentAmount(count);
            }
        }

        private void OnMissionRemoved(Mission mission)
        {
            if (!_missionUiElements.Remove(mission, out var uiElement))
                return;

            if (_updateHandlers.Remove(mission, out var handler))
            {
                mission.RemainingCount.StopObserving(handler);
            }

            Destroy(uiElement.gameObject);
        }
    }
}