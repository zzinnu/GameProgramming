using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    public UnityEvent onChanged;
    public List<EnemySpawner> enemySpawnerList;

    public void AddWave(EnemySpawner spawner)
    {
        enemySpawnerList.Add(spawner);
        onChanged.Invoke();
    }

    public void RemoveWave(EnemySpawner spawner)
    {
        enemySpawnerList.Remove(spawner);
        onChanged.Invoke();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Already exists WaveManager");
            Destroy(gameObject);
        }
            
    }
}
