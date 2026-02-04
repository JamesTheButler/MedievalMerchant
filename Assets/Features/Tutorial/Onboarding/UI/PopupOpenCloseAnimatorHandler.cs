using System;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Tutorial.Onboarding.UI
{
    public sealed class PopupOpenCloseAnimatorHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private Animator animator;

        public event Action OnClosed;

        private static readonly int TriggerOpen = Animator.StringToHash("Open");
        private static readonly int TriggerClose = Animator.StringToHash("Close");

        public void StartOpenAnimation()
        {
            animator.SetTrigger(TriggerOpen);
        }

        public void StartCloseAnimation()
        {
            animator.SetTrigger(TriggerClose);
        }

        public void CloseAnimationFinishedEvent()
        {
            OnClosed?.Invoke();
        }
    }
}