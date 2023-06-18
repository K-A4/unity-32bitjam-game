using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Hittable target;
    [SerializeField] private Image healthBar;
    [SerializeField] private ValueAnimation healthBarAnimation;

    private void Start()
    {
        SetHealthBar(target.Health / target.MaxHealth);
        target.OnHealthChange.AddListener(OnHealthChange);
    }

    private void SetHealthBar(float val)
    {
        healthBar.fillAmount = val;
    }

    private void OnHealthChange(float changeValue, float currentHealth)
    {
        float newHealth = Mathf.Clamp(currentHealth + changeValue, 0, target.MaxHealth);
        float range = newHealth - currentHealth;

        healthBarAnimation.Play(
            (val, defaultVal) =>
            {
                healthBar.fillAmount = (defaultVal + range * val) / target.MaxHealth;
            },
            currentHealth
        );
    }
}
