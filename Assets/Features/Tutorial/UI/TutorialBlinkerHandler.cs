using NaughtyAttributes;
using UnityEngine;

namespace Features.Tutorial.UI
{
    public sealed class TutorialBlinkerHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private TutorialUIBlinker uiBlinker;

        [SerializeField, Required]
        private TutorialMapBlinker mapBlinker;

        public void Show(RectTransform targetTransform)
        {
            mapBlinker.Hide();
            uiBlinker.Show(targetTransform);
        }

        public void Show(Vector2 targetPosition)
        {
            uiBlinker.Hide();
            mapBlinker.Show(targetPosition);
        }

        public void Hide()
        {
            mapBlinker.Hide();
            uiBlinker.Hide();
        }
    }
}