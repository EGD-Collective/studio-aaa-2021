using Assets.Scripts;
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
    private float revealDurationBase;
    private float revealDuration;
    [SerializeField]
    private float flashCooldownBase;
    private float flashCooldown;
    private float peakIntensity;



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
        peakIntensity = flashPointLight.intensity;
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
        if (flashPointLight.isActiveAndEnabled)
        {
            flashPointLight.intensity = peakIntensity * Mathf.Max(0, 1 - revealDuration / revealDurationBase);
            revealDuration += Time.deltaTime;
            if (revealDuration - revealDurationBase >= 0.1f)
                flashPointLight.gameObject.SetActive(false);
        }
    }

    public void OnFlash()
    {
        if (flashCooldown <= 0)
        {
            //Flash Animation
            //if (flashAnimator)
            //{
            //    flashAnimator.SetTrigger("ToFlash");
            //}

            flashPointLight.gameObject.SetActive(true);
            revealDuration = 0;
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
                    if (flashHit[i].transform.TryGetComponent(out FlashIndicator flashIndicator)
                        && flashHit[i].transform.TryGetComponent(out BaseClue clue) 
                        && !clue.Activated)
                    {
                        flashIndicator.ActivateIndicator(revealDurationBase);
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
