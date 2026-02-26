using System;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using Common.UI.Elements.Cells;
using Common.UI.InventoryUI;
using Common.Utility;
using Features.Player.Retinue;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Features.Player.Camp.UI
{
    public class CompanionCampPanelUiItem : MonoBehaviour
    {
        [SerializeField]
        private CompanionType companionType;

        [SerializeField, Required]
        private Image companionIcon;

        [SerializeField, Required]
        private TMP_Text nameText, levelText, upkeepValueText;

        [SerializeField]
        private LocalizedString levelString;

        [SerializeField, Required]
        private RectTransform effectsContainer, upgradeGoodsContainer;

        [SerializeField, Required]
        private DefaultListItem effectListItemPrefab;

        [SerializeField, Required]
        private CoinCell costItemPrefab;

        [SerializeField, Required]
        private InventoryCell goodItemPrefab;

        private CompanionConfig _companionConfig;
        private CompanionModel _companionModel;

        private readonly Bindings _bindings = new(), _missionBindings = new();

        private void Awake()
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            _companionModel = GameplayContext.Instance.Model.Player.RetinueModel.Companions[companionType];

            companionIcon.sprite = _companionConfig.Get(companionType).Icon;
            nameText.text = _companionConfig.Get(companionType).Name;

            upgradeGoodsContainer.DestroyChildren();
            effectsContainer.DestroyChildren();
        }

        public void Bind()
        {
            _bindings.Track(
                _companionModel.Level.Observe(OnLevelChanged),
                _companionModel.Upkeep.Observe(OnUpkeepChanged)
            );

            TrackMissionItems();
        }

        private void ResetUpgradeMissions()
        {
            _missionBindings.UnbindAll();
            upgradeGoodsContainer.DestroyChildren();
        }

        public void Unbind()
        {
            _bindings.UnbindAll();
            ResetUpgradeMissions();
            effectsContainer.DestroyChildren();
        }

        private void OnLevelChanged(int level)
        {
            levelText.text = levelString.GetLocalizedString(new { _int_Level = level });

            effectsContainer.DestroyChildren();
            if (level <= 0)
                return;

            var levelInfo = _companionConfig.Get(companionType).GetLevelData(level);
            foreach (var line in levelInfo.Description.Split(Environment.NewLine))
            {
                var effectListItem = Instantiate(effectListItemPrefab, effectsContainer);
                effectListItem.Text.text = line;
            }

            ResetUpgradeMissions();
            TrackMissionItems();
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepValueText.text = upkeep.ToString("0.#");
        }

        private void TrackMissionItems()
        {
            var mission = _companionModel.ActiveMission.Value;

            foreach (var (good, item) in mission.MissionItems)
            {
                var cell = Instantiate(goodItemPrefab, upgradeGoodsContainer);
                cell.SetGood(good);
                _missionBindings.Track(item.RemainingAmount.Observe(cell.SetAmount));
            }

            var coinCell = Instantiate(costItemPrefab, upgradeGoodsContainer);
            _missionBindings.Track(mission.CoinCost.RemainingAmount.Observe(coinCell.SetAmount));
        }
    }
}