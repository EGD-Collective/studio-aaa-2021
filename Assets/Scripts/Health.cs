using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //Health Values
    [SerializeField]
    public float maxHealth;
    public float currentHealth;

    //Death
    [SerializeField]
    private UnityEvent onDeathActivated;
    private bool triggeredDeath = false;

    //UI
    [SerializeField]
    private Slider healthBar;
    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void LoseHealth(float amount)
    {
        Debug.Log("Lost health:" + gameObject.name);
        //Losing Health
        currentHealth = Mathf.Max(0f, currentHealth - amount);

        //UI
        if(healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }

        //Dying
        if(currentHealth == 0)
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
