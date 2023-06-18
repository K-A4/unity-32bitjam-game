using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackClip
{
    public float CoolDown;
    public AnimationClip AttackAnimation;
    public float HitStunTime;
    public Transform HitTransform;
    public float KnockPower;
    public float KnockDistance;
    public bool Hard;

    public void PlayAttack(AttackController attacker)
    {
        SetAnimation(attacker.AnimatorOverrideController);
        attacker.CurrentAttack = this;
        attacker.animator.SetTrigger("HandAttack");
        attacker.animator.SetBool("Attacking", true);
        attacker.SetAttackCooldown(CoolDown);
        attacker.mover.SetMoving(false);
    }

    private void SetAnimation(AnimatorOverrideController AOC)
    {
        AOC["HandAttack"] = AttackAnimation;
    }
}

public class AttackController : MonoBehaviour
{
    public Entity mover;
    public Animator animator;
    public LayerMask AttackMask;
    public List<AttackClip> Attacks;
    [HideInInspector] public AnimatorOverrideController AnimatorOverrideController;
    private Color color;
    private float coolDownDuration;
    private float coolDownTime;
    public AttackClip CurrentAttack;
    public ComboController cc;

    public void SetAttackCooldown(float cd)
    {
        //coolDownDuration += cd;
        coolDownTime = Time.time + cd;
    }

    private void Start()
    {
        AnimatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = AnimatorOverrideController;
    }
    
    private void Update()
    {
        if (Time.time > coolDownTime)
        {
            mover.SetMoving(true);
            animator.SetBool("Attacking", false);
        }

        color = Color.green;
    }

    public void TriggerInteract()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1.0f))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactItem))
            {
                interactItem.Interact();
            }
        }
    }

    public void TriggerAttack(int i)
    {
        if (Time.time > coolDownTime/* + coolDownDuration*/)
        {
            Attacks[i].PlayAttack(this);

            if (cc)
            {
                cc.AddFirstInput(i);
            }
        }
        else
        {
            if (cc)
            {
                cc.AddInput(i);
            }
        }
    }

    public void Attack()
    {
        var colls = Physics.OverlapSphere(CurrentAttack.HitTransform.position, 0.25f, AttackMask);
        foreach (var item in colls)
        {
            if (item.transform.TryGetComponent(out Hittable hittable))
            {
                var attcker = item.transform.GetComponentInChildren<AttackController>();
                if (attcker)
                {
                    attcker.mover.SetLaying(CurrentAttack.Hard, CurrentAttack.HitStunTime);
                    attcker.SetAttackCooldown(CurrentAttack.HitStunTime);
                    if (CurrentAttack.KnockPower != 0)
                    {
                        StartCoroutine(KnockBackCoroutine(attcker.mover.transform, CurrentAttack.KnockPower, CurrentAttack.KnockDistance));
                    }
                    color = Color.red;
                }
                hittable.GetHit(1.0f, CurrentAttack.HitTransform.position);
            }
        }
    }

    public IEnumerator KnockBackCoroutine(Transform transform, float knockPower, float distance = 0.2f)
    {
        var startPos = transform.position;
        var endPos = transform.position - (transform.forward * distance);
        var timeElapsed = 0.0f;

        while (true)
        {
            timeElapsed += Time.deltaTime;
            var t = timeElapsed / distance * knockPower;
            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;

            if (t > 1)
            {
                yield break;
            }
        }
    }
    //private void Hit(RaycastHit hitInfo)
    //{
    //    if (hitInfo.transform.TryGetComponent(out Hittable hittable))
    //    {
    //        //hittable.GetHit(1.0f, hitInfo.point);
    //    }
    //    Gizmos.color = Color.red;
    //    attckPos = hitInfo.point;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        if (CurrentAttack != null)
        {
            if (CurrentAttack.HitTransform)
            {
                Gizmos.DrawSphere(CurrentAttack.HitTransform.position, 0.25f);
            }
        }
    }
}
