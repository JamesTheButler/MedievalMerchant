using System;
using System.Collections.Generic;
using Common;
using Data;
using Data.Configuration;
using Data.Goods.Recipes.Config;
using Data.Player;
using Data.Towns;
using Data.Towns.Production.Config;
using Data.Towns.Production.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public sealed class Tier3ConstructionPopup : Popup
    {
        [SerializeField]
        private Transform goodGroupParent;

        [SerializeField]
        private GameObject goodGroupPrefab;

        [SerializeField]
        private Button costButton;

        private readonly Lazy<RecipeConfig> _recipeConfig = new(() => ConfigurationManager.Instance.RecipeConfig);
        private readonly Lazy<Colors> _colors = new(() => ConfigurationManager.Instance.Colors);
        private readonly Lazy<PlayerModel> _player = new(() => Model.Instance.Player);
        private readonly Lazy<ProducerConfig> _producerConfig = new(() => ConfigurationManager.Instance.ProducerConfig);

        private readonly Dictionary<Tier3UpgradePathElement, Action> _clickHandlers = new();

        private Tier3UpgradePathElement _selectedElement;
        private Town _town;
        private float _cost = -1;

        private void OnPlayerFundsChanged(float playerFunds)
        {
            if (_town == null || _cost < 0)
            {
                Debug.LogError(
                    $"{nameof(Tier3UpgradePathElement)} shouldn't observe player right now. No town set up.");
                return;
            }

            var isInteractable = playerFunds >= _cost;
            UpdateButtonState(isInteractable);
        }

        private void UpdateButtonState(bool isInteractable)
        {
            // button state is right already
            if (costButton.interactable == isInteractable)
                return;

            costButton.interactable = isInteractable;
            costButton.GetText().color = isInteractable ? _colors.Value.FontDark : _colors.Value.Bad;
        }

        public void Setup(Town town, int cellIndex)
        {
            Unbind();
            Bind(town, cellIndex);
        }

        private void Bind(Town town, int cellIndex)
        {
            _town = town;
            // disabled on start, since no element will be selected
            costButton.interactable = false;

            var productionBuildingCount = _town.ProductionManager.GetProducerCount(Tier.Tier3);
            var cost = _producerConfig.Value.GetUpgradeCost(Tier.Tier3, productionBuildingCount);
            if (cost == null)
            {
                Debug.LogError($"The town has no more empty building slots for {Tier.Tier3}.");
                return;
            }

            _cost = cost.Value;

            costButton.GetText().text = _cost.ToString("N0");

            // TODO: disable button if no group is selected
            costButton.onClick.AddListener(() =>
            {
                if (_selectedElement == null) return;
                town.AddProduction(_selectedElement.Tier3Good, cellIndex);
                _player.Value.Inventory.RemoveFunds(_cost);
                Hide();
            });

            _player.Value.Inventory.Funds.Observe(OnPlayerFundsChanged);

            var producers = town.ProductionManager.GetProducers(Tier.Tier2);
            var tier2Producer = producers[cellIndex];
            if (tier2Producer == null)
                return;

            var tier2Component1 = tier2Producer.ProducedGood;
            var recipeConfig = _recipeConfig.Value;
            var tier3Recipes = recipeConfig.GetTier3RecipeForComponent(tier2Component1);

            foreach (var recipe in tier3Recipes)
            {
                var tier2Component2 = recipe.GetOther(tier2Component1);
                var tier1Component1 = recipeConfig.GetTier2RecipeForResult(tier2Component1).Component;
                var tier1Component2 = recipeConfig.GetTier2RecipeForResult(tier2Component2).Component;

                var goodGroup = Instantiate(goodGroupPrefab, goodGroupParent);
                var tier3Element = goodGroup.GetComponent<Tier3UpgradePathElement>();

                tier3Element.Setup(
                    tier1Component1,
                    tier2Component1,
                    tier1Component2,
                    tier2Component2,
                    recipe.Result);

                Action popupGroupClickHandler = () => PopupGroupOnClicked(tier3Element);
                tier3Element.Clicked += popupGroupClickHandler;
                _clickHandlers.Add(tier3Element, popupGroupClickHandler);
            }
        }

        private void Unbind()
        {
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

        private void PopupGroupOnClicked(Tier3UpgradePathElement constructionElement)
        {
            if (_selectedElement == constructionElement) return;

            if (_selectedElement)
            {
                _selectedElement.Deselect();
            }

            constructionElement.Select();

            _selectedElement = constructionElement;
            costButton.interactable = true;
        }
    }
}