using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinOrLoseUI : MonoBehaviour
{
    public Button reStartButton;
    public TMP_Text timeText;
    public TMP_Text winText;
    public TMP_Text loseText;
    public UnityEvent<bool> onCalled;
    

    private Canvas m_controlCanvas;
    private float m_elapsedTime;

    private void Awake()
    {
        m_controlCanvas = GetComponent<Canvas>();
        if(m_controlCanvas.enabled)
            m_controlCanvas.enabled = false;

        // true�� ��� Win, false�� ��� Lose
        onCalled.RemoveAllListeners();
        onCalled.AddListener((isWin) =>
        {
            m_controlCanvas.enabled = true;
            Time.timeScale = 0.0f;
            GameManager.Instance.isPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (isWin)
            {
                winText.enabled = true;
                loseText.enabled = false;
            }
            else
            {
                winText.enabled = false;
                loseText.enabled = true;
            }

            int minutes = Mathf.FloorToInt(m_elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(m_elapsedTime - minutes * 60);
            timeText.text = string.Format("{0}M {1}S", minutes, seconds);

            
        });
    }

    private void Start()
    {
        reStartButton.onClick.RemoveAllListeners();
        reStartButton.onClick.AddListener(() => RestartGame());
    }

    private void FixedUpdate()
    {
        m_elapsedTime += Time.fixedDeltaTime;
    }

    void RestartGame()
    {
        // ���� Scene�� �ٽ� �ε�
        Debug.Log(Time.timeScale);

        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Debug.Log(Time.timeScale);
    }
}
