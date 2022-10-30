using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve alphaChange;
    [SerializeField] private float duration = 1f;

    private float defaultValue;
    private bool isPlaying = false;
    private float time = 0;
    private Action<float, float> valueChangeFunc;
    private float curveTime => duration > 0 ? time / duration : 1f;
    
    private void Update()
    {
        if (isPlaying)
        {
            if (time == duration)
            {
                Stop();
            }
            else
            {
                valueChangeFunc?.Invoke(alphaChange.Evaluate(curveTime), defaultValue);

                time = Mathf.Clamp(time + Time.deltaTime, 0, duration);
            }
        }
    }

    public void Play(Action<float, float> valueChangeFunc, float defaultValue)
    {
        this.valueChangeFunc = valueChangeFunc;
        this.defaultValue = defaultValue;
        time = 0;
        isPlaying = true;
    }

    public void Stop()
    {
        isPlaying = false;
        time = 0;
    }
}
