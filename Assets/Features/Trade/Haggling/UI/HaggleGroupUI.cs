using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Trade.Haggling.UI
{
    public sealed class HaggleGroupUI : MonoBehaviour
    {
        public event Action<HaggleLevel> HaggleLevelChanged;

        [SerializeField]
        private List<HaggleToggle> toggles;

        private HaggleLevel _selectedLevel = HaggleLevel.Fair;

        private void Awake()
        {
            foreach (var toggle in toggles)
            {
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