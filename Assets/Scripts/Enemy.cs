using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;

    void Start()
    {
        EnemyManager.Instance.AddEnemy(this);
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.RemoveEnemy(this);
    }
}
