using Common.Utility;
using Features.Levels.GameModifiers.Data;
using TMPro;
using UnityEngine;

namespace Features.Levels.Conditions.UI
{
    public sealed class LevelConditionUiElement : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text titleText, descriptionText, effectsText;

        public void Setup(LevelGameModifierData levelModifierData)
        {
            titleText.text = levelModifierData.Title;
            descriptionText.text = levelModifierData.Description;
            effectsText.text = levelModifierData.Effects
                .AggregateString(effect => $"- {effect.Description}\n");
        }
    }
}