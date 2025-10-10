using System.Collections.Generic;
using Data.Player.Caravan.Logic;
using NaughtyAttributes;
using TMPro;
using UI;
using UnityEngine;

namespace Data.Player.Caravan.UI
{
    public sealed class CaravanUI : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text moveSpeedText, upkeepText;

        [SerializeField, Required]
        private TooltipHandler moveSpeedTooltip, upkeepTooltip;

        [SerializeField]
        private List<CartUI> cartUis;

        private CaravanManager _caravanManager;

        private void Start()
        {
            _caravanManager = Model.Instance.Player.CaravanManager;

            for (var i = 0; i < _caravanManager.Carts.Count; i++)
            {
                var cartId = i;
                cartUis[i].Bind(_caravanManager.Carts[i], () => _caravanManager.UpgradeCart(cartId));
            }

            _caravanManager.MoveSpeed.Observe(OnMoveSpeedChanged);
            _caravanManager.Upkeep.Observe(OnUpkeepChanged);
        }

        private void OnDestroy()
        {
            foreach (var cartUI in cartUis)
            {
                cartUI.Unbind();
            }

            _caravanManager.MoveSpeed.StopObserving(OnMoveSpeedChanged);
            _caravanManager.Upkeep.StopObserving(OnUpkeepChanged);
        }

        private void OnMoveSpeedChanged(float moveSpeed)
        {
            moveSpeedText.text = moveSpeed.ToString("0.##");
            moveSpeedTooltip.SetTooltip(_caravanManager.MoveSpeed.ToString());
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepText.text = upkeep.ToString("0.##");
            upkeepTooltip.SetTooltip(_caravanManager.Upkeep.ToString());
        }
    }
}