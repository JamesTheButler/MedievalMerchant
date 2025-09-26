using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InventoryUI.TownInventory
{
    public sealed class ArrowDrawer : MonoBehaviour
    {
        [SerializeField]
        private RectTransform[] topAnchors;

        [SerializeField]
        private RectTransform[] bottomAnchors;

        [SerializeField]
        private Image arrowPrefab;

        [SerializeField]
        private Transform arrowParent;

        private readonly List<Image> _activeArrows = new();

        public void AddArrow(int topIndex, int bottomIndex)
        {
            if (topIndex < 0 || topIndex >= topAnchors.Length)
            {
                Debug.LogError($"Could not add arrow. Invalid topIndex {topIndex}.");
                return;
            }

            if (bottomIndex < 0 || bottomIndex >= bottomAnchors.Length)
            {
                Debug.LogError($"Could not add arrow. Invalid bottomIndex {bottomIndex}.");
                return;
            }

            var start = topAnchors[topIndex].position;
            var end = bottomAnchors[bottomIndex].position;
            var dir = end - start;

            var arrow = Instantiate(arrowPrefab, arrowParent);
            arrow.name = $"Arrow_{topIndex}-{bottomIndex}";
            _activeArrows.Add(arrow);

            var rect = arrow.rectTransform;
            rect.position = start;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, dir.magnitude / rect.lossyScale.y);
            rect.rotation = Quaternion.FromToRotation(Vector3.down, dir);
        }

        public void ClearArrows()
        {
            foreach (var arrow in _activeArrows)
            {
                Destroy(arrow.gameObject);
            }

            _activeArrows.Clear();
        }
    }
}