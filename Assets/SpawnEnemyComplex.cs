using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyComplex : MonoBehaviour
{
    public GameObject enemy;

    void Start()
    {
        // Spawn an enemy every x seconds where x is the last argument given
        InvokeRepeating("SpawnEnemyRandomlyComplex", 0f, 4f);
    }

    void SpawnEnemyRandomlyComplex()
    {
        // We don't want the enemies to spawn in front of the player
        var distanceBetweenPlayer = Vector3.Distance(GetComponent<Transform>().position, GameObject.FindGameObjectWithTag("Player").transform.position);

        // We don't want the PC to blow up if the enemies are not eliminated so we also check the total number of enemies spawned

        if (GameObject.FindGameObjectsWithTag("Enemy").Length < 25 && distanceBetweenPlayer > 15)
        {
            var enemySpawned = Instantiate(enemy, transform.position, Quaternion.identity);
            enemySpawned.GetComponent<EnemyBehaviour>().player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
