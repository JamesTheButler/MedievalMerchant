using System;
using Data.Towns;
using UnityEngine;

namespace UI.Popups
{
    public sealed class Tier1ConstructionPopupHandler : MonoBehaviour
    {
        [SerializeField]
        private Tier1ConstructionPopup popup;

        private readonly Lazy<Selection> _selection = new(() => Selection.Instance);
        
        public void Show()
        {
            popup.gameObject.SetActive(true);

            var town = _selection.Value.SelectedTown;
            popup.Setup(town, 1000);
        }

        public void Hide()
        {
            popup.gameObject.SetActive(false);
        }
    }
}