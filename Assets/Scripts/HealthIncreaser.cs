using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncreaser : MonoBehaviour
{
    [SerializeField] private Hittable target;
    [SerializeField] private float healthInc = 1f;
    [SerializeField] private float frezeHealingDuration = 2f;

    private float startHealTime = float.MaxValue; 

    private void Update()
    {
        if(Time.time >= startHealTime)
        {
            target.AddHealth(healthInc * Time.deltaTime);
        }
    }

    public void OnGetHit(int health)
    {
        if(health <= 0 && health >= target.MaxHealth)
        {
            startHealTime = float.MaxValue;
        }
        else
        {
            startHealTime = frezeHealingDuration + Time.time;
        }
    }
}
