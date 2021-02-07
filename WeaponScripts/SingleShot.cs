using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleShot : MonoBehaviour
{
    public AudioSource GunShot;
    [HideInInspector]public GameObject player;
    private PauseManagement pauseManagement;
    private int currentAmmo = 0;
    public int damage;
    
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        pauseManagement = player.GetComponent<PauseManagement>();
    }

    void Update() {
        if (!pauseManagement.isPaused && !player.GetComponent<WeaponManagement>().isReloading) {
            currentAmmo = player.GetComponent<WeaponManagement>().getCurrentAmmo();
            if (currentAmmo != 0) {
                if (Input.GetMouseButtonDown(0)) {
                    shoot();
                }
            }
            if (Input.GetKeyDown("r")) {
                player.GetComponent<WeaponManagement>().reload();
            }
        }
    }

    private void shoot() {
        GunShot.Play();
        player.GetComponent<WeaponManagement>().substractAmmo();
        RaycastHit hit;
        if (Physics.Raycast(transform.parent.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
            if (hit.collider.tag == "Head") {
                EnemyHealth eHealth = hit.collider.transform.parent.transform.parent.GetComponent<EnemyHealth>();
                eHealth.TakeDamage(damage * 3);
                if (eHealth.currentHealth > 0) {
                    player.GetComponent<ScoreManagement>().addScore("Head");
                }
            } else if (hit.collider.tag == "Enemy") {
                EnemyHealth eHealth = hit.collider.GetComponent<EnemyHealth>();
                eHealth.TakeDamage(damage);
                if (eHealth.currentHealth > 0) {
                    player.GetComponent<ScoreManagement>().addScore("Body");
                }
            }
        }
    }
}
