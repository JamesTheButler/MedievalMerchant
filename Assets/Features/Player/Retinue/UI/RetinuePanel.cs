using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinuePanel : MonoBehaviour
    {
        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}