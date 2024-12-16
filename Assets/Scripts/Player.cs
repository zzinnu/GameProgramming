using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Lifes playerLife;
    public float maxStamina;
    public float currentStamina;
    public bool isUseStamina;
    public UnityEvent onDeath;

    public Gun gun;

    [Header("No HP")]
    public AudioClip criticalMusic;
    public int percent = 30;

    [Header("Hit effect")]
    public Image hitImage;
    private int m_currentLifes;
    public AudioClip hitSound;

    public AudioSource[] audioSources;

    [Header("Action SFX")]
    public AudioClip jumpSound;

    private void Awake()
    {
        currentStamina = maxStamina;
        if (gun == null)
            gun = GetComponentInChildren<Gun>();

        if(playerLife == null)
            playerLife = GetComponent<Lifes>();

        // 0은 Music, 1은 SFX
        GameObject audioGameObject = transform.Find("Audio").gameObject;
        audioSources = new AudioSource[2];

        audioSources[0] = audioGameObject.transform.Find("Music").GetComponent<AudioSource>();
        audioSources[0].clip = criticalMusic;
        audioSources[0].loop = true;
        audioSources[0].volume = 1.0f;

        audioSources[1] = audioGameObject.transform.Find("SFX").GetComponent<AudioSource>();
        audioSources[1].clip = hitSound;
        audioSources[1].loop = false;
        audioSources[1].volume = 1.0f;
    }

    private void Start()
    {
        // Test용
        // playerLife.CurrentLifes = (int)(playerLife.maxLifes * 0.3);

        // Lifes에서 CurrentLifes를 Awake에서 초기화하기 때문에, Start에서 초기화
        m_currentLifes = playerLife.CurrentLifes;
    }

    private void FixedUpdate()
    {
        // Pause면 정지
        if(GameManager.Instance.isPaused)
            return;

        // Life 모두 소진 시 Game over
        if(playerLife.CurrentLifes <= 0)
        {
            onDeath.Invoke();
            // Destroy(gameObject);
        }

        // 30퍼 이하면 criticalMusic 재생
        if(playerLife.CurrentLifes <= (int)(playerLife.maxLifes * percent / 100))
        {
            if (!audioSources[0].isPlaying)
            {
                
                audioSources[0].Play();
            }
        }
        else
        {
            if(audioSources[0].isPlaying)
                audioSources[0].Stop();
        }

        // Debug.Log("m_currentLifes : " + m_currentLifes + " / playerLife.CurrentLifes : " + playerLife.CurrentLifes);
        // 맞았으면 hit effect
        if (m_currentLifes > playerLife.CurrentLifes)
        {
            m_currentLifes = playerLife.CurrentLifes;
            StartCoroutine(HitEffect());
            audioSources[1].Play();
        }

        // Stamina 회복
        if (!isUseStamina && currentStamina <= maxStamina)
        {
            currentStamina += 0.05f;
        }
    }

    IEnumerator HitEffect()
    {
        // Alpha값을 0.5로 올린 후, 0.5초동안 Fade out
        hitImage.color = new Color(hitImage.color.r, hitImage.color.g, hitImage.color.b, 0.5f);
        yield return hitImage.DOFade(0, 0.5f).SetEase(Ease.Linear).WaitForCompletion();
    }
}
