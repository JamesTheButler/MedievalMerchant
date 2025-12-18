using Features.Player.Retinue.Logic;
using Infrastructure;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinuePanel : MonoBehaviour
    {
        private void Start()
        {
            Bind(GameplayContext.Instance.Model.Player.RetinueManager);
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        private void Bind(RetinueManager retinueManager) { }
    }
}