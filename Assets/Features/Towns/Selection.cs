using System;

namespace Features.Towns
{
    public sealed class Selection
    {
        // TODO - STYLE: use Observable<Town>
        public event Action<Town> TownSelected;

        public Town SelectedTown { get; private set; }

        public void Select(Town town)
        {
            if (SelectedTown == town)
                return;

            SelectedTown = town;
            TownSelected?.Invoke(town);
        }

        public void Deselect()
        {
            Select(null);
        }
    }
}