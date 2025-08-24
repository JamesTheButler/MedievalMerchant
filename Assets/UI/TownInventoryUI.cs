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

            base.Bind(town.Inventory);

            _boundTown = town;
            TownUpgrade();
            _boundTown.TierChanged += TownUpgrade;

            upgradeButton.onClick.AddListener(() => _boundTown.Upgrade());
        }


        public void UnBindTown()
        {
            upgradeButton.onClick.RemoveAllListeners();

            if (_boundTown == null) return;

            _boundTown.TierChanged -= TownUpgrade;
            _boundTown = null;
        }

        private void TownUpgrade()
        {
            townNameText.text = $"{_boundTown.Name} ({_boundTown.Tier.ToRomanNumeral()})";

            foreach (var good in _boundTown.Production)
            {
                InventoryCells[good].SetIsProduced(true);
            }
        }
    }
}