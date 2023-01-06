using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;

    void Start()
    {
        // Spawn an enemy every x seconds where x is the last argument given
        InvokeRepeating("SpawnEnemyRandomly", 0f, 4f);
    }
    

    void SpawnEnemyRandomly()
    {
        // We don't want the PC to blow up if the enemies are not eliminated so we also check the total number of enemies spawned
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < 10)
        {
            var enemySpawned = Instantiate(enemy, GetComponent<Transform>());
            enemySpawned.GetComponent<EnemyBehaviour>().player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
    }
}
