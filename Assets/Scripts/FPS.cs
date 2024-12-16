using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public float updateInterval = 0.5f;

    private TMP_Text fpsText;

    private void Awake()
    {
        fpsText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        StartCoroutine(TextFPS());
    }

    IEnumerator TextFPS()
    {
        while(true)
        {
            float fps = 1 / Time.unscaledDeltaTime;
            fpsText.text = fps.ToString("F0");
            yield return new WaitForSeconds(updateInterval);
        }
        
    }
}
