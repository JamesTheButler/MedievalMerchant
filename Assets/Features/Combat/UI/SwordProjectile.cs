using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Combat.UI
{
    public sealed class SwordProjectile : MonoBehaviour
    {
        [SerializeField, Required]
        private RectTransform rectTransform;

        [SerializeField]
        private AnimationCurve flightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [SerializeField]
        private float spinDegrees = 540f;

        public void Throw(RectTransform from, RectTransform to, float seconds, Action onArrive)
        {
            StartCoroutine(FlyRoutine(from, to, seconds, onArrive));
        }

        private IEnumerator FlyRoutine(RectTransform from, RectTransform to, float seconds, Action onArrive)
        {
            var parent = (RectTransform)rectTransform.parent;
            var start = AnchoredCentreOf(from, parent);
            var end = AnchoredCentreOf(to, parent);

            var facing = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            var elapsed = 0f;

            while (elapsed < seconds)
            {
                var t = flightCurve.Evaluate(elapsed / seconds);

                rectTransform.anchoredPosition = Vector2.LerpUnclamped(start, end, t);
                rectTransform.localRotation = Quaternion.Euler(0f, 0f, facing + spinDegrees * t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            rectTransform.anchoredPosition = end;

            onArrive?.Invoke();
            Destroy(gameObject);
        }

        private static Vector2 AnchoredCentreOf(RectTransform target, RectTransform parent)
        {
            var world = target.TransformPoint(target.rect.center);
            var local = (Vector2)parent.InverseTransformPoint(world);
            return local - parent.rect.min;
        }
    }
}