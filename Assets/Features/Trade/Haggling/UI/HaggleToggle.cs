using System;
using Common.Infrastructure;
using Common.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Trade.Haggling.UI
{
    public sealed class HaggleToggle : MonoBehaviour, IPointerClickHandler
    {
        public event Action<HaggleLevel> Selected;

        [field: SerializeField]
        public HaggleLevel HaggleLevel { get; private set; }

        [SerializeField, Required]
        private TMP_Text titleText, coinText, reputationText;

        [SerializeField, Required]
        private GameObject selectionFrame;

        private void Awake()
        {
            var levelName = ResourceManager.Instance.HaggleResources.HaggleLevelNames[HaggleLevel];
            titleText.text = levelName;

            var configs = ConfigurationManager.Configurations.HaggleConfig.Configs[HaggleLevel];
            var coinFactor = configs.CoinDifferencePercentage;
            var reputation = configs.ReputationPer100Goods;
            coinText.text = $"{coinFactor.ToPercentString(true)} coin";
            reputationText.text = $"{reputation.Sign()}{reputation} Rep";
        }

        public void Toggle(bool isToggled)
        {
            selectionFrame.SetActive(isToggled);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Selected?.Invoke(HaggleLevel);
        }
    }
}