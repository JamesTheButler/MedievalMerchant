using Common.Infrastructure;
using Common.UI.Elements;
using Features.Goods.Config;
using Features.Towns.Missions.Data;
using NaughtyAttributes;
using TMPro;
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
        private TMP_Text titleText, countText, daysLeftText;

        [SerializeField, Required]
        private MissionTooltipHandler missionTooltipHandler;

        private Color _defaultDaysLeftIconColor, _badColor;

        private GoodsResources _goodsResources;
        private MissionConfig _missionConfig;

        private Mission _mission;

        public void Initialize()
        {
            _goodsResources = ResourceManager.Instance.GoodsResources;
            _missionConfig = ConfigurationManager.Configurations.MissionConfig;

            abortButton.onClick.AddListener(AbortButtonClicked);
            _defaultDaysLeftIconColor = daysLeftIcon.color;
            _badColor = ResourceManager.Instance.Colors.Bad;
        }

        public void Bind(Mission mission)
        {
            if (_mission != null)
            {
                Unbind(mission);
            }

            _mission = mission;

            var currentAmount = mission.RemainingCount;

            var goodName = _goodsResources.ResourceData[mission.Good].GoodName;
            titleText.text = $"Sell {mission.TotalCount} {goodName}";
            goodCell.SetGood(mission.Good);

            var isHighlighted = mission.Type == MissionType.UpgradeMission;
            background.sprite = isHighlighted ? highlightedBackground : defaultBackground;

            countText.text = currentAmount.ToString();

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
            daysLeftText.text = $"{daysLeft} days left";

            var isCloseToFailure = daysLeft <= _missionConfig.WarningThresholdDays;
            daysLeftIcon.color = isCloseToFailure ? _badColor : _defaultDaysLeftIconColor;
        }

        private void OnRemainingCountChanged(int remainingCount)
        {
            var deliveredCount = _mission.TotalCount - remainingCount;
            countText.text = $"{deliveredCount}/{_mission.TotalCount} delivered";
        }

        private void AbortButtonClicked()
        {
            _mission?.Fail();
        }
    }
}