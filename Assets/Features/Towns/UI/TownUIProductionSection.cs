using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Features.Goods.Config;
using Features.Trade;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Towns.UI
{
    public sealed class TownUIProductionSection : TownUISection
    {
        [SerializeField]
        private UnityEvent<ProductionCell, TradeType> productionCellClicked;

        [SerializeField]
        private UnityEvent<ProductionCell> tier1UpgradeButtonClicked;

        [SerializeField]
        private UnityEvent<ProductionCell> tier2UpgradeButtonClicked;

        [SerializeField]
        private UnityEvent<ProductionCell> tier3UpgradeButtonClicked;

        private ProducerGroup[] _producerGroups;
        private GoodsResources _goodResources;

        public override void Initialize()
        {
            _goodResources = ResourceManager.Instance.GoodsResources;

            _producerGroups = GetComponentsInChildren<ProducerGroup>();
            
            for (var index = 0; index < _producerGroups.Length; index++)
            {
                var group = _producerGroups[index];
                group.Initialize(index);
                group.UpgradeButtonClicked += OnUpgradeButtonClicked;
                group.ProductionCellClicked += OnProductionCellClicked;
            }
        }

        public override void CleanUp() { }

        public override void Bind(Town town)
        {
            var availableTier1GoodsInTown = town.AvailableGoods
                .Count(good => _goodResources.ResourceData[good].Tier == Tier.Tier1);

            for (var index = 0; index < _producerGroups.Length; index++)
            {
                var group = _producerGroups[index];
                group.Bind(town, index < availableTier1GoodsInTown);
            }
        }

        public override void Unbind(Town town)
        {
            foreach (var group in _producerGroups)
            {
                group.Unbind();
            }
        }

        private void OnUpgradeButtonClicked(ProductionCell productionCell, Tier tier)
        {
            switch (tier)
            {
                case Tier.Tier1:
                    tier1UpgradeButtonClicked.Invoke(productionCell);
                    break;
                case Tier.Tier2:
                    tier2UpgradeButtonClicked.Invoke(productionCell);
                    break;
                case Tier.Tier3:
                    tier3UpgradeButtonClicked.Invoke(productionCell);
                    break;
            }
        }

        private void OnProductionCellClicked(ProductionCell productionCell)
        {
            productionCellClicked.Invoke(productionCell, TradeType.Buy);
        }
    }
}