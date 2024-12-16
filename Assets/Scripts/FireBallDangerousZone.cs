using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireBallDangerousZone : MonoBehaviour
{
    // FireBall이 바닥과 충돌함
    public UnityEvent onCollided;
    public int damage = 10;
    bool detectionCollision = false;

    void EnableCollisionDetection()
    {
        detectionCollision = true;
    }

    private void OnTriggerStay(Collider other)
    {
        // Lifes TakeDamage
        if (detectionCollision)
        {
            if((1 << other.gameObject.layer) == LayerMask.GetMask("Player"))
            {
                other.GetComponent<Lifes>().TakeDamage(damage);
            }
        }
    }

    private void Awake()
    {
        onCollided.RemoveAllListeners();
        onCollided.AddListener(EnableCollisionDetection);
    }

    private void OnDestroy()
    {
        onCollided.RemoveListener(EnableCollisionDetection);
    }

    


}
