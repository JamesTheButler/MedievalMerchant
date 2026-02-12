using NaughtyAttributes;
using UnityEngine;

namespace Common.UI.Art
{
    public sealed class SimpleAnimationHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private Animator animator;

        [SerializeField]
        private string playTriggerKey = "Play";

        private int _triggerPlay;

        private void Awake()
        {
            _triggerPlay = Animator.StringToHash(playTriggerKey);
        }

        public void Play()
        {
            Stop();
            animator.SetTrigger(_triggerPlay);
        }

        public void Stop()
        {
            animator.ResetTrigger(_triggerPlay);
        }
    }
}