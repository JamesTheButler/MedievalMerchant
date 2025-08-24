using System;
using UnityEngine;

namespace Data
{
    public class Selection : MonoBehaviour
    {
        public static Selection Instance;

        public event Action<Town> TownSelected;
        public Town SelectedTown { get; private set; }

        public void Select(Town town)
        {
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