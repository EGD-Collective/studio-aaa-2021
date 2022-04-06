using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    public float maxHealth;
    public float currentHealth;

    [SerializeField]
    private UnityEvent onDeathActivated;
    private bool triggeredDeath = false;
    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void LoseHealth(float amount)
    {
        Debug.Log("Lost health:" + gameObject.name);
        //Losing Health
        currentHealth -= amount;

        //Dying
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath();
        }
    }
    public void OnDeath()
    {
        if (!triggeredDeath)
        {
            triggeredDeath = true;
            onDeathActivated.Invoke();
        }
    }
}
