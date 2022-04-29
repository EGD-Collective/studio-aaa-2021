using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private float focusDownDurationBase;
    private float focusDownDuration;
    private EnemyAI enemyAI;

    [SerializeField]
    private ParticleSystem focusParticle;
    // Start is called before the first frame update
    void Start()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
        sphereCollider = GetComponent<SphereCollider>();

        sphereCollider.radius = enemyAI.weakPointSize;
        focusDownDurationBase = enemyAI.focusDownDurationBase;
        focusDownDuration = focusDownDurationBase;
    }

    public void SetWeakPointActive(bool condition)
    {
        gameObject.SetActive(condition);
        focusDownDuration = focusDownDurationBase;
    }


    public void Hit(float damageAmount)
    {
        focusDownDuration -= Time.fixedDeltaTime;
        if (!focusParticle.isPlaying)
        {
            focusParticle.Play();
        }
        if(focusDownDuration <= 0)
        {
            enemyAI.SetPlayerDamageTaken(damageAmount);
            enemyAI.RemoveWeakpointFromList(this);
            Destroy(gameObject);
        }
    }
}
