using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public Image m_FilledImage;

    void Update()
    {
        m_FilledImage.fillAmount = GameManager.Instance.Player.currentStamina / GameManager.Instance.Player.maxStamina;
    }
}
