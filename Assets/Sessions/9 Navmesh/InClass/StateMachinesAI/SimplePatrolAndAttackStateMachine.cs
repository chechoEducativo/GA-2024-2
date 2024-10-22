using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class SimplePatrolAndAttackStateMachine : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Attack
    }

    [Serializable]
    public struct StateContext
    {
        public float searchRadius;
        public float searchDelaySecondsMin;
        public float searchDelaySecondsMax;
        [HideInInspector] public float currentSearchDelaySeconds;
        [HideInInspector] public float searchTimer;
        [HideInInspector] public Vector3? patrolTarget;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Transform enemy;
        [HideInInspector] public Animator animator;


        public float attackDelayMin;
        public float attackDelayMax;
        [HideInInspector] public float currentAttackDelay;
        [HideInInspector] public float attackTimer;
    }
    

    [SerializeField] private StateContext context;
    [SerializeField] private State currentState = State.Patrol;
    [SerializeField] private float enemyDetectionRadius;
    [SerializeField] private LayerMask enemyDetectionMask;

    private void Awake()
    {
        context.agent = GetComponent<NavMeshAgent>();
        context.animator = GetComponent<Animator>();
    }

    private void Patrol()
    {
        if (context.patrolTarget == null)
        {
            context.searchTimer += Time.deltaTime;
            if (context.searchTimer >= context.currentSearchDelaySeconds)
            {
                Vector2 randomDirection = Random.insideUnitCircle * context.searchRadius;
                context.patrolTarget = transform.position + Vector3.right * randomDirection.x +
                                       Vector3.forward * randomDirection.y;
                context.agent.destination = context.patrolTarget.Value;
                context.currentSearchDelaySeconds = Random.Range(context.searchDelaySecondsMin, context.searchDelaySecondsMax);
                context.searchTimer = 0;
            }
            return;
        }

        if (context.agent.remainingDistance < 4)
        {
            context.patrolTarget = null;
        }
    }

    private void Chase()
    {
        if (context.enemy == null) return;
        context.agent.destination = context.enemy.position;
    }

    private void Attack()
    {
        if (context.animator.GetFloat("ActiveAttack") < 0.2f)
        {
            context.attackTimer += Time.deltaTime;
            if (context.attackTimer >= context.currentAttackDelay)
            {
                context.animator.SetTrigger("Attack");
                context.attackTimer = 0;
                context.currentAttackDelay = Random.Range(context.attackDelayMin, context.attackDelayMax);
            }
        }
    }

    private void DetectEnemies()
    {
        if (context.enemy != null)
        {
            if (Vector3.Distance(context.enemy.position, transform.position) > enemyDetectionRadius)
            {
                context.enemy = null;
            }
        }
        else
        {
            Collider[] nearestEnemies = Physics.OverlapSphere(transform.position, enemyDetectionRadius, enemyDetectionMask);
            if (nearestEnemies.Length > 0)
            {
                context.enemy = nearestEnemies[0].transform;
            }
        }
    }
    
    private void Update()
    {
        DetectEnemies();
        
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (context.enemy != null)
                {
                    currentState = State.Chase;
                }
                break;
            case State.Chase:
                Chase();
                if (context.enemy == null)
                {
                    currentState = State.Patrol;
                }
                else
                {
                    if (context.agent.remainingDistance <= 4)
                    {
                        currentState = State.Attack;
                    }
                }
                break;
            case State.Attack:
                Attack();
                if (context.enemy == null)
                {
                    currentState = State.Patrol;
                }
                else
                {
                    if (Vector3.Distance(transform.position, context.enemy.position) > 4)
                    {
                        currentState = State.Chase;
                    }
                }
                break;
        }
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, context.searchRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyDetectionRadius);
    }
#endif
}
