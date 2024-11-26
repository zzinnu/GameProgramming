using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public UnityEvent onChanged;

    public List<Enemy> enemyList;

    public void AddEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
        onChanged.Invoke();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        onChanged.Invoke();
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Already exists EnemyManager");
            Destroy(gameObject);
        }
            
    }


}
