using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifes : MonoBehaviour
{
    public float lifes = 100.0f;

    public void TakeDamage(float amount)
    {
        lifes -= amount;
    }
}
