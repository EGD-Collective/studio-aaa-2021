using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerFlash : MonoBehaviour
{
    //Layers
    [SerializeField]
    private LayerMask groundLayer, enemyLayer;

    //Light
    [SerializeField]
    private Light flashPointLight;
    private Animator flashAnimator;

    //Flash Variables
    [SerializeField]
    private float flashRange;
    [SerializeField]
    private float stunDuration;
    [SerializeField]
    private float flashCooldownBase;
    private float flashCooldown;

    //UI
    [SerializeField]
    private Slider flashCooldownBar;

    // Start is called before the first frame update
    void Start()
    {
        //Getting Components
        flashAnimator = flashPointLight.GetComponent<Animator>();

        //Setting timers
        flashCooldown = flashCooldownBase;
    }

    private void Update()
    {
        //Reducing CD
        flashCooldown -= Time.deltaTime;

        //UI
        flashCooldownBar.value = 1f - Mathf.Max(0f, flashCooldown / flashCooldownBase);
    }

    public void OnFlash()
    {
        if (flashCooldown <= 0)
        {
            //Flash Animation
            if (flashAnimator)
            {
                flashAnimator.SetTrigger("ToFlash");
            }

            //Checking for enemy
            RaycastHit[] enemyHit = Physics.SphereCastAll(transform.position, flashRange, Vector3.forward, flashRange, enemyLayer);
            for (int i = 0; i < enemyHit.Length; i++)
            {
                //Checking for hitting enemy
                if (enemyHit[i].transform.TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
                {
                    Vector3 toEnemy = enemyAI.transform.position - transform.position;

                    //If its not obstructed
                    if (!Physics.Raycast(transform.position, toEnemy, toEnemy.magnitude, groundLayer))
                    {
                        enemyAI.Stun(stunDuration);
                    }

                    break;
                }
            }

            //Resetting CD
            flashCooldown = flashCooldownBase;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, flashRange);
    }
}
