using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Features.Feedback.Logic
{
    public class FeedbackService
    {
        private const string FormUrl = "https://docs.google.com/forms/d/1tdYh9PE26UMTd05RBw-yKAGnQdsa6c0-Vo0P0q6g7ak/formResponse";
        private const string VersionFieldId = "entry.349607095";
        private const string NameFieldId = "entry.1261410891";
        private const string FeedbackFieldId = "entry.1538213941";

        public IEnumerator PostFeedback(string senderName, string feedback)
        {
            var form = new WWWForm();
            form.AddField(NameFieldId, senderName);
            form.AddField(FeedbackFieldId, feedback);
            form.AddField(VersionFieldId, Application.version);

            using var request = UnityWebRequest.Post(FormUrl, form);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Success");
            }
        }
    }
}