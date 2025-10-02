using UnityEngine;
using UnityEngine.Events;

public sealed class GameTicker : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onTick;

    [SerializeField, Range(0.1f, 5f)]
    private float secondsPerTick = .5f;

    private float _timer;

    private bool _isTicking;

    public void StartTicking()
    {
        _isTicking = true;
    }

    public void StopTicking()
    {
        _isTicking = false;
    }

    private void FixedUpdate()
    {
        if (!_isTicking) return;

        _timer += Time.fixedDeltaTime;

        while (_timer >= secondsPerTick)
        {
            _timer -= secondsPerTick;
            onTick?.Invoke();
        }
    }
}