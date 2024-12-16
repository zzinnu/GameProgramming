using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifes : MonoBehaviour
{
    public int maxLifes = 100;
    public int CurrentLifes { get; set; }

    private void Awake()
    {
        CurrentLifes = maxLifes;
    }

    public void TakeDamage(int amount)
    {
        CurrentLifes -= amount;
    }
}
