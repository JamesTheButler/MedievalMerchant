using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Trade.Haggling.UI
{
    public sealed class HaggleGroup : MonoBehaviour
    {
        public event Action<HaggleLevel> HaggleLevelChanged;

        [SerializeField]
        private List<HaggleToggle> toggles;

        private HaggleLevel _selectedLevel;

        public void SetUp(HaggleLevel initialHaggleLevel, TradeType tradeType)
        {
            _selectedLevel = initialHaggleLevel;

            foreach (var toggle in toggles)
            {
                toggle.SetUp(tradeType);
                toggle.Selected += OnToggleClicked;
                toggle.Toggle(toggle.HaggleLevel == _selectedLevel);
            }
        }

        private void OnToggleClicked(HaggleLevel level)
        {
            _selectedLevel = level;
            HaggleLevelChanged?.Invoke(level);

            foreach (var toggle in toggles)
            {
                toggle.Toggle(toggle.HaggleLevel == _selectedLevel);
            }
        }
    }
}