using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float destroyDelay;

    void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    void FixedUpdate()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
