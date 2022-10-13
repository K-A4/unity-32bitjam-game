using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float spawnRate;
    [SerializeField] private GameObject MonsterPrefab;
    private float spawnTime;


    private void Update()
    {
        if (Time.time > spawnTime)
        {
            spawnTime = Time.time + spawnRate;
            Spawn(4);
        }
    }

    private void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var r = Random.value;
            r *= Mathf.PI * 2;
            var pos = new Vector3(Mathf.Cos(r) * radius, Mathf.Sin(r) * radius, -1) ;
            Instantiate(MonsterPrefab, pos, Quaternion.identity, transform);
        }
    }
}
