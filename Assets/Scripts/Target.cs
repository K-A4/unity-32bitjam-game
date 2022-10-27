using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [HideInInspector] public float Angle;
    private Transform enemyTransform;
    public void SetEnemy(Transform enemy)
    {
        enemyTransform = enemy;
    }

    private void Update()
    {
        if (enemyTransform)
        {
            Vector3 targetDir = transform.position - enemyTransform.position;
            Angle = Vector3.SignedAngle(enemyTransform.forward, targetDir, Vector3.up);
        }
    }
}
