using System;
using System.Collections.Generic;
using Common.Config;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Popups;
using Common.UI.Utility;
using Common.Utility;
using Features.Goods.Config;
using Features.Goods.Recipe.Data;
using Features.Player.Logic;
using Features.Towns.Production.Config;
using Features.Towns.Production.Logic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Production.UI.Construction
{
    public sealed class Tier1ConstructionPopup : Popup
    {
        [SerializeField, Required]
        private Transform goodGroupParent;

        [SerializeField, Required]
        private GameObject goodGroupPrefab;

        [SerializeField, Required]
        private Button costButton;

        private readonly Lazy<RecipeResources> _recipeResources = new(() => ResourceManager.Instance.RecipeResources);
        private readonly Lazy<GoodResources> _goodResources = new(() => ResourceManager.Instance.GoodResources);
        private readonly Lazy<PlayerModel> _player = new(() => GameplayContext.Instance.Model.Player);

        private readonly Lazy<ProducerConfig> _producerConfig =
            new(() => ConfigurationManager.Configurations.ProducerConfig);

        private readonly Dictionary<Tier1ConstructionElement, Action> _clickHandlers = new();

        private Tier1ConstructionElement _selectedElement;
        private Town _town;
        private float _cost = -1;
        private float _lastPlayerFunds;

        private void OnPlayerFundsChanged(float playerFunds)
        {
            if (_town == null || _cost < 0)
            {
                Debug.LogError($"{nameof(Tier1ConstructionPopup)} shouldn't observe player right now. No town set up.");
                return;
            }

            _lastPlayerFunds = playerFunds;
            UpdateButtonState();
        }

        public void Setup(Town town, int cellIndex)
        {
            Unbind();
            Bind(town, cellIndex);
        }

        private void Bind(Town town, int cellIndex)
        {
            _town = town;

            var productionBuildingCount = _town.ProductionManager.GetProducerCount(Tier.Tier1);
            var baseCost = _producerConfig.Value.GetUpgradeCost(Tier.Tier1, productionBuildingCount);
            if (baseCost == null)
            {
                Debug.LogError($"The town has no more empty building slots for {Tier.Tier1}.");
                return;
            }

            var modifierSum = _town.ProductionManager.ProductionBuildingCostModifiers;
            _cost = baseCost.Value * (1 + modifierSum);

            SetUpButton(town, cellIndex);
            SpawnElements(town);

            _player.Value.Inventory.Funds.Observe(OnPlayerFundsChanged);
        }

        private void SetUpButton(Town town, int cellIndex)
        {
            // disabled on start, since no element will be selected
            costButton.interactable = false;
            costButton.GetText().text = _cost.ToString("N0");

            costButton.onClick.AddListener(() =>
            {
                if (_selectedElement == null) return;
                town.AddProduction(_selectedElement.Tier1Good, cellIndex);
                _player.Value.Inventory.RemoveFunds(_cost);
                Close();
            });
        }

        private void SpawnElements(Town town)
        {
            var initialSelectionFound = false;
            foreach (var good in town.AvailableGoods)
            {
                if (_goodResources.Value.ResourceData[good].Tier != Tier.Tier1)
                    continue;

                if (town.ProductionManager.IsProduced(good))
                    continue;

                var element = SpawnElement(good);

                // select the first producer element that isn't built yet
                if (initialSelectionFound)
                    continue;

                PopupGroupOnClicked(element);
                initialSelectionFound = true;
            }
        }

        private Tier1ConstructionElement SpawnElement(Good good)
        {
            var tier2Good = _recipeResources.Value.GetTier2RecipeForComponent(good).Result;
            var goodGroup = Instantiate(goodGroupPrefab, goodGroupParent);
            var element = goodGroup.GetComponent<Tier1ConstructionElement>();
            element.Setup(good, tier2Good);

            Action popupGroupClickHandler = () => PopupGroupOnClicked(element);
            element.Clicked += popupGroupClickHandler;
            _clickHandlers.Add(element, popupGroupClickHandler);
            return element;
        }

        private void Unbind()
        {
            _selectedElement = null;

            if (_town == null)
                return;

            _player.Value.Inventory.Funds.StopObserving(OnPlayerFundsChanged);
            costButton.onClick.RemoveAllListeners();

            foreach (var (group, handler) in _clickHandlers)
            {
                group.Clicked -= handler;
            }

            _clickHandlers.Clear();
            goodGroupParent.DestroyChildren();
        }

        private void PopupGroupOnClicked(Tier1ConstructionElement constructionElement)
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
        }
    }
}