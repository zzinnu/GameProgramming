using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Button resumeButton;
    public Button exitButton;

    private Canvas m_pauseMenu;

    private void Awake()
    {
        m_pauseMenu = GetComponent<Canvas>();

        if(m_pauseMenu.enabled)
            m_pauseMenu.enabled = false;
    }

    private void Start()
    {
        // resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(() => Paused(false));

        // exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Paused(!m_pauseMenu.enabled);
        }
    }

    private void Paused(bool flag)
    {
        m_pauseMenu.enabled = flag;
        GameManager.Instance.isPaused = flag;

        if (flag)
        {
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
