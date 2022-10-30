using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public LayerMask LookMask;

    [SerializeField] private float lookRadius;

    private int targetIndex;
    private Target currentTarget;
    public PlayerMovement PlayerMover;

    private void Update()
    {
        UpdateTargeting();
        PlayerMover.SetTarget(currentTarget ? currentTarget.transform : null);

    }

    private void UpdateTargeting()
    {
        var enem = Physics.OverlapSphere(transform.position, lookRadius, LookMask);
        var targetList = new List<Target>();

        for (int i = 0; i < enem.Length; i++)
        {
            if (enem[i].TryGetComponent(out Target target))
            {
                if (currentTarget != target || currentTarget == null)
                {
                    currentTarget = target;
                }
                
                target.SetEnemy(transform);
                targetList.Add(target);
            }
        }

        if (targetList.Count == 0)
        {
            currentTarget = null;
        }


        //targetList.Sort((a, b) => a.Angle > b.Angle ? 1 : 0);
        targetList.Sort((a, b) => a.Distance > b.Distance ? 1 : 0);

        //targetIndex = Mathf.Clamp(targetIndex, 0, Mathf.Max(0, enem.Length - 1));
        currentTarget = targetList[0];
        //if (Input.GetButtonDown("TargetLeft"))
        //{
        //    var index = targetList.IndexOf(currentTarget);
        //    index = index - 1 < 0 ? targetList.Count - 1 : --index;
        //    currentTarget = targetList[index];
        //}

        //if (Input.GetButtonDown("TargetRight"))
        //{
        //    var index = targetList.IndexOf(currentTarget);
        //    index = index + 1 > targetList.Count - 1 ? 0 : ++index;
        //    currentTarget = targetList[index];
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
