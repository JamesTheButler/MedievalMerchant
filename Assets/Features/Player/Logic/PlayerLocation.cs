using System;
using Common.Infrastructure.Observation;
using Features.Towns;
using JetBrains.Annotations;
using UnityEngine;

namespace Features.Player.Logic
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
                if (_currentTown == value)
                    return;

                _currentTown = value;

                var action = _currentTown == null ? TownExited : TownEntered;
                action?.Invoke(_currentTown);
            }
        }

        [CanBeNull]
        private Town _currentTown;
    }
}