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
            // dir, distance를 랜덤으로 설정, targetPos 계산
            Vector3 randomDir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            randomDir.Normalize();

            Vector3 randomDistance = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f), Random.Range(-8.0f, 8.0f)) * 5;

            Vector3 targetPos = transform.position + new Vector3(randomDir.x * randomDistance.x, randomDir.y * randomDistance.y, randomDir.z * randomDistance.z);

            // 이동속도를 담당할 duration도 랜덤으로 설정
            float randomDuration = Random.Range(0.5f, 2.0f);

            // 만약 이동 지점이 영역을 벗어나면 다시 설정
            // x축 100 ~ 200, y축 50 ~ 100, z축 40 ~ 120
            if (targetPos.x < 100.0f || targetPos.x > 200.0f || targetPos.y < 50.0f || targetPos.y > 100.0f || targetPos.z < 40.0f || targetPos.z > 120.0f)
                continue;


            Debug.Log("dir: " + randomDir + " distance: " + randomDistance + " duration: " + randomDuration);

            // dotween을 이용한 이동 + Player 방향을 바라보도록 회전
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
            // 제자리 1개
            case 0:
                StartCoroutine(FireOne());
                break;

            // 제자리 3개
            case 1:
                StartCoroutine(FireThree());
                break;

            // 랜덤 5곳
            case 2:
                StartCoroutine(FireRandomFive());
                break;

            // 이동(회피)
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
        // 1개 소환
        Debug.Log("FireOne()");
        FireToPlayer();

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ActionDecision());
    }

    IEnumerator FireThree()
    {
        // 3개 소환
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
        // 랜덤 5곳
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
