using UnityEngine;

namespace Data
{
    public class Town
    {
        private readonly GoodInfoManager _goodInfoManager;
        private readonly TownSetupInfo _setupInfo;
        private readonly Inventory _inventory = new();
        private readonly Producer _producer;

        private Vector2Int _location;
        private float _developmentScore = 100f;
        private float _developmentTrend = 0f;

        public Town(TownSetupInfo setupInfo, Vector2Int location)
        {
            _setupInfo = setupInfo;
            _location = location;
            _producer = new Producer(setupInfo.Production);
            _goodInfoManager = GoodInfoManager.instance;
            
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

            foreach (var good in production)
            {
                _inventory.AddGood(good, 1);
            }
        }

        private void Consume()
        {
            foreach (var (good, _) in _inventory.Goods)
            {
                _inventory.RemoveGood(good, 1);
            }
        }

        private void UpdateDevelopment()
        {
            _developmentTrend = -0.25f;
        
            foreach (var (good, _) in _inventory.Goods)
            {
                var tier = _goodInfoManager.Infos[good].Tier;
                _developmentTrend += 0.25f * (int)tier + 1; // Tier1 -> 0.25, Tier2 0.5, and so on
            }
        
            _developmentScore += _developmentScore * _developmentTrend;
        }
    }
}