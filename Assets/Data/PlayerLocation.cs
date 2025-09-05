using System;
using Common;
using Data.Towns;
using JetBrains.Annotations;
using UnityEngine;

namespace Data
{
    public sealed class PlayerLocation
    {
        public event Action<Town> TownEntered;
        public event Action<Town> TownExited;

        public Observable<Vector2> WorldLocation { get; } = new();

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

        [CanBeNull]
        private Town _currentTown;
    }
}