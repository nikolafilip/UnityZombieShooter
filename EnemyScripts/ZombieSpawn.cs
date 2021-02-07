using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    public GameObject zombie;
    public Transform[] spawnPoints;
    public float spawnTime = 3f;
    [HideInInspector]public bool isActive = true;
    private GameObject player;

    public float speed = 1;
    public int damage = 10;
    public int health = 100;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn() {
        if (isActive && player.GetComponent<ScoreManagement>().zombieCount < player.GetComponent<ScoreManagement>().zombieLimit) {
            int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
            GameObject zombieCustom = Instantiate(zombie, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            zombieCustom.GetComponent<EnemyHealth>().startingHealth = health;
            zombieCustom.GetComponent<EnemyAttack>().attackDamage = damage;
            zombieCustom.GetComponent<EnemyMovement>().speed = speed;
            player.GetComponent<ScoreManagement>().zombieCount += 1;
        }
    }
}