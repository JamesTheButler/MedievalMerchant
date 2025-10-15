using System.Collections.Generic;
using Common;
using Common.UI;
using Features.Player.Retinue.Config;
using NaughtyAttributes;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.Player.Retinue.UI
{
    public sealed class CompanionUI : MonoBehaviour
    {
        [SerializeField]
        private CompanionType companionType;

        [SerializeField]
        public UnityEvent<CompanionType, int> levelUpgradeRequested;

        [Header("Set Up")]
        [SerializeField, Required]
        private Image companionIcon;

        [SerializeField, Required]
        private RectTransform levelUiParent;

        [SerializeField, Required]
        private GameObject levelUiPrefab;

        [SerializeField, Required]
        private CompanionTooltipHandler tooltip;

        private RetinueManager _retinueManager;
        private CompanionConfigData _configData;

        private readonly List<CompanionLevelUI> _levelUIs = new();

        private int _currentLevel = -1;

        private void Start()
        {
            _retinueManager = Model.Instance.Player.RetinueManager;
            _configData = ConfigurationManager.Instance.CompanionConfig.Get(companionType);

            InitializeUI();

            _retinueManager.CompanionLevels[companionType].Observe(OnCompanionLevelChanged);
        }

        private void InitializeUI()
        {
            companionIcon.sprite = _configData.Icon;
            UpdateTooltip();
            for (var i = 0; i < _configData.Levels.Count; i++)
            {
                var levelUi = Instantiate(levelUiPrefab, levelUiParent);
                var levelUIScript = levelUi.GetComponent<CompanionLevelUI>();

                // increment index by 1 as lvl 0 means nothing is upgraded
                levelUIScript.Setup(i + 1, companionType);
                levelUIScript.UnlockRequested += levelUpgradeRequested.Invoke;

                _levelUIs.Add(levelUIScript);
            }
        }

        private void UpdateTooltip()
        {
            tooltip.SetTooltip(new CompanionTooltip.Data(companionType, _currentLevel));
        }

        // TODO - STYLE: this code is quite cumbersome and funky
        private void OnCompanionLevelChanged(int newLevel)
        {
            if (newLevel == _currentLevel) return;

            if (newLevel > _currentLevel)
            {
                for (var i = _currentLevel + 1; i <= newLevel; i++)
                {
                    var levelUiId = newLevel - 1; // level 1 is in level ui 0, etc.
                    if (levelUiId >= 0)
                    {
                        _levelUIs[levelUiId].SetUpgraded(true);
                    }

                    var nextLevelUiId = levelUiId + 1;
                    if (nextLevelUiId < _levelUIs.Count)
                    {
                        _levelUIs[nextLevelUiId].SetUnlocked(true);
                    }
                }
            }
            else
            {
                for (var i = _currentLevel; i > newLevel; i--)
                {
                    var levelUiId = newLevel - 1; // level 1 is in level ui 0, etc.
                    if (levelUiId >= 0)
                    {
                        _levelUIs[levelUiId].SetUpgraded(false);
                    }

                    var nextLevelUiId = levelUiId + 1;
                    if (nextLevelUiId < _levelUIs.Count)
                    {
                        _levelUIs[nextLevelUiId].SetUnlocked(false);
                    }
                }
            }

            _currentLevel = newLevel;

            UpdateTooltip();
        }
    }
}