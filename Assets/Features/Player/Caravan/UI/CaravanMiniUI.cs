using Common.UI.Utility;
using TMPro;
using UnityEngine;

namespace Features.Player.Caravan.UI
{
    public sealed class CaravanMiniUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text moveSpeedText, upkeepText;

        [SerializeField]
        private string defaultStyleTag, disabledStyleTag;

        private string _currentStyleTag;
        private float _cachedUpkeep;

        private void Awake()
        {
            _currentStyleTag = defaultStyleTag;
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            moveSpeedText.text = moveSpeed.ToString("0.#");
        }

        public void SetUpkeep(float upkeep)
        {
            _cachedUpkeep = upkeep;
            RefreshUpkeepText();
        }

        public void ToggleUpkeep(bool isEnabled)
        {
            _currentStyleTag = isEnabled ? defaultStyleTag : disabledStyleTag;
            RefreshUpkeepText();
        }

        private void RefreshUpkeepText()
        {
            upkeepText.text = _cachedUpkeep.ToString("0.#").WithStyle(_currentStyleTag);
        }
    }
}