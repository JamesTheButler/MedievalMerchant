using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Player.Retinue.UI
{
    public sealed class CompanionLevelUI : MonoBehaviour
    {
        public enum State
        {
            Locked,
            Unlockable,
            Unlocked,
        }

        [SerializeField, Required]
        private CompanionLevelTooltipHandler tooltip;

        [SerializeField, Required]
        private Sprite defaultIcon, completedIcon;

        [SerializeField, Required]
        private Image levelIcon;

        private CompanionType _companionType;
        private int _level;

        public void Setup(int levelIndex, CompanionType companionType)
        {
            _level = levelIndex;
            _companionType = companionType;
            SetState(State.Locked);
        }

        public void SetState(State state)
        {
            levelIcon.sprite = state == State.Unlocked ? completedIcon : defaultIcon;

            var tooltipData = CreateTooltipData(state);
            tooltip.SetData(tooltipData);
        }

        private CompanionLevelTooltip.Data CreateTooltipData(State state)
        {
            return new CompanionLevelTooltip.Data(
                _companionType,
                _level,
                state);
        }
    }
}
