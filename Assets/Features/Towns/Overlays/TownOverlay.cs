using System;
using Common.UI.Elements;
using Common.Utility;
using UnityEngine;

namespace Features.Towns.Overlays
{
    public sealed class TownOverlay : MonoBehaviour, IOpenClosable
    {
        public event Action Opened;
        public event Action Closed;

        private Town _town;
        private Vector3 _worldPosition;

        [SerializeField]
        private float yOffset;

        public void SetUp(Town town)
        {
            _town = town;
            _worldPosition = town.WorldLocation.FromXY();
        }

        public void Open()
        {
            // bind all data

            gameObject.SetActive(true);
            RefreshPosition();
        }

        public void Close()
        {
            // un-bind all data

            gameObject.SetActive(false);
        }

        public void RefreshPosition()
        {
            var screenPosition = Camera.main!.WorldToScreenPoint(_worldPosition);
            gameObject.transform.position = screenPosition;
        }
    }
}