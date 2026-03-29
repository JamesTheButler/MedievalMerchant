using Common.Infrastructure.Observation;

namespace Features.Towns
{
    public sealed class Selection
    {
        public Observable<Town> SelectedTown { get; } = new();
        public Observable<bool> CampSelected { get; } = new();

        public void Select(Town town)
        {
            if (SelectedTown.Value == town)
                return;

            CampSelected.Value = false;
            SelectedTown.Value = town;
        }

        public void SelectCamp()
        {
            SelectedTown.Value = null;
            CampSelected.Value = true;
        }

        public void Deselect()
        {
            SelectedTown.Value = null;
            CampSelected.Value = false;
        }
    }
}