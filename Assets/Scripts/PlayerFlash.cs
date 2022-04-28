using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerFlash : MonoBehaviour
{
    //Layers
    [SerializeField]
    private LayerMask groundLayer;

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
    private float revealDuration;
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
        if (flashCooldownBar)
        {
            flashCooldownBar.value = 1f - Mathf.Max(0f, flashCooldown / flashCooldownBase);
        }
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
            RaycastHit[] flashHit = Physics.SphereCastAll(transform.position, flashRange, Vector3.forward, flashRange);
            for (int i = 0; i < flashHit.Length; i++)
            {
                //Checking not blocked
                Vector3 toHit = (flashHit[i].transform.position + (Vector3.up * 1.6f)) - (transform.position + (Vector3.up * 1.6f));
                if (!Physics.Raycast(transform.position + (Vector3.up * 1.6f), toHit, toHit.magnitude, groundLayer))
                {
                    //Hit Enemy
                    if (flashHit[i].transform.TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
                    {
                        enemyAI.Stun(stunDuration);
                    }
                    //Checking for hitting base clue
                    if (flashHit[i].transform.TryGetComponent<FlashIndicator>(out FlashIndicator flashIndicator))
                    {
                        flashIndicator.ActivateIndicator(revealDuration);
                    }
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
