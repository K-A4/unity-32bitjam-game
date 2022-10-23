using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    public Animator animator;
    public Transform LegTransform;
    public Transform HandTransform;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("HandAttack");
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("LegAttack");
        }
    }

    public void LegAttack()
    {
        if (Physics.SphereCast(LegTransform.position, 0.1f, Vector3.up, out RaycastHit hitInfo))
        {
            Hit(hitInfo);
        }
    }

    public void HandAttack()
    {
        if(Physics.SphereCast(HandTransform.position, 0.1f, Vector3.up, out RaycastHit hitInfo))
        {
            Hit(hitInfo);
        }
    }

    private static void Hit(RaycastHit hitInfo)
    {
        if (hitInfo.transform.TryGetComponent(out Hittable hittable))
        {
            hittable.GetHit(1.0f, hitInfo.point);
        }
    }
}
