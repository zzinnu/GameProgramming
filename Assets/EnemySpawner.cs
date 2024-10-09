using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] spawnObjects;
    public GameObject player;

    public float startTime;
    public float endTime;
    public float spawnRate;

    private BoxCollider spawnAreaCollider;
    void Spawn()
    {
        int objectIndex = Random.Range(0, spawnObjects.Length);

        float rangeX = spawnAreaCollider.bounds.size.x / 2;
        float rangeZ = spawnAreaCollider.bounds.size.z / 2;

        float randomX = Random.Range(-rangeX, rangeX);
        float randomZ = Random.Range(-rangeZ, rangeZ);

        Vector3 spawnPos = new Vector3(randomX, 0.0f, randomZ);
        spawnPos += transform.position;

        GameObject spawnObject = Instantiate(spawnObjects[objectIndex]);
        spawnObject.transform.position = spawnPos;
        spawnObject.transform.rotation = Quaternion.LookRotation((player.transform.position -  spawnPos).normalized);
    }

    void Start()
    {
        spawnAreaCollider = GetComponent<BoxCollider>();
        InvokeRepeating("Spawn", startTime, spawnRate);
        Invoke("CancelInvoke", endTime);
    }
}
