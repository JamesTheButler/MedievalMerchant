using Infrastructure;
using UnityEngine;

namespace Features.Player
{
    public sealed class PlayerTicker : MonoBehaviour
    {
        public void Tick()
        {
            GameplayContext.Instance.Model.Player.Tick();
        }
    }
}