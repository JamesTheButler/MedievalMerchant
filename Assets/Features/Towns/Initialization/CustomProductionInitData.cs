using System;
using AYellowpaper.SerializedCollections;
using Common.Types;
using UnityEngine;

namespace Features.Towns.Initialization
{
    [Serializable]
    public sealed class CustomProductionInitData : ProductionInitData
    {
        [SerializeField, SerializedDictionary("Good", "Start Amount")]
        private SerializedDictionary<Good, int> startGoods;

        public override void Initialize(Town town)
        {
            foreach (var (good, amount) in startGoods)
            {
                town.ProductionManager.AddProducer(good, 0);
                town.Inventory.AddGood(good, amount);
            }
        }
    }
}