using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{

    public GameObject player;
    public GameObject akPrefab;
    private GameObject weapon;
    void Start() {
        weapon = GameObject.FindGameObjectWithTag("Weapon");
    }

    void Update() {
        if (Input.GetKeyDown("q")) {
            weapon = GameObject.FindGameObjectWithTag("Weapon");
            Destroy(weapon);
            GameObject newWeapon = GameObject.Instantiate(akPrefab, player.transform.position, player.transform.rotation, player.transform) as GameObject;
            Vector3 newPosition;

            newPosition.x = 0.3291168f;
            newPosition.y = -0.3225468f;
            newPosition.z = 0.7810516f;

            newWeapon.transform.localPosition = newPosition;
        }
    }


}
