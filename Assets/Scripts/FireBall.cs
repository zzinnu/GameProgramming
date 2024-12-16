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
            // ���� �¾��� ��
            other.GetComponent<Lifes>().TakeDamage(10);         // 10�� ������
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            GameObject impact = Instantiate(impactPrefab);
            impact.transform.position = instancePos;

            // �������� ������ ������ ������
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

        RaycastHit[] hits = new RaycastHit[2];              // �ִ� 2�� raycast hit(ground, player)
        Ray ray = new Ray(transform.position, dir);

        int hitCount = Physics.RaycastNonAlloc(ray, hits, 10000.0f, targetLayer);    // 10000.0f�� ������ ū ��

        // Instantiate�� �� ����� position
        instancePos = Vector3.zero;

        // hits�� player�� �ִ��� �˻�
        // FireBall�� �ʱ� ��ġ�� ������ Player�� �ʹ� �ڿ��� Instance�Ǳ� ������, Player�� ������ Player��ġ�� ������
        // 0��°�� player���, 1��°�� ground��
        instancePos = hits[0].point;
        if (hits[0].collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            instancePos.y = hits[1].point.y;

        // Dangerous zone ����
        dangerousZoneInstance = Instantiate(dangerousZonePrefab, instancePos, Quaternion.identity);

        // �̵�
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(dir * power, ForceMode.Impulse);

        // �ȸ����� �ڵ��ı�
        Destroy(gameObject, 5f);
    }
}
