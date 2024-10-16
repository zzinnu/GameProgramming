using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float lifes;
    public UnityEvent onDeath;

    private void Update()
    {
        if(lifes <= 0)
        {
            onDeath.Invoke();
            // Destroy(gameObject);
        }    
    }
}
