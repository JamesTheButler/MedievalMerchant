using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GoodInfoManager", menuName = "Data/GoodInfoManager")]
    public class GoodInfoManager : ScriptableSingleton<GoodInfoManager>
    {
        [SerializeField]
        private List<GoodInfo> infos;

        public Dictionary<Good, GoodInfo> Infos { get; private set; }
    
        private void Awake()
        {
            Infos = infos.ToDictionary(item => item.GoodType, item => item);
        }
    }
}