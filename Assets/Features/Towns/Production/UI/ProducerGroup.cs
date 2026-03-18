using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements.Cells;
using Common.Utility;
using Features.Goods;
using Features.Goods.Config;
using Features.Goods.Recipe.Data;
using Features.Player.Logic;
using Features.Towns.Production.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Towns.Production.UI
{
    public sealed class ProducerGroup : MonoBehaviour
    {
        public event Action<ProductionCell, Tier> UpgradeButtonClicked;

        [SerializeField, Required]
        private GameObject unavailableGroup;

        [SerializeField, Required]
        private ProductionCell t1Cell, t2Cell, t3Cell;

        [SerializeField, Required]
        private InventoryCell deliveryCell;

        [SerializeField, Required]
        private GameObject arrowT1T2, arrowT2T3, arrowT2T3Delivery, notHereBlockerGroup;

        [SerializeField, Required]
        private TMP_Text titleText;

        [SerializeField]
        private LocalizedString emptyProducerString;

        private readonly Dictionary<Tier, ProductionCell> _producerCellsPerTier = new();
        private readonly Dictionary<Good, ProductionCell> _producerCellsPerGood = new();

        private RecipeResources _recipeResources;
        private GoodResources _goodResources;
        private PlayerLocation _playerLocation;
        private GoodPool _globalGoodPool;
        private Town _town;
        private ProductionManager _productionManager;
        private int _producerIndex;
        private bool _isAvailable;

        public void Initialize(int producerIndex)
        {
            _recipeResources = ResourceManager.Instance.RecipeResources;
            _goodResources = ResourceManager.Instance.GoodResources;
            _playerLocation = GameplayContext.Instance.Model.Player.Location;
            _globalGoodPool = GameplayContext.Instance.Model.GoodPool;

            _producerIndex = producerIndex;

            _producerCellsPerTier.Add(Tier.Tier1, t1Cell);
            _producerCellsPerTier.Add(Tier.Tier2, t2Cell);
            _producerCellsPerTier.Add(Tier.Tier3, t3Cell);

            foreach (var (tier, cell) in _producerCellsPerTier)
            {
                cell.Index = _producerIndex;
                cell.UnlockButtonClicked += () => UpgradeButtonClicked?.Invoke(cell, tier);
            }
        }

        public void Bind(Town town, bool isAvailable)
        {
            _town = town;
            _productionManager = _town.ProductionManager;

            _isAvailable = isAvailable;
            unavailableGroup.SetActive(!isAvailable);
            titleText.text = emptyProducerString.GetLocalizedString(new { _int_ProducerIndex = _producerIndex + 1 });

            _playerLocation.CurrentTown.Observe(OnPlayerTownChanged);

            if (!_isAvailable)
                return;

            _productionManager.ProductionAddedIndexed.Observe(OnProducerAdded);
            foreach (var tier in EnumExtensions.Enumerate<Tier>())
            {
                var producer = _productionManager.GetProducers(tier)[_producerIndex];
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

            _town.Tier.Observe(OnTownTierChanged);

            RefreshProducerCellStates();
            RefreshArrows();
        }

        public void Unbind()
        {
            if (_town != null)
            {
                _town.Tier.StopObserving(OnTownTierChanged);
                _town.Inventory.GoodUpdated -= OnGoodUpdated;
                _productionManager.ProductionAddedIndexed.StopObserving(OnProducerAdded);
            }

            if (_playerLocation != null)
            {
                _playerLocation.CurrentTown.StopObserving(OnPlayerTownChanged);
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

        public ProductionCell GetCell(Good good)
        {
            return _producerCellsPerGood.GetValueOrDefault(good, null);
        }

        public ProductionCell GetCell(Tier tier)
        {
            return _producerCellsPerTier[tier];
        }

        private void OnTownTierChanged(Tier tier)
        {
            RefreshProducerCellStates();
            RefreshArrows();
        }

        // phewww...
        private void RefreshProducerCellStates()
        {
            var townTier = _town.Tier.Value;
            foreach (var (tier, cell) in _producerCellsPerTier)
            {
                if (townTier < tier)
                {
                    cell.SetState(ProductionCell.State.Hidden);
                    continue;
                }

                if (_productionManager.HasProducer(tier, _producerIndex))
                {
                    cell.SetState(ProductionCell.State.Active);
                    continue;
                }

                RefreshEmptyProducerCell(tier, cell);
            }

            ToggleDeliveryCell(_productionManager.HasProducer(Tier.Tier3, _producerIndex));
        }

        // Cell without an active producer in it.
        private void RefreshEmptyProducerCell(Tier tier, ProductionCell cell)
        {
            switch (tier)
            {
                case Tier.Tier1:
                    cell.SetState(ProductionCell.State.Upgradeable);
                    break;
                case Tier.Tier2:
                {
                    var hasT1Producer = _productionManager.HasProducer(Tier.Tier1, _producerIndex);
                    cell.SetState(hasT1Producer
                        ? ProductionCell.State.Upgradeable
                        : ProductionCell.State.Locked);
                    break;
                }
                case Tier.Tier3:
                {
                    var t2Producer = _productionManager.GetProducers(tier - 1)
                        .ElementAt(_producerIndex);
                    var hasT2Producer = t2Producer != null;
                    if (!hasT2Producer)
                    {
                        cell.SetState(ProductionCell.State.Locked);
                        break;
                    }

                    var t3Recipes = _recipeResources
                        .GetTier3RecipeForComponent(t2Producer.ProducedGood);

                    var globallyAvailableT3Goods = _globalGoodPool.Tier3Goods.ToList();
                    var recipes = t3Recipes.Where(recipe => globallyAvailableT3Goods.Contains(recipe.Result));

                    var doesAnyRecipeExist = recipes.Any(recipe => !_productionManager.IsProduced(recipe.Result));

                    cell.SetState(doesAnyRecipeExist
                        ? ProductionCell.State.Upgradeable
                        : ProductionCell.State.MissingRecipes);
                    break;
                }
            }
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
            {
                // need to refresh in case one producer group built the only available T3 producer of this group
                RefreshProducerCellStates();
                return;
            }


            var isHighestTierProducer = !_town.ProductionManager.HasProducer(producer.Tier + 1, producerIndex);
            if (producer.Tier == Tier.Tier3 || isHighestTierProducer)
            {
                titleText.text = _goodResources.ResourceData[producer.ProducedGood].BuildingName;
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

        private void OnPlayerTownChanged(Town town)
        {
            notHereBlockerGroup.SetActive(_town != town);
        }
    }
}