using System;
using Common;
using UnityEngine;

namespace Features.Towns
{
    public sealed class TownTicker : MonoBehaviour
    {
        private readonly Lazy<GameplayModel> _model = new(() => GameplayModel.Instance);

        public void Tick()
        {
            foreach (var town in _model.Value.Towns.Values)
            {
                town.Tick();
            }
        }
    }
}