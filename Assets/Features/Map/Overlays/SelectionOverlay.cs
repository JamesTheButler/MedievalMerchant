using Common.Infrastructure;
using Features.Towns;
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
            _selection = GameplayContext.Instance.Selection;

            _selection.SelectedTown.Observe(Select);

            Select(_selection.SelectedTown);
        }

        private void OnDestroy()
        {
            _selection.SelectedTown.StopObserving(Select);
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