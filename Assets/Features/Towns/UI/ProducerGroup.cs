using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Utility;
using Common.Utility;
using Features.Goods.Config;
using Features.Towns.Production.Config;
using Features.Towns.Production.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Towns.UI
{
    public sealed class ProducerGroup : MonoBehaviour
    {
        public event Action<ProductionCell, Tier> UpgradeButtonClicked;
        public event Action<GoodCell> ProductionCellClicked, DeliveryCellClicked;

        [SerializeField, Required]
        private GameObject unavailableGroup;

        [SerializeField, Required]
        private ProductionCell t1Cell, t2Cell, t3Cell;

        [SerializeField, Required]
        private InventoryCell deliveryCell;

        [SerializeField, Required]
        private GameObject arrowT1T2, arrowT2T3, arrowT2T3Delivery;

        [SerializeField, Required]
        private TMP_Text titleText;

        private readonly Dictionary<Tier, ProductionCell> _producerCellsPerTier = new();
        private readonly Dictionary<Good, ProductionCell> _producerCellsPerGood = new();

        private RecipeResources _recipeResources;
        private ProducerResources _producerResources;
        private Town _town;
        private int _producerIndex;
        private bool _isAvailable;

        public void Initialize(int producerIndex)
        {
            _recipeResources = ResourceManager.Instance.RecipeResources;
            _producerResources = ResourceManager.Instance.ProducerResources;

            _producerIndex = producerIndex;

            _producerCellsPerTier.Add(Tier.Tier1, t1Cell);
            _producerCellsPerTier.Add(Tier.Tier2, t2Cell);
            _producerCellsPerTier.Add(Tier.Tier3, t3Cell);

            foreach (var (tier, cell) in _producerCellsPerTier)
            {
                cell.Index = _producerIndex;
                cell.Clicked += () => ProductionCellClicked?.Invoke(cell);
                cell.UnlockButtonClicked += () => UpgradeButtonClicked?.Invoke(cell, tier);
            }

            deliveryCell.Clicked += () => DeliveryCellClicked?.Invoke(deliveryCell);
        }

        public void Bind(Town town, bool isAvailable)
        {
            _isAvailable = isAvailable;
            unavailableGroup.SetActive(!isAvailable);
            titleText.text = $"Producer {_producerIndex + 1}".WithStyle(Style.Subtitle);

            if (!_isAvailable)
                return;

            _town = town;

            _town.Tier.Observe(OnTownTierChanged);

            town.ProductionManager.ProductionAddedIndexed += OnProducerAdded;
            foreach (var tier in EnumExtensions.Enumerate<Tier>())
            {
                var producer = _town.ProductionManager.GetProducers(tier)[_producerIndex];
                if (producer != null)
                {
                    OnProducerAdded(producer, _producerIndex);
                }
            }

            _town.Inventory.GoodUpdated += OnGoodUpdated;
            foreach (var (good, amount) in town.Inventory.Goods)
            {
                OnGoodUpdated(good, amount);
            }

            RefreshProducerCellStates();
            RefreshArrows();
        }

        public void Unbind()
        {
            if (_town != null)
            {
                _town.Tier.StopObserving(OnTownTierChanged);
                _town.Inventory.GoodUpdated -= OnGoodUpdated;
                _town.ProductionManager.ProductionAddedIndexed -= OnProducerAdded;
            }

            arrowT1T2.SetActive(false);
            arrowT2T3.SetActive(false);
            arrowT2T3Delivery.SetActive(false);

            ToggleDeliveryCell(false);

            foreach (var productionCell in _producerCellsPerTier.Values)
            {
                productionCell.Reset();
                productionCell.SetState(ProductionCell.State.Hidden);
            }

            _producerCellsPerGood.Clear();

            _town = null;
        }

        private void OnTownTierChanged(Tier tier)
        {
            RefreshProducerCellStates();
            RefreshArrows();
        }

        private void RefreshProducerCellStates()
        {
            var townTier = _town.Tier.Value;
            foreach (var (tier, cell) in _producerCellsPerTier)
            {
                if (townTier < tier)
                {
                    cell.SetState(ProductionCell.State.Hidden);
                }
                else
                {
                    if (_town.ProductionManager.HasProducer(tier, _producerIndex))
                    {
                        cell.SetState(ProductionCell.State.Active);
                    }
                    else
                    {
                        var isUpgradable =
                            tier == Tier.Tier1 ||
                            _town.ProductionManager.HasProducer(tier - 1, _producerIndex);
                        cell.SetState(isUpgradable ? ProductionCell.State.Upgradeable : ProductionCell.State.Locked);
                    }
                }
            }

            ToggleDeliveryCell(_town.ProductionManager.HasProducer(Tier.Tier3, _producerIndex));
        }

        private void RefreshArrows()
        {
            var townTier = _town.Tier.Value;

            arrowT1T2.SetActive(townTier > Tier.Tier1);
            arrowT2T3.SetActive(townTier > Tier.Tier2);
            var hasT3Producer = _town.ProductionManager.HasProducer(Tier.Tier3, _producerIndex);
            arrowT2T3Delivery.SetActive(townTier > Tier.Tier2 && hasT3Producer);
        }

        private void OnProducerAdded(Producer producer, int producerIndex)
        {
            if (!_isAvailable)
                return;

            if (producerIndex != _producerIndex)
                return;

            if (producer.Tier == Tier.Tier3
                || !_town.ProductionManager.HasProducer(producer.Tier + 1, producerIndex))
            {
                titleText.text = _producerResources.producerNames[producer.ProducedGood];
            }

            var producerCell = _producerCellsPerTier[producer.Tier];
            producerCell.SetGood(producer.ProducedGood);
            producerCell.SetAmount(0);
            _producerCellsPerGood[producer.ProducedGood] = producerCell;
            RefreshProducerCellStates();
            RefreshArrows();

            if (producer.Tier == Tier.Tier3)
            {
                ToggleDeliveryCell(true);
            }
        }

        private void OnProducerRemoved(Producer producer)
        {
            if (!_producerCellsPerGood.Remove(producer.ProducedGood, out var producerCell))
                return;

            if (producer.Tier == Tier.Tier3)
            {
                ToggleDeliveryCell(false);
            }

            producerCell.Reset();

            RefreshProducerCellStates();
            RefreshArrows();
            _producerCellsPerGood.Remove(producer.ProducedGood);
        }

        private void OnGoodUpdated(Good good, int amount)
        {
            if (_producerCellsPerGood.TryGetValue(good, out var producerCell))
            {
                producerCell.SetAmount(amount);
            }

            if (deliveryCell.Good == good)
            {
                deliveryCell.SetAmount(amount);
            }
        }

        private void ToggleDeliveryCell(bool isEnabled)
        {
            var canvasGroup = deliveryCell.GetComponent<CanvasGroup>();
            canvasGroup.alpha = isEnabled ? 1 : 0;
            canvasGroup.blocksRaycasts = isEnabled;

            if (!isEnabled)
                return;

            // temporary, later this is useless (either it's hidden or not)
            var tier2Good = _producerCellsPerTier[Tier.Tier2].Good;
            var tier3Good = _producerCellsPerTier[Tier.Tier3].Good;
            if (tier2Good == null || tier3Good == null)
            {
                Debug.LogError($"Could not toggle cell on t2cell {tier2Good}, t3 cell {tier3Good}");
                return;
            }

            var t2GoodToDeliver = _recipeResources
                .GetTier3RecipeForResult(tier3Good.Value)
                .GetOtherComponent(tier2Good.Value);

            deliveryCell.SetGood(t2GoodToDeliver);
            deliveryCell.SetAmount(_town.Inventory.Get(t2GoodToDeliver));
        }
    }
}