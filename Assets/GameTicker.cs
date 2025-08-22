using System;
using UnityEngine;

public class GameTicker : MonoBehaviour
{
    public event Action OnTick;

    [SerializeField]
    private float tickRateSec = .02f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        while (_timer >= tickRateSec)
        {
            _timer -= tickRateSec;
            OnTick?.Invoke();
        }
    }
}