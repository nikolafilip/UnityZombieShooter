using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    [HideInInspector]public UnityEngine.AI.NavMeshAgent nav;
    [HideInInspector]public float speed;


    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    void Update() {
        if (enemyHealth.currentHealth > 0  && playerHealth.currentHealth > 0) {
            nav.SetDestination(player.position);
            nav.speed = speed;
        } else {
            nav.enabled = false;
        }
    }
}
