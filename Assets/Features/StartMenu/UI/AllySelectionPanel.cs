using System;
using System.Collections.Generic;
using Common.Types;
using Features.Levels.GameModifiers.Effects.Data;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.StartMenu.UI
{
    public sealed class AllySelectionPanel : MonoBehaviour
    {
        [SerializeField]
        private List<AllySelectionToggle> toggles;

        [SerializeField, Required]
        private Button startButton;

        private Region? _selectedRegion;

        private AllyEffectData _allyEffectData;

        private void Awake()
        {
            foreach (var toggle in toggles)
            {
                toggle.Selected += OnToggleClicked;
                toggle.Toggle(false);
            }

            startButton.interactable = false;
        }

        public void SetUp(AllyEffectData effectData, Action startLevelCallback)
        {
            gameObject.SetActive(true);
            _allyEffectData = effectData;
            startButton.onClick.AddListener(startLevelCallback.Invoke);
        }

        private void OnToggleClicked(Region region)
        {
            _allyEffectData.SetRegion(region);
            foreach (var toggle in toggles)
            {
                toggle.Toggle(toggle.Region == region);
            }

            startButton.interactable = true;
        }
    }
}