using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Combo
{
    public string ComboInputs;
    public float firstAttackDuration;
    public List<AttackClip> Animations;
}

public class ComboController : MonoBehaviour
{
    public AttackController Attacker;
    public List<Combo> Combos;
    [SerializeField] private float inputTime = 0.2f;
    private Dictionary<string, Combo> pairs = new();
    private string lastPressed = "";
    private float lastPressedTime;

    private void Start()
    {
        foreach (var combo in Combos)
        {
            string inputs = combo.ComboInputs;
            
            pairs.Add(inputs, combo);
        }
    }

    public void AddInput(int input)
    {
        if (Time.time < lastPressedTime + inputTime)
        {
            AddPressedInput(input);
        }
    }

    public void AddFirstInput(int input)
    {
        lastPressed += $"{input}";
        lastPressedTime = Time.time;
    }

    private void AddPressedInput(int input)
    {
        lastPressed += $"{input}";
        lastPressedTime = Time.time;
        //Attacker.SetAttackCooldown(inputTime);
    }

    private void Update()
    {
        if (Time.time > lastPressedTime + inputTime)
        {
            PlayCombo();
            lastPressed = "";
        }
    }

    private void PlayCombo()
    {
        if (pairs.ContainsKey(lastPressed))
        {
            var combo = pairs[lastPressed];
            StartCoroutine(PlayAllAnimation(combo.Animations, combo.firstAttackDuration));
        }
    }

    private IEnumerator PlayAllAnimation(List<AttackClip> animation, float offset)
    {
        var timeOFfset = Time.time + offset;
        Attacker.SetAttackCooldown(offset);
        while (true)
        {
            if (Time.time > timeOFfset)
            {
                break;
            }
            yield return null;
        }
        for (int i = 0; i < animation.Count; i++)
        {
            animation[i].PlayAttack(Attacker);
            var animLength = animation[i].CoolDown;
            var time = Time.time + animLength;
            while (true)
            {
                if (Time.time > time)
                {
                    break;
                }
                yield return null;
            }
        }
    }
}
