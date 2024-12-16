using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public Camera playerCamera;
    public int damage = 1;
    public float fireRate = 0.1f;
    public int maxAmmo = 30;
    public int CurrentAmmo { get; private set; }

    public LayerMask enemyLayerMask;
    public bool IsRepeatFire { get; set; }

    public Animator anim;

    public CrossHair crossHair;
    public GameObject firePoint;

    [Header("Audio")]
    public AudioClip fireSound;
    public AudioClip[] reloadSound;

    public int poolSize = 60;
    public AudioMixer audioMixer;
    private Queue<AudioSource> audioSourcePool;
    private AudioSource m_audioSource;              // reload에서 사용

    [Header("Visual Effect")]
    public ParticleSystem muzzleFlash;

    private void Awake()
    {
        if(playerCamera == null)
            playerCamera = Camera.main;

        if(anim == null)
            anim = GetComponent<Animator>();

        CurrentAmmo = maxAmmo;

        // muzzleFlash 비활성화
        muzzleFlash.gameObject.SetActive(false);

        // Audio Pool 생성
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = new GameObject("fireAudioSource");
            go.transform.SetParent(transform);
            go.transform.position = firePoint.transform.position;
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.clip = fireSound;
            audioSource.volume = 1f;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 1f;
            audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
            audioSourcePool.Enqueue(audioSource);
        }

        // Reload Sound를 위한 AudioSource
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_audioSource.loop = false;
        m_audioSource.spatialBlend = 1f;
    }

   
    // 총알 반복 발사
    public IEnumerator FireCoroutine()
    {
        IsRepeatFire = true;

        while (IsRepeatFire)
        {
            Fire();
            yield return new WaitForSeconds(fireRate);
        }
    }

    public IEnumerator StopFireCoroutine()
    {
        IsRepeatFire = false;
        StopCoroutine(FireCoroutine());
        muzzleFlash.gameObject.SetActive(false);

        yield return null;
    }

    private void Fire()
    {
        // Ammo가 없으면 Reload 실시
        if (CurrentAmmo <= 0)
        {
            muzzleFlash.gameObject.SetActive(false);
            Reload();
            return;
        }

        CurrentAmmo--;                  // Ammo 소모
        anim.SetBool("Fire", true);     // 발사 애니메이션 실행

        // Audio Pool에서 AudioSource를 꺼내 사용
        if(audioSourcePool.Count > 0)
        {
            // Debug.Log("Bang");
            AudioSource audioSource = audioSourcePool.Dequeue();
            audioSource.Play();
            StartCoroutine(ReturnToPoolAfterPlay(audioSource));
        }

        // Muzzle Flash 효과
        muzzleFlash.gameObject.SetActive(true);
        muzzleFlash.Play();

        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 100f, Color.red, 0.1f);
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1000.0f, enemyLayerMask))
        {
            Debug.Log(hit.transform.name);

            Lifes enemyLife = hit.collider.GetComponent<Lifes>();
            if (enemyLife != null)
            {
                enemyLife.TakeDamage(damage);
                crossHair.Hit();
            }
                
        }
    }

    // Animation Event 호출용, 실제 기능은 Event로 처리
    public void Reload()
    {
        // Ammo가 이미 가득 차있으면 Reload 불가
        if (CurrentAmmo == maxAmmo)
            return;

        anim.SetBool("Reload", true);
        anim.SetBool("Fire", false);
    }

    private void ReloadAnimationEvent()
    {
        anim.SetBool("Reload", false);
        CurrentAmmo = maxAmmo;
    }

    public void SetAnimParamDefault()
    {
        anim.SetBool("Fire", false);
        anim.SetBool("Reload", false);
    }

    IEnumerator ReturnToPoolAfterPlay(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSourcePool.Enqueue(audioSource);
    }

    private void OutSound()
    {
        Debug.Log("Out");
        m_audioSource.clip = reloadSound[0];
        m_audioSource.Play();
    }

    private void InSound()
    {
        Debug.Log("In");
        m_audioSource.clip = reloadSound[1];
        m_audioSource.Play();
    }

}