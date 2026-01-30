using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Goods.Recipe.Data;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Goods.Recipe.UI
{
    public sealed class RecipePanelTier2RegionGroup : MonoBehaviour
    {
        [SerializeField, Required]
        private Image icon;

        [SerializeField, Required]
        private TMP_Text title;

        [SerializeField, Required]
        private GameObject container;

        [SerializeField, Required]
        private RecipePanelTier2Group recipeItemPrefab;

        public void SetUp(Region region, Tier2Recipe[] recipes)
        {
            container.DestroyChildren();
            var regionResources = ResourceManager.Instance.RegionResources.Data[region];
            icon.sprite = regionResources.Icon;
            title.text = regionResources.Name;

            container.DestroyChildren();
            foreach (var recipe in recipes)
            {
                var recipeItem = Instantiate(recipeItemPrefab, container.transform);
                recipeItem.Setup(recipe.Component, recipe.Result);
            }
        }
    }
}