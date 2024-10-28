using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float lifes;
    public float maxStamina;
    public float currentStamina;
    public bool isUseStamina;
    public UnityEvent onDeath;

    private void Awake()
    {
        currentStamina = maxStamina;
    }

    private void FixedUpdate()
    {
        if(lifes <= 0)
        {
            onDeath.Invoke();
            // Destroy(gameObject);
        }

        if(!isUseStamina && currentStamina <= maxStamina)
        {
            currentStamina += 0.05f;
        }
    }
}
