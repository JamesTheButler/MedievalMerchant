using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Overlays
{
    public sealed class OverlayIconGroup : MonoBehaviour
    {
        [SerializeField, Required]
        private Transform itemParent;

        [SerializeField, Required]
        private OverlayIcon itemPrefab;

        [SerializeField]
        private float itemSpacing = 1;

        private readonly List<Sprite> _icons = new();
        private readonly Dictionary<Sprite, OverlayIcon> _iconObjects = new();

        public void AddIcon(Sprite icon)
        {
            if (_icons.Contains(icon))
                return;

            var item = Instantiate(itemPrefab, itemParent);
            item.SetUp(icon);
            SetIconPosition(item, _icons.Count);

            _icons.Add(icon);
            _iconObjects.Add(icon, item);
        }

        public void RemoveIcon(Sprite icon)
        {
            var removedIndex = _icons.IndexOf(icon);
            if (removedIndex < 0)
                return;

            _icons.RemoveAt(removedIndex);
            Destroy(_iconObjects[icon].gameObject);
            _iconObjects.Remove(icon);

            for (var i = removedIndex; i < _icons.Count; i++)
                SetIconPosition(_iconObjects[_icons[i]], i);
        }

        private void SetIconPosition(OverlayIcon icon, int index)
        {
            icon.transform.localPosition = new Vector3(index * itemSpacing, 0, 0);
        }
    }
}