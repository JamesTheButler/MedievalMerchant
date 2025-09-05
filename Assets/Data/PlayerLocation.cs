using System;
using Data.Towns;
using JetBrains.Annotations;
using UnityEngine;

namespace Data
{
    public sealed class PlayerLocation
    {
        public event Action<Town> TownEntered;
        public event Action<Town> TownExited;
        public event Action<Vector2> WorldLocationChanged;

        [CanBeNull]
        public Town CurrentTown
        {
            get => _currentTown;
            set
            {
                _currentTown = value;

                var action = value == null ? TownExited : TownEntered;
                action?.Invoke(_currentTown);

                _currentTown = value;
            }
        }

        public Vector2 WorldLocation
        {
            get => _worldLocation;
            set
            {
                _worldLocation = value;
                WorldLocationChanged?.Invoke(_worldLocation);
            }
        }

        [CanBeNull]
        private Town _currentTown;

        private Vector2 _worldLocation;
    }
}