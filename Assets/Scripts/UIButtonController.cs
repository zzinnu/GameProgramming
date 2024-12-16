using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Audio")]
    public AudioClip hoverAudio;
    public AudioClip clickAudio;

    [Header("Hover Effect")]
    [SerializeField] private GameObject hoverImage;
    private AudioSource m_audioSource;
    private Button m_button;

    private void Awake()
    {
        if(hoverImage == null)
            hoverImage = transform.Find("HoverImage").gameObject;
        hoverImage.SetActive(false);

        m_audioSource = GetComponent<AudioSource>();
        m_button = GetComponent<Button>();
    }

    private void Start()
    {
        m_button.onClick.RemoveAllListeners();
        m_button.onClick.AddListener(() => { m_audioSource.PlayOneShot(clickAudio); Debug.Log("Click"); });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_audioSource.PlayOneShot(hoverAudio);
        hoverImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        hoverImage.SetActive(false);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveAllListeners();
    }

    
}
