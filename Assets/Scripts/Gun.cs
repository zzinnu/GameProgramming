using UnityEngine;

public enum GunType { Basic, Shotgun }

public class Gun : MonoBehaviour
{
    public GunType gunType;
    public GameObject[] bulletPrefab;
    public Transform firePoint;
    public int shotgunPellets = 5;
    public float spreadAngle = 15f;

    public void Fire()
    {
        if (gunType == GunType.Basic)
        {
            FireBasic();
        }
        else if (gunType == GunType.Shotgun)
        {
            FireShotgun();
        }
    }

    void FireBasic()
    {
        Instantiate(bulletPrefab[0], firePoint.position, firePoint.rotation);
    }

    void FireShotgun()
    {
        for (int i = 0; i < shotgunPellets; i++)
        {
            Quaternion spreadRotation = Quaternion.Euler(firePoint.eulerAngles + new Vector3(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), 0));
            Instantiate(bulletPrefab[1], firePoint.position, spreadRotation);
        }
    }
}