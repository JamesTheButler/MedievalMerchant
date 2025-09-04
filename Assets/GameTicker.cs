using UnityEngine;
using UnityEngine.Events;

public sealed class GameTicker : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onTick;

    [SerializeField]
    private float secondsPerTick = .5f;

    private float _timer;

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;

        while (_timer >= secondsPerTick)
        {
            _timer -= secondsPerTick;
            onTick?.Invoke();
        }
    }
}