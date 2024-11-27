using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Lifes enemyLifes;

    private void Awake()
    {
        if (enemyLifes == null)
            enemyLifes = GetComponent<Lifes>();
    }

    void Start()
    {
        EnemyManager.Instance.AddEnemy(this);
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.RemoveEnemy(this);
    }

    private void Update()
    {
        // lifes가 0이 되면 사망
        if (enemyLifes.lifes <= 0)
        {
            Destroy(gameObject);
        }
    }
}
