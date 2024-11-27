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

    // �Ѿ� �ݺ� �߻�
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
        // Ammo�� ������ Reload �ǽ�
        if (CurrentAmmo <= 0)
        {
            Reload();
            return;
        }

        CurrentAmmo--;                  // Ammo �Ҹ�
        anim.SetBool("Fire", true);     // �߻� �ִϸ��̼� ����

        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 100f, Color.red, 0.1f);
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1000.0f, enemyLayerMask))
        {
            Debug.Log(hit.transform.name);
            hit.collider.GetComponent<Lifes>().TakeDamage(damage);
        }
    }

    // Animation Event ȣ���, ���� ����� Event�� ó��
    public void Reload()
    {
        // Ammo�� �̹� ���� �������� Reload �Ұ�
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