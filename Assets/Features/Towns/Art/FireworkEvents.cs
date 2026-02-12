using System.Collections.Generic;
using Common.UI.Art;
using UnityEngine;

namespace Features.Towns.Art
{
    public sealed class FireworkEvents : MonoBehaviour
    {
        [SerializeField]
        private List<SimpleAnimatorHandler> lightAnimatorHandlers;

        public void Fire(int index)
        {
            var animatorHandler = lightAnimatorHandlers[index];
            animatorHandler.Stop();
            animatorHandler.Play();
        }
    }
}