using Common.Types;
using Common.UI.Elements.Panels;
using Common.UI.Utility;
using Common.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Features.Goods.Recipe.UI
{
    public sealed class RecipePanelUI : DynamicPanel
    {
        [SerializeField, Required]
        private RecipePanelTier2Section tier2Section;

        [SerializeField, Required]
        private RecipePanelTier3Section tier3Section;

        [SerializeField, Required]
        private Button tierSelectorButton;

        [SerializeField]
        private LocalizedString tierString;

        private Tier? _selectedTier;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            SetUpRecipes();
            tierSelectorButton.onClick.AddListener(OnTierSelectorClicked);

            SetTier(Tier.Tier2);
        }

        private void OnTierSelectorClicked()
        {
            SetTier(_selectedTier == Tier.Tier3 ? Tier.Tier2 : Tier.Tier3);
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

            var args = new { Tier = tier.ToRomanNumeral() };
            tierSelectorButton.GetText().text = tierString.GetLocalizedString(args);
            tier2Section.gameObject.SetActive(tier == Tier.Tier2);
            tier3Section.gameObject.SetActive(tier == Tier.Tier3);

            _selectedTier = tier;
        }
    }
}