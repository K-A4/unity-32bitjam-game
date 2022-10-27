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

    public void PlayAttack(AttackPlayer attacker)
    {
        SetAnimation(attacker.AnimatorOverrideController);
        attacker.animator.SetTrigger("HandAttack");
        attacker.SetAttackCooldown(CoolDown);
        attacker.mover.SetMoving(false);
    }

    private void SetAnimation(AnimatorOverrideController AOC)
    {
        AOC["HandAttack"] = AttackAnimation;
    }
}

public class AttackPlayer : MonoBehaviour
{
    public Entity mover;
    public Animator animator;
    public Transform AttackCollider;
    public LayerMask AttackMask;
    public List<AttackClip> Attacks;
    [HideInInspector] public AnimatorOverrideController AnimatorOverrideController;
    //[SerializeField] private List<float> CD;
    private Collider[] colls;
    private Color color;
    private float coolDownTime;
    private AttackClip CurrentAttack;

    public void SetAttackCooldown(float cd)
    {
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
        if (Time.time > coolDownTime)
        {
            Attacks[i].PlayAttack(this);
            CurrentAttack = Attacks[i];
            //animator.SetTrigger("HandAttack");
            ////coolDownTime = Time.time + CD[0];
            //mover.SetMoving(false);
        }
    }

    public void LegAttack()
    {
        foreach (var item in colls)
        {
            if (item.transform.TryGetComponent(out Hittable hittable))
            {
                hittable.GetHit(1.0f, Vector3.zero);
                color = Color.red;
                return;
            }
        }
        //PlayerMover.SetMoving(true);
        //if (Physics.SphereCast(LegTransform.position, 0.5f, transform.forward, out RaycastHit hitInfo, 0.01f))
        //{
        //    Hit(hitInfo);
        //}
    }

    public void Attack()
    {
        var colls = Physics.OverlapSphere(CurrentAttack.HitTransform.position, 0.25f, AttackMask);
        foreach (var item in colls)
        {
            if (item.transform.TryGetComponent(out Hittable hittable))
            {
                item.transform.GetComponentInChildren<AttackPlayer>().SetAttackCooldown(CurrentAttack.HitStunTime);
                hittable.GetHit(1.0f, CurrentAttack.HitTransform.position);
                color = Color.red;
                return;
            }
        }

        //PlayerMover.SetMoving(true);

        //if(Physics.SphereCast(HandTransform.position, 0.5f, transform.forward, out RaycastHit hitInfo, 0.01f))
        //{
        //    Hit(hitInfo);
        //}
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
            Gizmos.DrawSphere(CurrentAttack.HitTransform.position, 0.25f);
        }
    }
}
