using System;
using System.Collections.Generic;
using System.Linq;
using Common.Config;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Popups;
using Common.UI.Utility;
using Common.Utility;
using Features.Goods.Config;
using Features.Player.Logic;
using Features.Towns.Production.Config;
using Features.Towns.Production.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Production.UI
{
    public sealed class Tier3ConstructionPopup : Popup
    {
        [SerializeField]
        private Transform goodGroupParent;

        [SerializeField]
        private GameObject goodGroupPrefab;

        [SerializeField]
        private Button costButton;

        private readonly Lazy<RecipeResources> _recipeConfig = new(() => ResourceManager.Instance.RecipeResources);
        private readonly Lazy<Colors> _colors = new(() => ResourceManager.Instance.Colors);
        private readonly Lazy<PlayerModel> _player = new(() => GameplayContext.Instance.Model.Player);

        private readonly Lazy<ProducerConfig> _producerConfig =
            new(() => ConfigurationManager.Configurations.ProducerConfig);

        private readonly Dictionary<Tier3UpgradePathElement, Action> _clickHandlers = new();

        private Tier3UpgradePathElement _selectedElement;
        private Town _town;
        private float _cost = -1;
        private float _lastPlayerFunds;

        public void Setup(Town town, int cellIndex)
        {
            Unbind();
            Bind(town, cellIndex);
        }

        private void Bind(Town town, int cellIndex)
        {
            _town = town;

            var productionBuildingCount = _town.ProductionManager.GetProducerCount(Tier.Tier3);
            var cost = _producerConfig.Value.GetUpgradeCost(Tier.Tier3, productionBuildingCount);
            if (cost == null)
            {
                Debug.LogError($"The town has no more empty building slots for {Tier.Tier3}.");
                return;
            }

            _cost = cost.Value;

            SetUpButton(town, cellIndex);
            SpawnElements(town, cellIndex);

            _player.Value.Inventory.Funds.Observe(OnPlayerFundsChanged);
        }

        private void OnPlayerFundsChanged(float playerFunds)
        {
            if (_town == null)
            {
                Debug.LogError(
                    $"{nameof(Tier3UpgradePathElement)} shouldn't observe player right now. No town set up.");
                return;
            }

            _lastPlayerFunds = playerFunds;
            UpdateButtonState();
        }

        private void SetUpButton(Town town, int cellIndex)
        {
            // disabled on start, since no element will be selected
            costButton.interactable = false;
            costButton.GetText().text = _cost.ToString("N0");

            costButton.onClick.AddListener(() =>
            {
                if (_selectedElement == null) return;
                town.AddProduction(_selectedElement.Tier3Good, cellIndex);
                _player.Value.Inventory.RemoveFunds(_cost);
                Hide();
            });
        }

        private void SpawnElements(Town town, int cellIndex)
        {
            var producers = town.ProductionManager.GetProducers(Tier.Tier2);
            var tier2Producer = producers[cellIndex];
            if (tier2Producer == null)
                return;

            var primaryTier2Good = tier2Producer.ProducedGood; // T2 good that was clicked in ui that is shown first
            var tier3Recipes = _recipeConfig.Value.GetTier3RecipeForComponent(primaryTier2Good);
            var initialSelectionFound = false;

            var globalGoodPool = GameplayContext.Instance.Model.GoodPool;
            var globallyAvailableT3Goods = globalGoodPool.Tier3Goods;
            foreach (var recipe in tier3Recipes)
            {
                if (!globallyAvailableT3Goods.Contains(recipe.Result))
                    continue;

                var isAlreadyBuilt = town.ProductionManager.IsProduced(recipe.Result);
                var element = SpawnElement(recipe, primaryTier2Good);

                // select the first producer element that isn't built yet
                if (isAlreadyBuilt || initialSelectionFound)
                    continue;

                PopupGroupOnClicked(element);
                initialSelectionFound = true;
            }
        }

        private Tier3UpgradePathElement SpawnElement(Tier3Recipe recipe, Good tier2Component1)
        {
            var tier2Component2 = recipe.GetOtherComponent(tier2Component1);
            var tier1Component1 = _recipeConfig.Value.GetTier2RecipeForResult(tier2Component1).Component;
            var tier1Component2 = _recipeConfig.Value.GetTier2RecipeForResult(tier2Component2).Component;
            var tier3Result = recipe.Result;

            var goodGroup = Instantiate(goodGroupPrefab, goodGroupParent);
            var element = goodGroup.GetComponent<Tier3UpgradePathElement>();

            var productionMgr = _town.ProductionManager;

            element.Setup(
                new ConstructionCellData(tier1Component1, productionMgr.IsProduced(tier1Component1)),
                new ConstructionCellData(tier2Component1, productionMgr.IsProduced(tier2Component1)),
                new ConstructionCellData(tier1Component2, productionMgr.IsProduced(tier1Component2)),
                new ConstructionCellData(tier2Component2, productionMgr.IsProduced(tier2Component2)),
                new ConstructionCellData(tier3Result, productionMgr.IsProduced(tier3Result)));

            Action popupGroupClickHandler = () => PopupGroupOnClicked(element);
            element.Clicked += popupGroupClickHandler;
            _clickHandlers.Add(element, popupGroupClickHandler);
            return element;
        }

        private void Unbind()
        {
            _selectedElement = null;

            _player.Value.Inventory.Funds.StopObserving(OnPlayerFundsChanged);
            costButton.onClick.RemoveAllListeners();

            foreach (var (group, handler) in _clickHandlers)
            {
                group.Clicked -= handler;
            }

            _clickHandlers.Clear();
            goodGroupParent.DestroyChildren();
        }

        private void PopupGroupOnClicked(Tier3UpgradePathElement constructionElement)
        {
            if (_selectedElement == constructionElement) return;

            if (_selectedElement)
            {
                _selectedElement.Deselect();
            }

            constructionElement.Select();

            _selectedElement = constructionElement;
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            var isInteractable = _lastPlayerFunds >= _cost && _selectedElement;
            // button state is right already
            if (costButton.interactable == isInteractable)
                return;

            costButton.interactable = isInteractable;
            costButton.GetText().color = isInteractable ? _colors.Value.FontDark : _colors.Value.Bad;
        }
    }
}