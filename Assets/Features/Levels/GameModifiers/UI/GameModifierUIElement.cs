using Common.Utility;
using Features.Levels.GameModifiers.Data;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Levels.GameModifiers.UI
{
    public class GameModifierUIElement : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text titleText, descriptionText, effectsText;

        private GameModifierData _activeModifierData;

        public void Setup(GameModifierData modifierData)
        {
            if (_activeModifierData == modifierData)
                return;

            _activeModifierData = modifierData;

            titleText.text = modifierData.Title;
            descriptionText.text = modifierData.Description;
            effectsText.text = modifierData.Effects
                .AggregateString(effect => $"- {effect.Description}\n");
        }
    }
}