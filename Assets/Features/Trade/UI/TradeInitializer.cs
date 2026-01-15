using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements;
using Features.Player.Logic;
using Features.Towns;
using Features.Trade.Logic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Trade.UI
{
    public sealed class TradeInitializer : InitializableBehavior
    {
        [SerializeField]
        private UnityEvent<Good, TradeType> initializeTradeUI;

        [SerializeField]
        private TradeType tradeType;

        [SerializeField, Required]
        private SimpleErrorPopup errorPopupPrefab;

        private PlayerModel _player;
        private Selection _selection;

        public override void Initialize()
        {
            _player = GameplayContext.Instance.Model.Player;
            _selection = GameplayContext.Instance.Selection;
        }

        public void OnCellClicked(GoodCell cell)
        {
            if (cell.Good == null)
                return;

            var tradeValidator = new TradeValidator(_player, _selection.SelectedTown.Value);
            var tradeResult = tradeValidator.Validate(tradeType, cell.Good.Value, 1);
            if (tradeResult.Success)
            {
                initializeTradeUI.Invoke(cell.Good.Value, tradeType);
            }
            else
            {
                cell.PostMessage(tradeResult.Error);
            }
        }
    }
}