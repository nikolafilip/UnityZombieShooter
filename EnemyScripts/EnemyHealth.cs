using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public int scoreValue = 10;
    [HideInInspector]public ScoreManagement scoreManagement;
    [HideInInspector]public GameObject player;

    Animator anim;
    CapsuleCollider capsuleCollider;
    bool isDead;


    void Awake() {
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        currentHealth = startingHealth;
    }

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        scoreManagement = player.GetComponent<ScoreManagement>();
    }

    public void TakeDamage(int amount) {
        if (isDead) {
            return;
        }
        currentHealth -= amount;
        if (currentHealth <= 0) {
            Death();
        }
    }


    void Death() {
        isDead = true;
        capsuleCollider.isTrigger = true;
        anim.SetTrigger("Dead");
        StartSinking();
        scoreManagement.addKill();

        player.GetComponent<ScoreManagement>().zombieCount -= 1;
    }

    public void StartSinking() {
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject, 2f);
    }
}
