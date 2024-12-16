using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireBall : MonoBehaviour
{
    public Vector3 targetPos;
    public GameObject dangerousZonePrefab;
    public GameObject impactPrefab;
    public float power = 80f;

    Rigidbody rb;
    [SerializeField] Vector3 dir;
    LayerMask targetLayer;
    RaycastHit hit;
    private GameObject dangerousZoneInstance;
    private Vector3 instancePos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 직접 맞았을 때
            other.GetComponent<Lifes>().TakeDamage(10);         // 10은 데미지
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            GameObject impact = Instantiate(impactPrefab);
            impact.transform.position = instancePos;

            // 영역내에 있으면 데미지 입히기
            dangerousZoneInstance.GetComponent<FireBallDangerousZone>().onCollided.Invoke();
        }
        Destroy(dangerousZoneInstance);
        Destroy(gameObject);
    }

    private void Start()
    {
        targetLayer = LayerMask.GetMask("Player") | LayerMask.GetMask("Ground");
        // target = GameManager.Instance.player.gameObject;

        dir = (targetPos - transform.position).normalized;

        RaycastHit[] hits = new RaycastHit[2];              // 최대 2개 raycast hit(ground, player)
        Ray ray = new Ray(transform.position, dir);

        int hitCount = Physics.RaycastNonAlloc(ray, hits, 10000.0f, targetLayer);    // 10000.0f는 적당히 큰 수

        // Instantiate할 때 사용할 position
        instancePos = Vector3.zero;

        // hits에 player가 있는지 검사
        // FireBall의 초기 위치가 낮으면 Player의 너무 뒤에서 Instance되기 때문에, Player가 있으면 Player위치를 저장함
        // 0번째가 player라면, 1번째는 ground임
        instancePos = hits[0].point;
        if (hits[0].collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            instancePos.y = hits[1].point.y;

        // Dangerous zone 생성
        dangerousZoneInstance = Instantiate(dangerousZonePrefab, instancePos, Quaternion.identity);

        // 이동
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(dir * power, ForceMode.Impulse);

        // 안맞으면 자동파괴
        Destroy(gameObject, 5f);
    }
}
