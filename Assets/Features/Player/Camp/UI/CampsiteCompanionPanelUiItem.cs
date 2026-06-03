using System;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Elements.Cells;
using Common.UI.InventoryUI;
using Common.Utility;
using Features.Player.Retinue;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.Config.CompanionDatas;
using Features.Player.Retinue.Config.Resources;
using Features.Player.Retinue.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteCompanionPanelUiItem : MonoBehaviour
    {
        [SerializeField]
        private CompanionType companionType;

        [SerializeField, Required]
        private Image companionIcon;

        [SerializeField, Required]
        private TMP_Text nameText, levelText, upkeepValueText, descriptionText, forHireText, upgradeText;

        [SerializeField]
        private LocalizedString levelString, deliveryString, hireString;

        [SerializeField, Required]
        private RectTransform effectsContainer, missionItemContainer;

        [SerializeField, Required]
        private GameObject notHiredDetails, hiredDetails;

        [SerializeField, Required]
        private DefaultListItem effectListItemPrefab;

        [SerializeField, Required]
        private CoinCell costItemPrefab;

        [SerializeField, Required]
        private InventoryCell goodItemPrefab;

        [SerializeField, Required]
        private GameObject upgradeGroup;

        [SerializeField, Required]
        private CompanionDeliveryPanel deliveryPanel;

        private CompanionConfig _companionConfig;
        private CompanionConfigData _companionConfigData;
        private CompanionResource _companionResource;
        private CompanionModel _companionModel;

        private readonly Bindings _bindings = new(), _missionBindings = new();
        private int _currentLevel;

        private void Awake()
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            _companionResource = ResourceManager.Instance.CompanionResources.Get(companionType);
            _companionModel = GameplayContext.Instance.Model.Player.RetinueModel.Companions[companionType];

            companionIcon.sprite = _companionResource.Icon;
            nameText.text = _companionResource.Name;
            descriptionText.text = _companionResource.Description;

            _companionConfigData = _companionConfig.Get(companionType);

            missionItemContainer.DestroyChildren();
            effectsContainer.DestroyChildren();
        }

        public void Bind()
        {
            Unbind();

            _bindings.Track(
                _companionModel.Level.Observe(OnLevelChanged),
                _companionModel.Upkeep.Observe(OnUpkeepChanged),
                _companionModel.ActiveMission.Observe(OnActiveMissionChanged)
            );
        }

        public void Unbind()
        {
            _bindings.UnbindAll();
            _missionBindings.UnbindAll();

            missionItemContainer.DestroyChildren();
            effectsContainer.DestroyChildren();
        }

        public void HoverNextLevel()
        {
            if (_currentLevel == _companionConfigData.MaxLevel)
                return;

            SetLevelInfo(_currentLevel + 1, true);
        }

        public void UnhoverNextLevel()
        {
            if (_currentLevel == _companionConfigData.MaxLevel)
                return;

            SetLevelInfo(_currentLevel, true);
        }

        private void OnLevelChanged(int level)
        {
            _currentLevel = level;
            SetLevelInfo(level, false);
        }

        private void SetLevelInfo(int level, bool ignoreUpdateSection)
        {
            var isHired = level > 0;

            forHireText.gameObject.SetActive(!isHired);
            notHiredDetails.SetActive(!isHired);
            levelText.gameObject.SetActive(isHired);
            hiredDetails.SetActive(isHired);

            if(!ignoreUpdateSection)
            {
                upgradeText.text = isHired ? deliveryString.GetLocalizedString() : hireString.GetLocalizedString();
            }

            if (!isHired)
                return;

            levelText.text = levelString.GetLocalizedString(new { _int_Level = level });

            var levelInfo = _companionConfigData.GetLevelData(level);
            SetLevelDetails(levelInfo.Description);
        }

        private void SetLevelDetails(string description)
        {
            effectsContainer.DestroyChildren();
            foreach (var line in description.Split(Environment.NewLine))
            {
                if (line == string.Empty)
                    continue;

                var effectListItem = Instantiate(effectListItemPrefab, effectsContainer);
                effectListItem.Text.text = line;
            }
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepValueText.text = upkeep.ToString("0.#");
        }

        private void OnActiveMissionChanged(CompanionMission mission)
        {
            missionItemContainer.DestroyChildren();
            _missionBindings.UnbindAll();

            upgradeGroup.SetActive(mission != null);

            if (mission == null)
                return;

            foreach (var (good, item) in mission.MissionItems)
            {
                var cell = Instantiate(goodItemPrefab, missionItemContainer);
                cell.SetGood(good);

                _missionBindings.Track(item.RemainingAmount.Observe(cell.SetAmount));
                _missionBindings.Track(item.IsCompleted.Observe(isCompleted => cell.EnableCornerIcon(isCompleted)));

                var capturedGood = good;
                cell.Clicked += () => OnGoodCellClicked(capturedGood);
            }

            var coinCell = Instantiate(costItemPrefab, missionItemContainer);
            _missionBindings.Track(mission.CoinCost.RemainingAmount.Observe(coinCell.SetAmount));
            coinCell.Clicked += OnCoinCellClicked;
        }

        private void OnGoodCellClicked(Good good)
        {
            var mission = _companionModel.ActiveMission.Value;
            if (mission == null)
                return;

            if (!mission.MissionItems.TryGetValue(good, out var item) || item.IsCompleted.Value)
                return;

            deliveryPanel.SetUp(companionType, item);
            deliveryPanel.Open();
        }

        private void OnCoinCellClicked()
        {
            var mission = _companionModel.ActiveMission.Value;
            if (mission == null || mission.CoinCost.IsCompleted.Value)
                return;

            deliveryPanel.SetUp(companionType, mission.CoinCost);
            deliveryPanel.Open();
        }
    }
}