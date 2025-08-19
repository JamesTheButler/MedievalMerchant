using System.Collections.Generic;
using Data;

namespace Model
{
    public class Town
    {
        private readonly GoodInfoManager _goodInfoManager;
        private readonly TownSetupInfo _setupInfo;

        private Dictionary<Good, int> _goods;

        private float _developmentScore = 100f;
        private float _developmentTrend = 0f;

        public Town(TownSetupInfo setupInfo)
        {
            _setupInfo = setupInfo;
            _goodInfoManager = GoodInfoManager.instance;
        }

        public void Tick()
        {
            UpdateDevelopment();
            Consume();
        }

        public void SellToTown(Good good, int count)
        {
            _goods.TryAdd(good, 0);
            _goods[good] += count;
        }

        public void BuyFromTown(Good good, int count)
        {
            _goods[good] -= count;
        
            if (_goods[good] <= 0)
            {
                _goods[good] = 0;
            }
        }

        private void Consume()
        {
            foreach (var (good, count) in _goods)
            {
                _goods[good]--;
                if (_goods[good] <= 0)
                {
                    _goods[good] = 0;
                }
            }
        }

        private void UpdateDevelopment()
        {
            _developmentTrend = -0.25f;
        
            foreach (var (good, count) in _goods)
            {
                if (count <= 0) continue;
            
                var tier = _goodInfoManager.Infos[good].Tier;
                _developmentTrend += 0.25f * (int)tier + 1; // Tier1 -> 0.25, Tier2 0.5, and so on
            }
        
            _developmentScore += _developmentScore * _developmentTrend;
        }
    }
}