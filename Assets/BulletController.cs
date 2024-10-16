using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float damage;
    public float destroyDelay;
    public LayerMask hitLayer;

    private Rigidbody rb;

    void Start()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyDelay);
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime, hitLayer))
        {
            // 面倒 贸府 肺流
            Destroy(gameObject);

            Debug.Log("Hit: " + hit.collider.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            enemy.hp -= damage;
            if(enemy.hp <= 0)
                Destroy(hit.transform.gameObject);
        }
    }
}
