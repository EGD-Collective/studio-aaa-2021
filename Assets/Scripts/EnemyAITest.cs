using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAITest : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public GameObject player;
    public LayerMask groundLayer, playerLayer;
    public float sightRange;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        bool playerInRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        if (playerInRange)
        {
            Debug.Log("chasing");
            navMeshAgent.SetDestination(player.transform.position);
        }
        else { 
            navMeshAgent.SetDestination(transform.position);
        }
    }
}
