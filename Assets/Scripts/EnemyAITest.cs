using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAITest : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public GameObject player;
    public LayerMask groundLayer, playerLayer, waypointLayer;

    //Patrol pathing
    public Transform patrolPath;
    int patrolIndex = 0;

    //State Machine
    enum BasicEnemyAIStates
    {
        IDLE,
        CHASE,
        ATTACK
    }
    enum BasicEnemyAttackStates
    {
        COOLDOWN,
        STARTUP,
        ATTACK,
        RECOVERY
    }

    BasicEnemyAIStates currentAIState;
    BasicEnemyAttackStates currentAttackState;
    
    //Attack
    public float attackCDBase;
    float attackCD;
    public float attackStartupBase;
    float attackStartup;
    public float attackRecoverBase;
    float attackRecover;
    public float attackDurationBase;
    float attackDuration;

    public float attackRange;
    public float attackSize;

    //Seeing Variables
    public float sightRangeBase;
    float sightRange;
    Transform lastSeen;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        //Initalization
        currentAIState = BasicEnemyAIStates.IDLE;
        currentAttackState = BasicEnemyAttackStates.COOLDOWN;

        sightRange = sightRangeBase;
        attackCD = 0f;
        attackStartup = attackStartupBase;
        attackDuration = attackDurationBase;
        attackRecover = attackRecoverBase;
    }

    // Update is called once per frame
    void Update()
    {
        //Calculating sight range
        Vector3 toPlayer = player.transform.position - transform.position;
        if (toPlayer.magnitude < sightRange) 
        {
            sightRange = toPlayer.magnitude;
        }
        else
        {
            sightRange = sightRangeBase;
        }
        //bool playerInRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        bool playerInSightRange = Physics.Raycast(transform.position, toPlayer, sightRange, playerLayer);
        bool terrainInRange = Physics.Raycast(transform.position, toPlayer, sightRange, groundLayer);
        bool playerInRange = Physics.CheckSphere(transform.position + transform.forward.normalized * attackRange, attackSize / 4, playerLayer);
        bool hitPlayer = Physics.CheckSphere(transform.position + transform.forward.normalized * attackRange, attackSize, playerLayer);

        //Recuding attack cooldown
        attackCD -= Time.deltaTime;
        //Statemachine
        switch (currentAIState)
        {
            case BasicEnemyAIStates.IDLE:
                //Player in ranage chase
                if(playerInSightRange && !terrainInRange)
                {
                    currentAIState = BasicEnemyAIStates.CHASE;
                    lastSeen = player.transform;
                    navMeshAgent.SetDestination(lastSeen.position);
                    Debug.Log("Chasing");
                }
                break;
            case BasicEnemyAIStates.CHASE:
                if (playerInSightRange)
                {
                    if (terrainInRange)
                    {
                        //Travelling to last seen location
                        if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
                        {
                            currentAIState = BasicEnemyAIStates.IDLE;
                            lastSeen = null;
                            Debug.Log("Idling");
                        }
                    }
                    else
                    {
                        //AI Setting and remembering last seen
                        lastSeen = player.transform;
                        navMeshAgent.SetDestination(lastSeen.position);
                    }
                }
                else
                {
                    //Traveling to last seen location
                    if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
                    {
                        currentAIState = BasicEnemyAIStates.IDLE;
                        lastSeen = null;
                        Debug.Log("Idling");
                    }
                }
                //Debug.Log(toPlayer.magnitude + " : " + attackRange);
                
                //Attacking if in range
                if (playerInRange)
                {
                    currentAIState = BasicEnemyAIStates.ATTACK;
                    navMeshAgent.SetDestination(transform.position);
                    Debug.Log("Attack");
                }
                break;
            case BasicEnemyAIStates.ATTACK:
                //Statemachine for attack cycle
                switch (currentAttackState)
                {
                    case BasicEnemyAttackStates.COOLDOWN:

                        //Chaning state to chasing when out of range
                        if(!playerInRange)
                        {
                            currentAIState = BasicEnemyAIStates.CHASE;
                            lastSeen = player.transform;
                            navMeshAgent.SetDestination(lastSeen.position);
                            Debug.Log("Chasing");
                        }

                        if(attackCD < 0f)
                        {
                            currentAttackState = BasicEnemyAttackStates.STARTUP;
                            attackCD = attackCDBase;
                        }
                        break;
                    case BasicEnemyAttackStates.STARTUP:
                        attackStartup -= Time.deltaTime;
                        if (attackStartup < 0f)
                        {
                            currentAttackState = BasicEnemyAttackStates.ATTACK;
                            attackStartup = attackStartupBase;
                        }
                        break;
                    case BasicEnemyAttackStates.ATTACK:
                        attackDuration -= Time.deltaTime;
                        if (attackDuration < 0f)
                        {
                            if (hitPlayer)
                            {
                                Debug.Log("HIT!"); //Do something when hitting player
                            }
                            currentAttackState = BasicEnemyAttackStates.RECOVERY;
                            attackDuration = attackDurationBase;
                        }
                        break;
                    case BasicEnemyAttackStates.RECOVERY:
                        attackRecover -= Time.deltaTime;
                        if (attackRecover < 0f)
                        {
                            currentAttackState = BasicEnemyAttackStates.COOLDOWN;
                            attackRecover = attackRecoverBase;
                        }
                        break;
                }
                break;
        }


        //Old AI, need to use waypoint stuff

        //if (playerInRange && !terrainInRange)
        //{
        //    Debug.Log("chasing");
        //    foundPlayer = true;
        //    lastSeen = player.transform;
        //    navMeshAgent.SetDestination(player.transform.position);
        //}
        //else if(foundPlayer)
        //{
        //    navMeshAgent.SetDestination(transform.position);
        //}
        //else if (patrolPath != null ){
        //    Transform currentPoint = patrolPath.GetChild(patrolIndex);
        //    navMeshAgent.SetDestination(currentPoint.position);
        //    Collider[] waypointContacts = Physics.OverlapSphere(transform.position, 0.5f, waypointLayer);

        //    Debug.Log(patrolIndex);

        //    for (int i = 0; i < waypointContacts.Length; i++)
        //    {
        //        Debug.Log(waypointContacts[i].name + " : " + currentPoint.name);
        //        if (waypointContacts[i].name == currentPoint.name)
        //        {
        //            patrolIndex++;
        //            if(patrolIndex > patrolPath.childCount-1)
        //            {
        //                patrolIndex = 0;
        //            }
        //        }
        //    }
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawRay(new Ray(transform.position, (player.transform.position - transform.position)));
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, sightRangeBase);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward.normalized * attackRange, attackSize);
    }
}
