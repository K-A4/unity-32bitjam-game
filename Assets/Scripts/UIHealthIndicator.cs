using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthIndicator : MonoBehaviour
{
    [SerializeField] private AnimationCurve alphaByHealthCurve;
    [SerializeField] private ValueAnimation getHitAnimation;
    private Image vignettImage;

    void Start()
    {
        vignettImage = GetComponent<Image>();
    }

    public void OnHealthChange(float changeValue, float currentHealth)
    {
        Color c = vignettImage.color;
        c.a = alphaByHealthCurve.Evaluate(Mathf.Max(currentHealth + changeValue, 0));
        vignettImage.color = c;

        getHitAnimation.Play(
            (val, defaultVal) =>
            {
                Color color = vignettImage.color;
                color.a = val + defaultVal;
                vignettImage.color = color;
            },
            vignettImage.color.a
        );
    }
}
