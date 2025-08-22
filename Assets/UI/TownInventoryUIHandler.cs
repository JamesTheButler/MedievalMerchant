using Data;
using UnityEngine;

namespace UI
{
    public sealed class TownInventoryUIHandler : MonoBehaviour
    {
        [SerializeField]
        private TownInventoryUI inventoryUi;

        public void SelectTown(Town town)
        {
            inventoryUi.Bind(town);
            inventoryUi.Show();
        }

        public void DeselectTown()
        {
            inventoryUi.UnBindTown();
            inventoryUi.Hide();
        }
    }
}