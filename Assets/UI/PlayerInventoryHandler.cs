using Data;
using UnityEngine;

namespace UI
{
    public class PlayerInventoryHandler : MonoBehaviour
    {
        [SerializeField]
        private InventoryUI inventoryUi;

        private Player _player;

        private void Start()
        {
            _player = Model.Instance.Player;

            inventoryUi.Bind(_player.Inventory);
            inventoryUi.Show();
        }

        private void OnDestroy()
        {
            inventoryUi.UnBind();
        }
    }
}