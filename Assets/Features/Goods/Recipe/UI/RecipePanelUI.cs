using Common.Types;
using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Recipe.UI
{
    public sealed class RecipePanelUI : DynamicPanel
    {
        [SerializeField, Required]
        private RecipePanelTier2Section tier2Section;

        [SerializeField, Required]
        private RecipePanelTier3Section tier3Section;

        private Tier? _selectedTier;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            SetUpRecipes();
            SetTier(Tier.Tier2);
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }

        private void SetUpRecipes()
        {
            tier2Section.Initialize();
            tier3Section.Initialize();
        }

        private void SetTier(Tier tier)
        {
            if (_selectedTier == tier) return;

            tier2Section.gameObject.SetActive(tier == Tier.Tier2);
            tier3Section.gameObject.SetActive(tier == Tier.Tier3);

            _selectedTier = tier;
        }
    }
}