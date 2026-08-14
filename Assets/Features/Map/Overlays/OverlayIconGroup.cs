using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Map.Overlays
{
    public sealed class OverlayIconGroup : MonoBehaviour
    {
        [SerializeField, Required]
        private Transform itemParent;

        [SerializeField, Required]
        private GameObject itemPrefab;

        [SerializeField]
        private float itemSpacing = 1;
        
        private readonly SortedDictionary<Sprite, GameObject> _icons = new();

        public void AddIcon(Sprite icon)
        {
            if (_icons.ContainsKey(icon))
                return;

            var item = Instantiate(
                itemPrefab,
                new Vector3(_icons.Count * itemSpacing, 0, 0),
                quaternion.identity,
                itemParent);

            _icons.Add(icon, item);
        }

        public void RemoveIcon(Sprite icon)
        {
            if (!_icons.Contains(icon))
                return;

            var removedIndex = _icons.(icon);
            _icons.Remove(icon);

            for (var i = removedIndex; i >= 0; i--) { }
        }
    }
}