using System;
using Common;
using Infrastructure;
using UnityEngine;

namespace Features.Towns
{
    public sealed class TownTicker : MonoBehaviour
    {
        private readonly Lazy<GameplayModel> _model = new(() => GameplayContext.Instance.Model);

        public void Tick()
        {
            foreach (var town in _model.Value.Towns.Values)
            {
                town.Tick();
            }
        }
    }
}