using System.Collections.Generic;
using Common.Infrastructure;
using Common.Utility;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Player.Retinue.UI
{
    public sealed class CompanionUI : MonoBehaviour
    {
        [SerializeField]
        private CompanionType companionType;

        [SerializeField, Required]
        private Image companionIcon, fadeOutImage;

        [SerializeField, Required]
        private RectTransform levelUiParent;

        [SerializeField, Required]
        private GameObject levelUiPrefab;

        [SerializeField, Required]
        private CompanionTooltipHandler tooltip;

        [SerializeField, Required]
        private TMP_Text nameText, descriptionText, effectsText;

        private RetinueModel _retinueModel;
        private CompanionConfigData _configData;
        private CompanionUpgradeService _companionUpgradeService;

        private readonly List<CompanionLevelUI> _levelUIs = new();

        private int _currentLevel = -1;

        private void Start()
        {
            _retinueModel = GameplayContext.Instance.Model.Player.RetinueModel;
            _configData = ConfigurationManager.Configurations.CompanionConfig.Get(companionType);
            _companionUpgradeService = GameplayContext.Instance.Services.CompanionUpgradeService;

            InitializeUI();

            _retinueModel.CompanionLevels[companionType].Observe(OnCompanionLevelChanged);
        }

        private void InitializeUI()
        {
            levelUiParent.DestroyChildren();
            nameText.text = _configData.Name;
            descriptionText.text = _configData.Description;
            companionIcon.sprite = _configData.Icon;
            companionIcon.color = _configData.IsImplemented ? Color.white : Color.white.WithAlpha(0.5f);
            fadeOutImage.enabled = !_configData.IsImplemented;

            UpdateTooltip();
            UpdateEffectsText();
            for (var i = 0; i < _configData.Levels.Count; i++)
            {
                var levelUi = Instantiate(levelUiPrefab, levelUiParent);
                var levelUIScript = levelUi.GetComponent<CompanionLevelUI>();

                // increment index by 1 as lvl 0 means nothing is upgraded
                levelUIScript.Setup(i + 1, companionType);
                levelUIScript.UnlockRequested += _companionUpgradeService.LevelUpgradeRequested;

                _levelUIs.Add(levelUIScript);
            }
        }

        private void UpdateTooltip()
        {
            tooltip.SetData(new CompanionTooltip.Data(companionType, _currentLevel));
        }

        private void OnCompanionLevelChanged(int newLevel)
        {
            if (newLevel == _currentLevel)
                return;

            // level 1 is in level ui 0, etc.
            var newLevelUiId = newLevel - 1;

            for (var i = 0; i < _levelUIs.Count; i++)
            {
                CompanionLevelUI.State state;
                if (i <= newLevelUiId)
                {
                    state = CompanionLevelUI.State.Unlocked;
                }
                else if (i == newLevelUiId + 1)
                {
                    state = CompanionLevelUI.State.Unlockable;
                }
                else
                {
                    state = CompanionLevelUI.State.Locked;
                }

                _levelUIs[i].SetState(state);
            }

            _currentLevel = newLevel;

            UpdateTooltip();
            UpdateEffectsText();
        }

        private void UpdateEffectsText()
        {
            var levelData = _configData.GetLevelData(_currentLevel);
            effectsText.text = levelData?.Description ?? string.Empty;
        }
    }
}