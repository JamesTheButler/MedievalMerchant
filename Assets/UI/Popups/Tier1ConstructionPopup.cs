using System;
using System.Linq;
using Common;
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

        public void Setup(Town town, int cost)
        {
            Clear();
            costButton.GetText().text = cost.ToString("N0");

            var recipes = _recipeConfig.Value.Tier2Recipes;
            foreach (var good in town.AvailableGoods)
            {
                var tier2Good = recipes[good];
                var isAlreadyBuilt = town.Production.Contains(good);

                var goodGroup = Instantiate(goodGroupPrefab, goodGroupParent);
                goodGroup.GetComponent<BuildTier1PopupGroup>().Setup(good, tier2Good, isAlreadyBuilt);
            }
        }

        public void Clear()
        {
            goodGroupParent.DestroyChildren();
        }
    }
}