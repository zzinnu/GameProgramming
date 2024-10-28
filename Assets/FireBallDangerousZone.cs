using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireBallDangerousZone : MonoBehaviour
{
    // FireBall�� �ٴڰ� �浹��
    public UnityEvent onCollided;
    public float damage;
    bool detectionCollision = false;

    void EnableCollisionDetection()
    {
        detectionCollision = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(detectionCollision)
        {
            if((1 << other.gameObject.layer) == LayerMask.GetMask("Player"))
            {
                other.gameObject.GetComponent<Player>().lifes -= damage;
            }
        }
    }

    private void Awake()
    {
        onCollided.AddListener(EnableCollisionDetection);
    }

    private void OnDestroy()
    {
        onCollided.RemoveListener(EnableCollisionDetection);
    }

    


}
