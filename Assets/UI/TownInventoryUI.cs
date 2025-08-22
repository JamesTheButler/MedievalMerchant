using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class TownInventoryUI : InventoryUI
    {
        [SerializeField]
        private TMP_Text townNameText;

        [SerializeField]
        private Button upgradeButton;

        private Town _boundTown;

        public void Bind(Town town)
        {
            UnBindTown();

            if (town == null) return;

            _boundTown = town;
            UpdateTownTitle();
            _boundTown.TierChanged += UpdateTownTitle;

            upgradeButton.onClick.AddListener(() => _boundTown.Upgrade());

            base.Bind(town.Inventory);
        }


        public void UnBindTown()
        {
            upgradeButton.onClick.RemoveAllListeners();
            if (_boundTown != null)
            {
                _boundTown.TierChanged -= UpdateTownTitle;
                _boundTown = null;
            }

            UnBind();
        }

        private void UpdateTownTitle()
        {
            townNameText.text = $"{_boundTown.Name} ({_boundTown.Tier.ToRomanNumeral()})";
        }
    }
}