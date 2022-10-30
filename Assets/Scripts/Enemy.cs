using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState
{
    Idle,
    Chase,
    Fighting
}

public class Enemy : Entity
{
    public AttackController EnemyAttacker;
    public NavMeshAgent Agent;
    public Animator Anim;
    public Hittable Hittable;
    [SerializeField] private float speed;
    [SerializeField] private float attackRadius;
    [SerializeField] private float stanceRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private bool moveOnStart;
    [SerializeField] private float LayDuration;
    private float LayTime;
    private bool canMove;
    private bool laying;
    private float distanceToTarget;
    private Transform player;
    private EnemyState state;
    private MovementStates movementState;

    public override void SetMoving(bool move)
    {
        if (canMove)
        {
            if (Agent)
            {
                Agent.speed = move ? speed : 0;
            }
        }
    }

    public void SetCanMoving(bool move)
    {
        canMove = move;
    }

    public override void SetLaying(bool lay, float layTime)
    {
        Anim.SetBool("Lay", lay);
        LayTime = Time.time + layTime;
        SetMoving(!lay);
        laying = true;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = EnemyState.Idle;
    }

    private void Start()
    {
        if (Agent)
        {
            Agent.speed = 0;
        }
        Hittable.OnGetHit.AddListener(PlayHittAnim);
        canMove = moveOnStart;
    }

    private void PlayHittAnim(int d)
    {
        Anim.SetTrigger("GetHit");
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Idle: Idleing(); break;
            case EnemyState.Chase: Chasing(); break;
            case EnemyState.Fighting: Fighting(); break;
            default:
                break;
        }

        if (Time.time > LayTime)
        {
            Anim.SetBool("Lay", false);
        }

        distanceToTarget = Vector3.Distance(transform.position, player.position);
        if (Agent)
        {
            Agent.SetDestination(player.position);
            Anim.SetFloat("State", (int)movementState);
            Anim.SetFloat("Speed", Agent.velocity.magnitude);
        }
    }

    private void Idleing()
    {
        SetMoving(false);
        if (distanceToTarget < chaseRadius)
        {
            state = EnemyState.Chase;
        }
        movementState = MovementStates.Normal;
    }

    private void Chasing()
    {
        movementState = MovementStates.Normal;

        if (distanceToTarget < stanceRadius)
        {
            state = EnemyState.Fighting;
        }

        if (distanceToTarget > chaseRadius)
        {
            state = EnemyState.Idle;
        }
    }

    private void Fighting()
    {
        movementState = MovementStates.Stance;
        transform.rotation = Quaternion.LookRotation((player.position - transform.position).normalized,Vector3.up);
        if (Agent)
        {
            Agent.speed = 0.5f * Agent.speed;
        }
        if (distanceToTarget < attackRadius)
        {
            RandomAttack();
        }

        if (distanceToTarget > stanceRadius)
        {
            state = EnemyState.Chase;
        }
    }

    private void RandomAttack()
    {
        EnemyAttacker.TriggerAttack((int)(Random.value * EnemyAttacker.Attacks.Count));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,chaseRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stanceRadius);
    }
}
