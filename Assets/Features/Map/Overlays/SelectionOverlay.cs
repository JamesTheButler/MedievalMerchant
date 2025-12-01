using Features.Towns;
using Infrastructure;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Overlays
{
    public sealed class SelectionOverlay : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject visuals;

        private Selection _selection;

        private void Start()
        {
            _selection = GameplayContext.Selection;

            _selection.TownSelected += Select;

            Select(_selection.SelectedTown);
        }

        private void OnDestroy()
        {
            _selection.TownSelected -= Select;
        }

        private void Select(Town town)
        {
            if (town == null)
            {
                visuals.gameObject.SetActive(false);
                return;
            }

            visuals.gameObject.SetActive(true);
            gameObject.transform.localPosition = town.WorldLocation;
        }
    }
}