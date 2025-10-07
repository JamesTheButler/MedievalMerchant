using System;
using Common;
using Data;
using Data.Configuration;
using Data.Goods;
using Data.Goods.Recipes.Config;
using Data.Player;
using Data.Towns;
using Data.Towns.Production;
using Data.Towns.Production.Config;
using Data.Towns.Production.Logic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public sealed class Tier2ConstructionPopup : Popup
    {
        [SerializeField, Required]
        private Tier2ConstructionElement tier2ConstructionElement;

        [SerializeField, Required]
        private Button costButton;

        private readonly Lazy<RecipeConfig> _recipeConfig = new(() => ConfigurationManager.Instance.RecipeConfig);
        private readonly Lazy<Colors> _colors = new(() => ConfigurationManager.Instance.Colors);
        private readonly Lazy<PlayerModel> _player = new(() => Model.Instance.Player);
        private readonly Lazy<ProducerConfig> _producerConfig = new(() => ConfigurationManager.Instance.ProducerConfig);

        private Town _town;
        private Good _tier1Good;
        private Good _tier2Good;
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

            var productionBuildingCount = _town.ProductionManager.GetProducerCount(Tier.Tier2);
            var cost = _producerConfig.Value.GetUpgradeCost(Tier.Tier2, productionBuildingCount);
            if (cost == null)
            {
                Debug.LogError($"The town has no more empty building slots for {Tier.Tier2}.");
                return;
            }

            _cost = cost.Value;

            var producedTier1Good = _town.ProductionManager.GetProducers(Tier.Tier1)[cellIndex];
            if (producedTier1Good == null)
            {
                Debug.LogError($"Town {_town.Name} has no producer in slot {cellIndex}.");
                Hide();
                return;
            }

            _tier1Good = producedTier1Good.ProducedGood;
            _tier2Good = _recipeConfig.Value.GetTier2Recipe(_tier1Good).Result;

            tier2ConstructionElement.Setup(_tier1Good, _tier2Good);
            costButton.GetText().text = _cost.ToString("N0");

            costButton.onClick.AddListener(() =>
            {
                town.AddProduction(_tier2Good, cellIndex);
                _player.Value.Inventory.RemoveFunds(_cost);
                Hide();
            });

            _player.Value.Inventory.Funds.Observe(OnPlayerFundsChanged);
        }

        private void Unbind()
        {
            if (_town == null)
                return;

            _player.Value.Inventory.Funds.StopObserving(OnPlayerFundsChanged);
            costButton.onClick.RemoveAllListeners();
        }
    }
}