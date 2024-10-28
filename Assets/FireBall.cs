using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireBall : MonoBehaviour
{
    public Vector3 targetPos;
    public GameObject dangerousZonePrefab;
    public GameObject impactPrefab;

    Rigidbody rb;
    [SerializeField] Vector3 dir;
    LayerMask targetLayer;
    RaycastHit hit;
    private GameObject dangerousZoneInstance;

    private void OnTriggerEnter(Collider other)
    {
        GameObject impact = Instantiate(impactPrefab);
        impact.transform.position = hit.point;

        dangerousZoneInstance.GetComponent<FireBallDangerousZone>().onCollided.Invoke();

        Destroy(gameObject);
        Destroy(dangerousZoneInstance);
    }

    private void Start()
    {
        targetLayer = LayerMask.GetMask("Player");
        // target = GameManager.Instance.player.gameObject;

        dir = (targetPos - transform.position).normalized;

        // Dangerous zone 생성
        if(Physics.Raycast(transform.position, dir, out hit, 10000.0f, LayerMask.GetMask("Ground")))
        {
            dangerousZoneInstance = Instantiate(dangerousZonePrefab, hit.point, Quaternion.identity);
        }

        // 이동
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(dir * 1000.0f);
    }

    private void FixedUpdate()
    {
        rb.velocity *= 1.01f;
    }
}
