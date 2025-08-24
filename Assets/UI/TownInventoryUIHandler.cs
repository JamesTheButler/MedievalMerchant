using Data;
using UnityEngine;

namespace UI
{
    public sealed class TownInventoryUIHandler : MonoBehaviour
    {
        [SerializeField]
        private TownInventoryUI inventoryUi;

        private void Start()
        {
            inventoryUi.Hide();

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
            inventoryUi.UnBindTown();
            inventoryUi.Hide();
        }
    }
}