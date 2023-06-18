using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIText : MonoBehaviour
{
    public static UIText Instance;
    [SerializeField] private TextMeshProUGUI text1;
    public UnityEvent OnEnable;
    public UnityEvent OnDisable;

    private void Awake()
    {
        Instance = this;
    }

    public void SendText(string text, float ShowTime)
    {
        if (text!="")
        {
            StartCoroutine(ShowMassageCor(text, ShowTime));
        }
    }

    private IEnumerator ShowMassageCor(string massage, float showTime)
    {
        var timeELapse = 0.0f;
        text1.enabled = true;
        OnEnable?.Invoke();
        while (timeELapse < showTime)
        {
            timeELapse += Time.deltaTime;
            text1.text = massage;
            yield return null;
        }
        text1.text = "";
        text1.enabled = false;
        OnDisable?.Invoke();
        yield break;
    }
}
