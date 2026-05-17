using Common.Config;
using Common.Infrastructure;
using Common.Types;
using Features.Goods.Config;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Production.UI
{
    public sealed class ProducerTooltipSection : MonoBehaviour
    {
        public sealed record Data(Good Good, float ChangeRate);

        [SerializeField, Required]
        private TMP_Text buildingTitle;

        [SerializeField, Required]
        private Image goodIcon, tierIcon;

        [SerializeField, Required]
        private ProducerTooltipSectionLine producerLine;

        [SerializeField]
        private ProducerTooltipSectionLine[] consumerLines;

        [SerializeField, Required]
        private GameObject divider;

        private GoodResources _goodResources;
        private TierResources _tierResources;

        private void Awake()
        {
            _goodResources = ResourceManager.Instance.GoodResources;
            _tierResources = ResourceManager.Instance.TierResources;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetUp(Data production, params Data[] consumptionDatas)
        {
            var resource = _goodResources.ResourceData[production.Good];
            goodIcon.sprite = resource.Icon;
            tierIcon.sprite = _tierResources.Icons[resource.Tier];
            buildingTitle.text = resource.BuildingName;

            producerLine.SetUp(production);
            divider.SetActive(consumptionDatas.Length > 0);

            // set up used consumption lines
            for (var i = 0; i < consumptionDatas.Length; i++)
            {
                var line = consumerLines[i];
                var data = consumptionDatas[i];

                line.gameObject.SetActive(true);
                line.SetUp(data);
            }

            // disable all unused consumption lines
            for (var j = consumptionDatas.Length; j < consumerLines.Length; j++)
            {
                var line = consumerLines[j];
                line.gameObject.SetActive(false);
            }
        }
    }
}