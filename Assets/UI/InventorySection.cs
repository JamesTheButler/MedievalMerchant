using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventorySection : MonoBehaviour
    {
        [SerializeField]
        private Transform parent;
        [SerializeField]
        private GameObject cellPrefab;
        [SerializeField]
        private List<Good> goods;

        public Dictionary<Good, InventoryCell> Cells { get; private set; }

        public void Start()
        {
            Cells = new Dictionary<Good, InventoryCell>();

            foreach (var good in goods)
            {
                var cellObject = Instantiate(cellPrefab, parent);
                var cell = cellObject.GetComponent<InventoryCell>();
                cell.SetGood(good);
                
                Cells.Add(good, cell);
            }
        }
    }
}
