using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Camera playerCamera;
    public float damage = 1f;
    public float fireRate = 0.1f;
    public int maxAmmo = 30;
    public int CurrentAmmo { get; private set; }

    public LayerMask enemyLayerMask;
    public bool IsRepeatFire { get; set; }


    public Animator anim;

    private void Awake()
    {
        if(playerCamera == null)
            playerCamera = Camera.main;

        if(anim == null)
            anim = GetComponent<Animator>();

        CurrentAmmo = maxAmmo;
    }

    // 총알 반복 발사
    public IEnumerator FireCoroutine()
    {
        while (IsRepeatFire)
        {
            Fire();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Fire()
    {
        // Ammo가 없으면 Reload 실시
        if (CurrentAmmo <= 0)
        {
            Reload();
            return;
        }

        CurrentAmmo--;                  // Ammo 소모
        anim.SetBool("Fire", true);     // 발사 애니메이션 실행

        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 100f, Color.red, 0.1f);
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1000.0f, enemyLayerMask))
        {
            Debug.Log(hit.transform.name);
            hit.collider.GetComponent<Lifes>().TakeDamage(damage);
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
}