using Features.StartMenu.UI;
using UnityEngine;

namespace Features.Feedback.UI
{
    public class FeedbackButton : MonoBehaviour
    {
        public void OpenFeedbackUI()
        {
            var feedbackForm = FindFirstObjectByType<FeedbackForm>(FindObjectsInactive.Include);
            if (feedbackForm == null)
            {
                Debug.LogError($"Could not find a {nameof(FeedbackForm)} in scene.");
                return;
            }
            
            feedbackForm.gameObject.SetActive(true);
        }
    }
}