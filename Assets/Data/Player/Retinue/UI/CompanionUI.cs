using System.Collections.Generic;
using Data.Configuration;
using Data.Player.Retinue.Config;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Player.Retinue.UI
{
    public sealed class CompanionUI : MonoBehaviour
    {
        [SerializeField] private CompanionType companionType;
        [SerializeField, Required] private Image companionIcon;
        [SerializeField, Required] private RectTransform levelUiParent;
        [SerializeField, Required] private GameObject levelUiPrefab;

        private RetinueManager _retinueManager;
        private CompanionConfigData _configData;

        private readonly List<CompanionLevelUI> _levelUIs = new();

        private int _currentLevel;

        private void Awake()
        {
            _retinueManager = Model.Instance.Player.RetinueManager;
            _configData = ConfigurationManager.Instance.CompanionConfig.Get(companionType);

            InitializeUI();

            _retinueManager.CompanionLevels[companionType].Observe(OnCompanionLevelChanged);
        }

        private void InitializeUI()
        {
            companionIcon.sprite = _configData.Icon;
            for (var i = 0; i < _configData.Levels.Count; i++)
            {
                var levelUi = Instantiate(levelUiPrefab, levelUiParent);
                var levelUIScript = levelUi.GetComponent<CompanionLevelUI>();
                levelUIScript.Setup(i + 1, _configData.Levels[i]); // level 0 means nothing is upgraded
                _levelUIs.Add(levelUIScript);
            }
        }

        private void OnCompanionLevelChanged(int level)
        {
            if (level == _currentLevel) return;

            if (level > _currentLevel)
            {
                for (var i = _currentLevel + 1; i <= level; i++)
                {
                    _levelUIs[level].SetUnlocked(true);
                }
                // TODO: need to sort out unlockability for levels larger than ´level´
            }
            else
            {
                for (var i = _currentLevel; i > level; i--)
                {
                    _levelUIs[level].SetUnlocked(false);
                }
                // TODO: need to sort out unlockability for levels larger than ´level´
            }

            _currentLevel = level;
        }
    }
}