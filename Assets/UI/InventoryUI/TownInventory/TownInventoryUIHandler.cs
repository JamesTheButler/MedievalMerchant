using Data.Towns;
using NaughtyAttributes;
using UnityEngine;

namespace UI.InventoryUI.TownInventory
{
    public sealed class TownInventoryUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private TownInventoryUI inventoryUi;

        private void Start()
        {
            inventoryUi.Hide();
            inventoryUi.Initialize();

            Selection.Instance.TownSelected += SelectTown;
        }

        private void SelectTown(Town town)
        {
            if (town == null)
            {
                DeselectTown();
                return;
            }

            inventoryUi.Show();
            inventoryUi.Bind(town);
        }

        private void DeselectTown()
        {
            inventoryUi.Unbind();
            inventoryUi.Hide();
        }
    }
}