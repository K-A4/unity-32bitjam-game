using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float MoveSpeed;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        var toPlayer = (player.position - transform.position);
        transform.position += toPlayer.normalized * MoveSpeed * Time.deltaTime;
    }
}
