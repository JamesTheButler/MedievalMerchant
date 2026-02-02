using Common.Infrastructure.Gameplay;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns.UI
{
    public sealed class TownUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private TownUI townUi;

        private Selection _selection;

        private void Start()
        {
            townUi.Close();
            townUi.Initialize();

            _selection = GameplayContext.Instance.Selection;
            _selection.SelectedTown.Observe(SelectTown, false);
        }

        private void OnDestroy()
        {
            _selection.SelectedTown.StopObserving(SelectTown);
        }

        private void SelectTown(Town town)
        {
            if (town == null)
            {
                DeselectTown();
                return;
            }

            townUi.Bind(town);
            townUi.Open();
        }

        private void DeselectTown()
        {
            townUi.Unbind();
            townUi.Close();
        }
    }
}