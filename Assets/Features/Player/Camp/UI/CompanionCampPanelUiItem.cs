using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.Utility;
using Features.Player.Retinue;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
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
        private LocalizeStringEvent nameText, levelText, upkeepValueText, upgradeHeaderText;

        [SerializeField, Required]
        private RectTransform effectsContainer, upgradeGoodsContainer;

        private CompanionConfig _companionConfig;
        private CompanionModel _companionModel;

        private readonly Bindings _bindings = new();

        private void Awake()
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            _companionModel = GameplayContext.Instance.Model.Player.RetinueModel.Companions[companionType];

            companionIcon.sprite = _companionConfig.Get(companionType).Icon;
            //nameText.StringReference = _companionConfig.Get(companionType).Name;
            nameText.GetComponent<TMP_Text>().text = _companionConfig.Get(companionType).Name;
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
            levelText.SetArguments(level);
        }

        private void OnUpkeepChanged(float upkeep)
        {
            //levelText.SetArguments(level);
        }
    }
}