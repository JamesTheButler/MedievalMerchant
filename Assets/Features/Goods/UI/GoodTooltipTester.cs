using Common.Types;
using UnityEngine;

namespace Features.Goods.UI
{
    public class GoodTooltipTester : MonoBehaviour
    {
        [SerializeField]
        private Good good;
        
        private GoodTooltipHandler _goodTooltipHandler;

        private void Start()
        {
            GetComponent<GoodTooltipHandler>().SetTooltip(good);
        }
    }
}