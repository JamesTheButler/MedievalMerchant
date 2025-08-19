using Model;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GoodInfo", menuName = "Data/GoodInfo")]
    public class GoodInfo : ScriptableObject
    {
        [field: SerializeField] public Good GoodType { get; private set; }
        [field: SerializeField] public string GoodName { get; private set; }
        [field: SerializeField] public Tier Tier { get; private set; }
        [field: SerializeField] public float BasePrice { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}