using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetWeapon : MonoBehaviour
{
    public float RANGE = 10f;
    private GameObject pressToBuyText;
    [HideInInspector]public GameObject player;
    [HideInInspector]public GameObject mainCamera;
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        pressToBuyText = GameObject.Find("Press To Buy Text");
        pressToBuyText.GetComponent<Text>().text = "";
    }

    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit, RANGE)) {
            if (gameObject.tag == hit.collider.tag) {
                pressToBuyText.GetComponent<Text>().text = "E - Buy " + gameObject.tag + ": " + player.GetComponent<WeaponManagement>().getPrice(gameObject.tag);
                if (Input.GetKeyDown("e")) {
                    player.GetComponent<WeaponManagement>().buyWeapon(gameObject.tag);
                }
            }
            if (hit.collider.tag != "M4A1" && hit.collider.tag != "AK47" && hit.collider.tag != "Glock" && hit.collider.tag != "EndGame" 
                && !hit.collider.tag.Contains("BarrierLevel") && hit.collider.tag != "Perk") {
                pressToBuyText.GetComponent<Text>().text = "";
            }
        } else {
            pressToBuyText.GetComponent<Text>().text = "";
        }
    }
}
