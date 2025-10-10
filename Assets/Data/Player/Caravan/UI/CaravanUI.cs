using System.Collections.Generic;
using Data.Player.Caravan.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Data.Player.Caravan.UI
{
    public sealed class CaravanUI : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text moveSpeedText;

        [SerializeField, Required]
        private TMP_Text upkeepText;

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

            _caravanManager.TotalMoveSpeed.Observe(OnMoveSpeedChanged);
            _caravanManager.TotalUpkeep.Observe(OnUpkeepChanged);
        }

        private void OnDestroy()
        {
            foreach (var cartUI in cartUis)
            {
                cartUI.Unbind();
            }

            _caravanManager.TotalMoveSpeed.StopObserving(OnMoveSpeedChanged);
            _caravanManager.TotalUpkeep.StopObserving(OnUpkeepChanged);
        }

        private void OnMoveSpeedChanged(float moveSpeed)
        {
            moveSpeedText.text = moveSpeed.ToString("0.##");
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepText.text = upkeep.ToString("0.##");
        }
    }
}