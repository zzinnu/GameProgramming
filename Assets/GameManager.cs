using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player player;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Already exists GameManager");
            Destroy(gameObject);
        }
            
    }

    private void Start()
    {
        player.onDeath.AddListener(CheckLoseCondition);

        EnemyManager.Instance.onChanged.AddListener(CheckWinCondition);
        WaveManager.Instance.onChanged.AddListener(CheckWinCondition);
    }

    void CheckLoseCondition()
    {
        SceneManager.LoadScene("LoseScene");
    }

    void CheckWinCondition()
    {
        if(EnemyManager.Instance.enemyList.Count <= 0 && WaveManager.Instance.enemySpawnerList.Count <= 0)
        {
            SceneManager.LoadScene("WinScene");
        }
    }
}
