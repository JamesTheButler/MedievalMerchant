using UnityEngine;
using UnityEngine.UI;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinueMiniProgressElement : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private Sprite completedSprite, incompleteSprite;

        public void SetCompleted(bool isCompleted)
        {
            image.sprite = isCompleted ? completedSprite : incompleteSprite;
        }
    }
}