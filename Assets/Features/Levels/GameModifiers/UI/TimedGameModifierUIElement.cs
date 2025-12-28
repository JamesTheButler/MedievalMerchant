using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Levels.GameModifiers.UI
{
    public sealed class TimedGameModifierUIElement : GameModifierUIElement
    {
        [SerializeField, Required]
        private TMP_Text timeText;

        public void SetTimeLeft(int timeLeft)
        {
            timeText.text = $"{timeLeft} days left";
        }
    }
}