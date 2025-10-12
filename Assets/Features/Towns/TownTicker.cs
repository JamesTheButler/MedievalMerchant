using System;
using Common;
using UnityEngine;

namespace Features.Towns
{
    public sealed class TownTicker : MonoBehaviour
    {
        private readonly Lazy<Model> _model = new(() => Model.Instance);

        public void Tick()
        {
            foreach (var town in _model.Value.Towns.Values)
            {
                town.Tick();
            }
        }
    }
}