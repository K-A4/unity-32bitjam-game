using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public AttackPlayer Attacker;
    public PlayerMovement PlayerMovement;
    public override void SetMoving(bool move)
    {
        PlayerMovement.SetMoving(move);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetButtonDown("HandAttack"))
        {
            Attacker.TriggerAttack(0);
        }

        if (Input.GetButtonDown("LegAttack"))
        {
            Attacker.TriggerAttack(1);
        }

        if (Input.GetButtonDown("Interact"))
        {
            Attacker.TriggerInteract();
        }
    }
}
