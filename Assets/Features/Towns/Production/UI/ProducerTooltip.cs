using System.Linq;
using Common.Types;
using Common.UI.Tooltips;
using Features.Towns.Production.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns.Production.UI
{
    public sealed class ProducerTooltip : TooltipBase<ProducerTooltip.Data>
    {
        public sealed record Data(Town Town, int ProducerIndex);

        [SerializeField, Required]
        private ProducerTooltipSection tier1Section, tier2Section, tier3Section;

        [SerializeField, Required]
        private GameObject divider1, divider2;

        protected override void UpdateUI(Data data)
        {
            var producers = data.Town.ProductionManager.GetProducers(data.ProducerIndex);

            divider1.SetActive(producers[Tier.Tier2] != null);
            divider2.SetActive(producers[Tier.Tier3] != null);

            SetUpSection(tier1Section, producers[Tier.Tier1]);
            SetUpSection(tier2Section, producers[Tier.Tier2]);
            SetUpSection(tier3Section, producers[Tier.Tier3]);
        }

        public override void Reset()
        {
            tier1Section.SetActive(false);
            tier2Section.SetActive(false);
            tier3Section.SetActive(false);
        }

        private static void SetUpSection(ProducerTooltipSection section, Producer producer)
        {
            section.SetActive(producer != null);
            if (producer == null)
                return;

            var productionData = new ProducerTooltipSection.Data(producer.ProducedGood, producer.ProductionRate);
            var consumptionDatas = producer.IngredientConsumptionRates
                .Select(kvPair => new ProducerTooltipSection.Data(kvPair.Key, -kvPair.Value))
                .ToArray();

            section.SetUp(productionData, consumptionDatas);
        }
    }
}