using System;
using System.Linq;
using UnityEngine;

namespace Data
{
    public class Town
    {
        private readonly GoodInfoManager _goodInfoManager;
        private readonly Producer _producer;

        public event Action TierChanged;

        public Inventory Inventory { get; } = new();
        public string Name { get; }
        public Tier Tier { get; private set; }
        public Vector2Int Location { get; }
        public float DevelopmentScore { get; private set; } = 100f;
        public float DevelopmentTrend { get; private set; } = 0f;

        public Town(TownSetupInfo setupInfo, Vector2Int location, GoodInfoManager goodInfoManager)
        {
            Location = location;
            _goodInfoManager = goodInfoManager;

            Name = setupInfo.NameGenerator.GenerateName();
            Tier = Tier.Tier1;
            _producer = new Producer(setupInfo.Production);
            Inventory.AddFunds(setupInfo.InitialFunds);
        }

        public void Tick()
        {
            UpdateDevelopment();
            Produce();
            Consume();
        }

        private void Produce()
        {
            var production = _producer.Produce();
            foreach (var (good, amount) in production)
            {
                Inventory.AddGood(good, amount);
            }
        }

        private void Consume()
        {
            foreach (var good in Inventory.Goods.Keys.ToList())
            {
                Inventory.RemoveGood(good, 1);
            }
        }

        private void UpdateDevelopment()
        {
            DevelopmentTrend = -0.25f;

            foreach (var good in Inventory.Goods.Keys.ToList())
            {
                var tier = _goodInfoManager.GoodInfos[good].Tier;
                DevelopmentTrend += 0.25f * (int)tier + 1; // Tier1 -> 0.25, Tier2 0.5, and so on
            }

            DevelopmentScore += DevelopmentScore * DevelopmentTrend;
        }

        public void Upgrade()
        {

            switch (Tier)
            {
                case Tier.Tier1:
                    Tier = Tier.Tier2;
                    TierChanged?.Invoke();
                    break;
                case Tier.Tier2:
                    Tier = Tier.Tier3;
                    TierChanged?.Invoke();
                    break;
                case Tier.Tier3:
                default: break;
            }

            Debug.Log($"{Name} upgraded to {Tier}");
            _producer.UpgradeTier(Tier);
        }
    }
}