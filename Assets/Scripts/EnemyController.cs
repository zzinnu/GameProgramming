using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    private float yOffset = 25.0f;

    void Start()
    {
        StartCoroutine(ActionDecision());

        rb = GetComponent<Rigidbody>();

        waitTime = new WaitForSeconds(decisionTime);

        Destroy(gameObject, destroyDelay);
        
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
        bullet.GetComponent<FireBall>().targetPos = GameManager.Instance.player.transform.position;
        bullet.transform.position = transform.position;

        bullet.transform.position = transform.position + new Vector3(0.0f, yOffset, 0.0f);
    }

    void FireToRandom()
    {
        float randomX = Random.Range(-15.0f, 15.0f);
        float randomZ = Random.Range(-15.0f, 15.0f);
        Vector3 randomOffset = new Vector3(randomX, 0.0f, randomZ);

        GameObject bullet = Instantiate(enemyBulletPrefab);
        bullet.GetComponent<FireBall>().targetPos = GameManager.Instance.player.transform.position + randomOffset;

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
