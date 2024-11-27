using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public Lifes playerLife;
    public float maxStamina;
    public float currentStamina;
    public bool isUseStamina;
    public UnityEvent onDeath;

    public Gun gun;

    private void Awake()
    {
        currentStamina = maxStamina;
        if (gun == null)
            gun = GetComponentInChildren<Gun>();

        if(playerLife == null)
            playerLife = GetComponent<Lifes>();
    }

    private void FixedUpdate()
    {
        if(playerLife.lifes <= 0)
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
