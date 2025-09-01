using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // TODO: needs a "reset" or "cancel" method to abort the current animation
    public class SliderAnimator : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private float unitsPerSecond = 120f;

        [SerializeField]
        private float duration = 2f;

        [SerializeField]
        private float snapEpsilon = 0.01f;

        private float _targetValue;
        private Coroutine _tween;

        private void Reset()
        {
            slider = GetComponent<Slider>();
        }

        private void Awake()
        {
            if (!slider)
            {
                slider = GetComponent<Slider>();
                if (!slider)
                {
                    Debug.LogError("Slider must be set");
                }
            }

            _targetValue = slider ? slider.value : 0f;
        }

        public void SetValue(float newValue, bool animate)
        {
            if (!slider) return;

            _targetValue = Mathf.Clamp(newValue, slider.minValue, slider.maxValue);

            if (!animate)
            {
                slider.value = _targetValue;
                return;
            }
            
            if (_tween != null)
            {
                StopCoroutine(_tween);
                _tween = null;
            }

            var currentValue = slider.value;
            var distance = Mathf.Abs(_targetValue - currentValue);
            if (distance <= snapEpsilon)
            {
                slider.value = _targetValue;
                return;
            }

            _tween = StartCoroutine(Tween(currentValue, _targetValue, duration));
        }

        private IEnumerator Tween(float from, float to, float tweenDuration)
        {
            var t = 0f;
            while (t < tweenDuration)
            {
                t += Time.deltaTime;
                var p = Mathf.Clamp01(t / tweenDuration);
                var v = Mathf.Lerp(from, to, p);
                slider.value = v;

                if (Mathf.Abs(to - v) <= snapEpsilon)
                {
                    slider.value = to;
                    break;
                }

                yield return null;
            }

            slider.value = to;
            _tween = null;
        }
    }
}