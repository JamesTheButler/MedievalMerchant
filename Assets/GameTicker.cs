using System;
using UnityEngine;

public class GameTicker : MonoBehaviour
{
    public event Action OnTick;
    
    public float tickRate = 20f;
    private float _tickInterval;
    private float _timer;

    private void Start()
    {
        _tickInterval = 1f / tickRate;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        while (_timer >= _tickInterval)
        {
            _timer -= _tickInterval;
            OnTick?.Invoke();
        }
    }
}