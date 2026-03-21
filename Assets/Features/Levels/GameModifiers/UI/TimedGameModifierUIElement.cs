using Features.Localization.UI;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.GameModifiers.UI
{
    public sealed class TimedGameModifierUIElement : GameModifierUIElement
    {
        [SerializeField, Required]
        private LocalizedText timeText;

        public void SetTimeLeft(int timeLeft)
        {
            timeText.SetArgs(timeLeft);
        }
    }
}