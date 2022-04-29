using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostGlimpse : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private bool reachedEnd = false;

    [SerializeField]
    private AudioClip[] spawnClips;
    private AudioSource spawnSource;

    [SerializeField]
    private GameObject demonModel;

    [SerializeField]
    private ParticleSystem smokeTrail;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        spawnSource = GetComponent<AudioSource>();
        spawnSource.clip = spawnClips[Random.Range(0, spawnClips.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.remainingDistance < 1.1f && !reachedEnd)
        {
            demonModel.SetActive(false);
        }
        //Destory object when particles are done
        if (smokeTrail.particleCount < 1 && !spawnSource.isPlaying && reachedEnd)
        {
            Destroy(gameObject);
        }
    }
}
