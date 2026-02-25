using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.UI.Elements.Cells;
using Common.UI.InventoryUI;
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
        private TMP_Text nameText, levelText, upkeepValueText, upgradeHeaderText;

        [SerializeField]
        private LocalizedString levelString;

        [SerializeField, Required]
        private RectTransform effectsContainer, upgradeGoodsContainer;

        [SerializeField, Required]
        private CoinCell costItemPrefab;
        [SerializeField, Required]
        private InventoryCell goodItemPrefab;
        
        private CompanionConfig _companionConfig;
        private CompanionModel _companionModel;

        private readonly Bindings _bindings = new();

        private void Awake()
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            _companionModel = GameplayContext.Instance.Model.Player.RetinueModel.Companions[companionType];

            companionIcon.sprite = _companionConfig.Get(companionType).Icon;
            nameText.text = _companionConfig.Get(companionType).Name;
        }

        public void Bind()
        {
            _bindings.Track(
                _companionModel.Level.Observe(OnLevelChanged),
                _companionModel.Upkeep.Observe(OnUpkeepChanged)
            );
        }

        public void Unbind()
        {
            _bindings.UnbindAll();
        }

        private void OnLevelChanged(int level)
        {
            levelText.text = levelString.GetLocalizedString(new { _int_LevelIndex = level });
        }

        private void OnUpkeepChanged(float upkeep)
        {
            //levelText.SetArguments(level);
        }


        private Dictionary<Good, InventoryCell> _cells = new();
        private CoinCell _coinCell;
        private void TrackMissionItems()
        {
            var missionBindings = new Bindings();
            _coinCell = Instantiate(costItemPrefab, upgradeGoodsContainer);
            var mission = _companionModel.ActiveMission.Value;
            missionBindings.Track(mission.CoinCost.RemainingAmount.Observe(_coinCell.SetAmount));
            foreach (var (good, item) in _companionModel.ActiveMission.Value.MissionItems)
            {
                var cell = Instantiate(goodItemPrefab, upgradeGoodsContainer);
                cell.SetGood(good);
                missionBindings.Track(item.RemainingAmount.Observe(cell.SetAmount));
            }
        }
    }
}