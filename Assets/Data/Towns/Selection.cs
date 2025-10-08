using System;
using UnityEngine;

namespace Data.Towns
{
    public sealed class Selection : MonoBehaviour
    {
        public static Selection Instance;

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

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}