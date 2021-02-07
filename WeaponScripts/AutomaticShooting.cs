using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutomaticShooting : MonoBehaviour
{
    public AudioSource GunShot;
    [HideInInspector]public GameObject player;
    private PauseManagement pauseManagement;
    private float nextActionTime = 0.0f;
    public float shootPeriod = 0.21f;
    public int damage;
    private int currentAmmo = 0;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        pauseManagement = player.GetComponent<PauseManagement>();
    }

    void Update() {   
        if (!pauseManagement.isPaused && !player.GetComponent<WeaponManagement>().isReloading) {
            currentAmmo = player.GetComponent<WeaponManagement>().getCurrentAmmo();
            if (currentAmmo != 0) {
                if (Input.GetMouseButtonDown(0)) {
                    GunShot.loop = true;
                    GunShot.Play();
                    shoot();
                }
                if (Input.GetMouseButton(0)) {
                    if (Time.time > nextActionTime) {
                        shoot();
                    }
                }
                if (Input.GetMouseButtonUp(0)) {
                    GunShot.loop = false;
                }
            } else {
                GunShot.loop = false;
            }
            if (Input.GetKeyDown("r")) {
                player.GetComponent<WeaponManagement>().reload();
            }
        }
    }


    private void shoot() {
        RaycastHit hit;
        player.GetComponent<WeaponManagement>().substractAmmo();
        nextActionTime = Time.time + shootPeriod;
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
