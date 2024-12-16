using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public WinOrLoseUI winOrLoseUI;

    private Player m_player;

    public Player Player 
    {
        get
        {
            if (m_player == null)
            {
                m_player = FindAnyObjectByType<Player>();
            }

            return m_player;
        }

        set
        {
            m_player = value;
        }
    }
    public bool isPaused;

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

    private void OnDestroy()
    {
        Player.onDeath.RemoveAllListeners();
        EnemyManager.Instance.onChanged.RemoveAllListeners();
        WaveManager.Instance.onChanged.RemoveAllListeners();
    }

    private void Start()
    {
        Player.onDeath.RemoveAllListeners();
        Player.onDeath.AddListener(CheckLoseCondition);

        EnemyManager.Instance.onChanged.RemoveAllListeners();
        EnemyManager.Instance.onChanged.AddListener(CheckWinCondition);

        WaveManager.Instance.onChanged.RemoveAllListeners();
        WaveManager.Instance.onChanged.AddListener(CheckWinCondition);
    }

    void CheckLoseCondition()
    {
        if(Player.playerLife.CurrentLifes <= 0)
            winOrLoseUI.onCalled.Invoke(false);
    }

    void CheckWinCondition()
    {
        if(EnemyManager.Instance.enemyList.Count <= 0 /*&& WaveManager.Instance.enemySpawnerList.Count <= 0*/)
        {
            winOrLoseUI.onCalled.Invoke(true);
        }
    }
}
