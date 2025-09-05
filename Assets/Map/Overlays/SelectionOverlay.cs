using Data;
using Data.Towns;
using UnityEngine;

namespace Map.Overlays
{
    public sealed class SelectionOverlay : MonoBehaviour
    {
        [SerializeField]
        private GameObject visuals;

        private Selection _selection;
        private Vector2 _origin;

        private void Start()
        {
            _selection = Selection.Instance;
            _origin = Model.Instance.TileFlagMap.Origin;

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
            gameObject.transform.localPosition = town.WorldLocation + _origin;
        }
    }
}