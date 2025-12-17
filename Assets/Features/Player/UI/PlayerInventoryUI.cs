using Common.UI;
using NaughtyAttributes;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Player
{
    public class PlayerInventoryUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField, Required]
        private TMP_Text fundsText;

        [SerializeField, Required]
        private ModifiableTooltipHandler fundsTooltip;

        private Features.Inventory.Inventory _playerInventory;

        public void Bind(PlayerModel player)
        {
            _playerInventory = player.Inventory;
            fundsTooltip.SetData(player.FundsChange);
            _playerInventory.Funds.Observe(OnFundsChanged);
        }

        public void Unbind()
        {
            _playerInventory.Funds.StopObserving(OnFundsChanged);
        }

        private void OnFundsChanged(float funds)
        {
            fundsText.text = funds.ToString("N0");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PopupManager.Instance.HideActive();
        }
    }
}