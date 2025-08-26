using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GoodInfoManager", menuName = "Data/GoodInfoManager")]
    public sealed class GoodInfoManager : ScriptableObject
    {
        [SerializeField]
        private List<GoodInfo> goodInfos;

        public IReadOnlyDictionary<Good, GoodInfo> GoodInfos => _goodInfoDict ??= GenerateGoodDictionary();

        private Dictionary<Good, GoodInfo> _goodInfoDict;

        private Dictionary<Good, GoodInfo> GenerateGoodDictionary()
        {
            return goodInfos.ToDictionary(info => info.GoodType, info => info);
        }
    }
}