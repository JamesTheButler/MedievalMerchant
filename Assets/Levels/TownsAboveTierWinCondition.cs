using System;
using AYellowpaper.SerializedCollections;
using Data;
using UnityEngine;

namespace Levels
{
    [Serializable]
    public sealed class TownsAboveTierWinCondition : WinCondition
    {
        [SerializeField, SerializedDictionary]
        private SerializedDictionary<Tier, int> tiersToReach;

        
        private Model _model;

        private void Awake()
        {
            _model = Model.Instance;
        }
        
        public override bool Evaluate()
        {
            return false;
        }
    }
}