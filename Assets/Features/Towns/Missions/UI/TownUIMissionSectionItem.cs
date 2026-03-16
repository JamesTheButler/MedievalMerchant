using Common.Infrastructure;
using Common.UI.Elements.Cells;
using Features.Goods.Config;
using Features.Localization.UI;
using Features.Towns.Missions.Data;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Missions.UI
{
    public sealed class TownUIMissionSectionItem : MonoBehaviour
    {
        [SerializeField, Required]
        private GoodCell goodCell;

        [SerializeField, Required]
        private Image background, daysLeftIcon;

        [SerializeField, Required]
        private Sprite defaultBackground, highlightedBackground;

        [SerializeField, Required]
        private Button abortButton;

        [SerializeField, Required]
        private LocalizedText titleText, countText, daysLeftText;

        [SerializeField, Required]
        private MissionTooltipHandler missionTooltipHandler;

        [SerializeField]
        private Color badColor;

        private Color _defaultDaysLeftIconColor;
        private GoodResources _goodResources;
        private MissionConfig _missionConfig;
        private Mission _mission;

        public void Initialize()
        {
            _goodResources = ResourceManager.Instance.GoodResources;
            _missionConfig = ConfigurationManager.Configurations.MissionConfig;

            abortButton.onClick.AddListener(AbortButtonClicked);
            _defaultDaysLeftIconColor = daysLeftIcon.color;
        }

        public void Bind(Mission mission)
        {
            if (_mission != null)
            {
                Unbind(mission);
            }

            _mission = mission;

            var goodName = _goodResources.ResourceData[mission.Good].GoodName;
            var args = new
            {
                _int_Amount = mission.TotalCount,
                GoodName = goodName,
            };
            titleText.SetArgs(args);
            goodCell.SetGood(mission.Good);

            var isHighlighted = mission.Type == MissionType.UpgradeMission;
            background.sprite = isHighlighted ? highlightedBackground : defaultBackground;

            missionTooltipHandler.SetData(mission);

            mission.RemainingCount.Observe(OnRemainingCountChanged);
            mission.DaysLeft.Observe(OnDaysLeftChanged);
        }

        public void Unbind(Mission mission)
        {
            if (mission == null)
                return;

            mission.RemainingCount.StopObserving(OnRemainingCountChanged);
            mission.DaysLeft.StopObserving(OnDaysLeftChanged);
        }

        private void OnDaysLeftChanged(int daysLeft)
        {
            var args = new { _int_Days = daysLeft };
            daysLeftText.SetArgs(args);

            var isCloseToFailure = daysLeft <= _missionConfig.WarningThresholdDays;
            daysLeftIcon.color = isCloseToFailure ? badColor : _defaultDaysLeftIconColor;
        }

        private void OnRemainingCountChanged(int remainingCount)
        {
            var deliveredCount = _mission.TotalCount - remainingCount;
            var args = new
            {
                _int_CurrentValue = deliveredCount,
                _int_MaxValue = _mission.TotalCount,
            };
            countText.SetArgs(args);
        }

        private void AbortButtonClicked()
        {
            _mission?.Fail();
        }
    }
}