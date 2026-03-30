using Common.Infrastructure.Observation;

namespace Features.Towns
{
    public sealed class Selection
    {
        public Observable<Town> SelectedTown { get; } = new();

        public void Select(Town town)
        {
            if (SelectedTown.Value == town)
                return;

            SelectedTown.Value = town;
        }

        public void Deselect()
        {
            SelectedTown.Value = null;
        }
    }
}