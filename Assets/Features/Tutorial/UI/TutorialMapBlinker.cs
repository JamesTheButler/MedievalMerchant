using Common.Utility;
using UnityEngine;

namespace Features.Tutorial.UI
{
    public sealed class TutorialMapBlinker : MonoBehaviour
    {
        public void Show(Vector2 worldLocation)
        {
            gameObject.transform.localPosition = worldLocation.FromXY(gameObject.transform.localPosition.z);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}