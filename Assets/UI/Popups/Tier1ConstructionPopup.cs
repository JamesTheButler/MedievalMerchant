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
    public sealed class Tier1ConstructionPopup : Popup
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
            var cost = _producerConfig.Value.GetUpgradeCost(Tier.Tier1, productionBuildingCount);
            if (cost == null)
            {
                Debug.LogError($"The town has no more empty building slots for {Tier.Tier1}.");
                return;
            }

            _cost = cost.Value;

            SetUpButton(town, cellIndex);
            SpawnElements(town);

            _player.Value.Inventory.Funds.Observe(OnPlayerFundsChanged);
        }

        private void SetUpButton(Town town, int cellIndex)
        {
            // disabled on start, since no element will be selected
            costButton.interactable = false;
            costButton.GetText().text = _cost.ToString("N2");

            costButton.onClick.AddListener(() =>
            {
                if (_selectedElement == null) return;
                town.AddProduction(_selectedElement.Tier1Good, cellIndex);
                _player.Value.Inventory.RemoveFunds(_cost);
                Hide();
            });
        }

        private void SpawnElements(Town town)
        {
            var initialSelectionFound = false;
            foreach (var good in town.AvailableGoods)
            {
                var isAlreadyBuilt = town.ProductionManager.IsProduced(good);
                var element = SpawnElement(good, isAlreadyBuilt);

                // select the first producer element that isn't built yet
                if (isAlreadyBuilt || initialSelectionFound)
                    continue;

                PopupGroupOnClicked(element);
                initialSelectionFound = true;
            }
        }

        private Tier1ConstructionElement SpawnElement(Good good, bool isAlreadyBuilt)
        {
            var tier2Good = _recipeConfig.Value.GetTier2RecipeForComponent(good).Result;
            var goodGroup = Instantiate(goodGroupPrefab, goodGroupParent);
            var element = goodGroup.GetComponent<Tier1ConstructionElement>();
            element.Setup(good, tier2Good, isAlreadyBuilt);

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
            costButton.GetText().color = isInteractable ? _colors.Value.FontDark : _colors.Value.Bad;
        }
    }
}