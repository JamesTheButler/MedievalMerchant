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

        [SerializeField, Required]
        private new Animation animation;

        private Selection _selection;
        private readonly GameSpeedAnimationHandler _animationHandler = new();

        private void Start()
        {
            _selection = GameplayContext.Instance.Selection;

            _animationHandler.Initialize(animation);
            _selection.SelectedTown.Observe(Select);
        }

        private void OnDestroy()
        {
            _animationHandler.CleanUp();
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