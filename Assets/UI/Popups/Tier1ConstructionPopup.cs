using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common;
using Data;
using Data.Configuration;
using Data.Towns;
using Unity.VisualScripting;
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
        private readonly Dictionary<BuildTier1PopupGroup, Action> _clickHandlers = new();

        private BuildTier1PopupGroup _selectedGroup;

        private Town _town;

        public void Setup(Town town, int cost, int cellIndex)
        {
            Unbind();
            Bind(town, cost, cellIndex);
        }

        private void Bind(Town town, int cost, int cellIndex)
        {
            _town = town;

            costButton.GetText().text = cost.ToString("N0");

            // TODO: bind to town funds to update button clickability
            // TODO: disable button if no group is selected
            costButton.onClick.AddListener(() =>
            {
                if (_selectedGroup == null) return;
                town.AddProduction(_selectedGroup.Tier1Good, cellIndex);
                town.Inventory.RemoveFunds(cost);
                Hide();
            });

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