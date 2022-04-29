using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostGlimpse : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private SkinnedMeshRenderer[] skinnedMesh;
    private bool reachedEnd = false;

    [SerializeField]
    private ParticleSystem smokeTrail;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        skinnedMesh = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.remainingDistance < 1.1f && !reachedEnd)
        {
            //Make invisible
            for (int i = 0; i < skinnedMesh.Length; i++)
            {
                skinnedMesh[i].enabled = false;
            }
            reachedEnd = true;
        }
        //Destory object when particles are done
        if (smokeTrail.particleCount < 1 && reachedEnd)
        {
            Destroy(gameObject);
        }
    }
}
