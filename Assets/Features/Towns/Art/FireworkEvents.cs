using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Features.Towns.Art
{
    public sealed class FireworkEvents : MonoBehaviour
    {
        [SerializeField]
        private List<Light2D> lights;

        private Coroutine _coroutine;

        [SerializeField]
        private float flashDurationSeconds = 0.25f, peakIntensity = 1.8f;

        private readonly Dictionary<int, Coroutine> _coroutines = new();

        public void Fire(int index)
        {
            if (_coroutines.TryGetValue(index, out var coroutine) && coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            _coroutines[index] = StartCoroutine(FlashRoutine(index));
        }

        private IEnumerator FlashRoutine(int index)
        {
            var light = lights[index];
            light.intensity = peakIntensity;

            var elapsedSeconds = 0f;
            while (elapsedSeconds < flashDurationSeconds)
            {
                elapsedSeconds += Time.deltaTime;
                var t = Mathf.Clamp01(elapsedSeconds / flashDurationSeconds);
                light.intensity = Mathf.Lerp(peakIntensity, 0, t);
                yield return null;
            }

            light.intensity = 0;

            _coroutines[index] = null;
        }
    }
}