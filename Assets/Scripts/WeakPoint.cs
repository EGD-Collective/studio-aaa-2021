using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    public SphereCollider sphereCollider;
    public float focusDownDur;
    public float size;

    private ParticleSystem particleSystem;

    public bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        EnemyAI enemyAI = GetComponentInParent<EnemyAI>();
        particleSystem = GetComponent<ParticleSystem>();
        
        sphereCollider = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
        sphereCollider.isTrigger = true;
        sphereCollider.radius = enemyAI.weakPointSize;
        focusDownDur = enemyAI.focusDownDurBase;
        particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetActive(bool condition)
    {
        active = condition;
        if(condition)
        {
            particleSystem.Play();
        }
        else
        {
            particleSystem.Stop();
        }
    }

    public void Hit()
    {
        focusDownDur -= Time.fixedDeltaTime;
        if(focusDownDur <= 0 && active == true)
        {
            SetActive(false);
        }
    }
}
