using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeText : MonoBehaviour
{
    private TMP_Text m_max;
    private TMP_Text m_current;

    private void Awake()
    {
        m_max = transform.Find("Max").GetComponent<TMP_Text>();
        m_current = transform.Find("Current").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        m_max.text = GameManager.Instance.Player.playerLife.maxLifes.ToString();
        m_current.text = GameManager.Instance.Player.playerLife.CurrentLifes.ToString();
    }

    private void Update()
    {
        m_current.text = GameManager.Instance.Player.playerLife.CurrentLifes.ToString();
    }
}
