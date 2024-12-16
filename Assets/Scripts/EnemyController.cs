using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;

    public float destroyDelay;
    public float decisionTime;

    public GameObject enemyBulletPrefab;
    public Transform attackTarget;

    private Vector3 moveDir;
    private Quaternion lookRotation;
    private WaitForSeconds waitTime;
    private Rigidbody rb;
    private float yOffset = 10.0f;

    private void Awake()
    {
        waitTime = new WaitForSeconds(decisionTime);
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Destroy(gameObject, destroyDelay);
        StartCoroutine(ActionDecision());
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        while(true)
        {
            // dir, distance�� �������� ����, targetPos ���
            Vector3 randomDir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            randomDir.Normalize();

            Vector3 randomDistance = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f), Random.Range(-8.0f, 8.0f)) * 5;

            Vector3 targetPos = transform.position + new Vector3(randomDir.x * randomDistance.x, randomDir.y * randomDistance.y, randomDir.z * randomDistance.z);

            // �̵��ӵ��� ����� duration�� �������� ����
            float randomDuration = Random.Range(0.5f, 2.0f);

            // ���� �̵� ������ ������ ����� �ٽ� ����
            // x�� 100 ~ 200, y�� 50 ~ 100, z�� 40 ~ 120
            if (targetPos.x < 100.0f || targetPos.x > 200.0f || targetPos.y < 50.0f || targetPos.y > 100.0f || targetPos.z < 40.0f || targetPos.z > 120.0f)
                continue;


            Debug.Log("dir: " + randomDir + " distance: " + randomDistance + " duration: " + randomDuration);

            // dotween�� �̿��� �̵� + Player ������ �ٶ󺸵��� ȸ��
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(targetPos, randomDuration).SetEase(Ease.Linear));
            sequence.Join(transform.DORotateQuaternion(Quaternion.LookRotation(Vector3.Normalize(GameManager.Instance.Player.transform.position - transform.position)), 0.1f));

            yield return sequence.WaitForCompletion();
        }
    }

    IEnumerator ActionDecision()
    {
        yield return waitTime;

        int rand = Random.Range(0, 3);
        //int rand = 2;

        switch(rand)
        {
            // ���ڸ� 1��
            case 0:
                StartCoroutine(FireOne());
                break;

            // ���ڸ� 3��
            case 1:
                StartCoroutine(FireThree());
                break;

            // ���� 5��
            case 2:
                StartCoroutine(FireRandomFive());
                break;

            // �̵�(ȸ��)
            case 3:
                StartCoroutine(Move());
                break;
        }
    }

    void FireToPlayer()
    {
        GameObject bullet = Instantiate(enemyBulletPrefab);
        bullet.GetComponent<FireBall>().targetPos = GameManager.Instance.Player.transform.position;
        bullet.transform.position = transform.position;

        bullet.transform.position = transform.position + new Vector3(0.0f, yOffset, 0.0f);
    }

    void FireToRandom()
    {
        float randomX = Random.Range(-15.0f, 15.0f);
        float randomZ = Random.Range(-15.0f, 15.0f);
        Vector3 randomOffset = new Vector3(randomX, 0.0f, randomZ);

        GameObject bullet = Instantiate(enemyBulletPrefab);
        bullet.GetComponent<FireBall>().targetPos = GameManager.Instance.Player.transform.position + randomOffset;

        bullet.transform.position = transform.position + new Vector3(0.0f, yOffset, 0.0f);
    }

    void RandDirection()
    {

    }

    IEnumerator FireOne()
    {
        // 1�� ��ȯ
        Debug.Log("FireOne()");
        FireToPlayer();

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ActionDecision());
    }

    IEnumerator FireThree()
    {
        // 3�� ��ȯ
        Debug.Log("FireThree()");
        for(int i = 0; i < 3; i++)
        {
            FireToPlayer();
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ActionDecision());
    }

    IEnumerator FireRandomFive()
    {
        // ���� 5��
        Debug.Log("FireRandomFive()");
        for(int i = 0; i < 5; i++)
        {
            FireToRandom();
        }


        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ActionDecision());
    }

    IEnumerator Move()
    {


        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ActionDecision());
    }


    IEnumerator DirectionDecision()
    {
        while (true)
        {
            moveDir = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
            moveDir.Normalize();
            lookRotation = Quaternion.LookRotation(moveDir);

            yield return waitTime;
        }
    }


}
