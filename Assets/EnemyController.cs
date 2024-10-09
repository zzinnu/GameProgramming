using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;

    public float destroyDelay;
    public float decisionTime;

    private Vector3 moveDir;
    private Quaternion lookRotation;
    private WaitForSeconds waitTime;

    

    void Start()
    {
        waitTime = new WaitForSeconds(decisionTime);

        Destroy(gameObject, destroyDelay);
        StartCoroutine(DirectionDecision());
    }

    void FixedUpdate()
    {
        transform.Translate(moveDir.x * moveSpeed * Time.deltaTime,
                            0.0f,
                            moveDir.z * moveSpeed * Time.deltaTime,
                            Space.World);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
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
