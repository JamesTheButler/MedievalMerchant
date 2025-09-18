using System;
using System.Collections.Generic;
using Common;
using Data;
using Data.Configuration;
using Data.Towns;
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
        private readonly Lazy<Player> _player = new(() => Model.Instance.Player);
        private readonly Lazy<ProducerConfig> _producerConfig = new(() => ConfigurationManager.Instance.ProducerConfig);

        private readonly Dictionary<BuildTier1PopupGroup, Action> _clickHandlers = new();

        private BuildTier1PopupGroup _selectedGroup;
        private Town _town;
        private int _cost;

        private void OnPlayerFundsChanged(int playerFunds)
        {
            if (_town == null)
            {
                Debug.LogError($"{nameof(Tier1ConstructionPopup)} shouldn't observe player right now. No town set up.");
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

            var productionBuildingCount = _town.Producer.GetProducerCount(Tier.Tier1);
            var cost = _producerConfig.Value.GetUpgradeCost(Tier.Tier1, productionBuildingCount);
            if (cost == null)
            {
                Debug.LogError($"The town has no more empty building slots for {Tier.Tier1}.");
                return;
            }

            _cost = cost.Value;

            costButton.GetText().text = _cost.ToString("N0");

            // TODO: disable button if no group is selected
            costButton.onClick.AddListener(() =>
            {
                if (_selectedGroup == null) return;
                town.AddProduction(_selectedGroup.Tier1Good, cellIndex);
                _player.Value.Inventory.RemoveFunds(_cost);
                Hide();
            });

            _player.Value.Inventory.Funds.Observe(OnPlayerFundsChanged);

            var recipes = _recipeConfig.Value.Tier2Recipes;
            foreach (var good in town.AvailableGoods)
            {
                var tier2Good = recipes[good];
                var isAlreadyBuilt = town.Producer.IsProduced(good);
                var goodGroup = Instantiate(goodGroupPrefab, goodGroupParent);
                var popupGroup = goodGroup.GetComponent<BuildTier1PopupGroup>();
                popupGroup.Setup(good, tier2Good, isAlreadyBuilt);

                Action popupGroupClickHandler = () => PopupGroupOnClicked(popupGroup);
                popupGroup.Clicked += popupGroupClickHandler;
                _clickHandlers.Add(popupGroup, popupGroupClickHandler);
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

        private void PopupGroupOnClicked(BuildTier1PopupGroup popupGroup)
        {
            if (_selectedGroup == popupGroup) return;
            if (_selectedGroup)
            {
                _selectedGroup.Deselect();
            }

            popupGroup.Select();

            _selectedGroup = popupGroup;
        }
    }
}