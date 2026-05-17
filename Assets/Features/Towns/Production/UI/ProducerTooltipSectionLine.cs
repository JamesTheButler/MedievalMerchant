using Common.Infrastructure;
using Features.Goods.Config;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Production.UI
{
    public sealed class ProducerTooltipSectionLine : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text changeRateText;

        [SerializeField, Required]
        private Image goodIcon;

        private GoodResources _goodResources;

        private void Awake()
        {
            _goodResources = ResourceManager.Instance.GoodResources;
        }

        public void SetUp(ProducerTooltipSection.Data data)
        {
            var resource = _goodResources.ResourceData[data.Good];

            changeRateText.text = $"{data.ChangeRate:+0.#;-0.#;0} {resource.GoodName}";
            goodIcon.sprite = resource.Icon;
        }
    }
}