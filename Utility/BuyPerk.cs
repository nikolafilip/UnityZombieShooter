using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyPerk : MonoBehaviour
{
    public string type;
    public int price;
    public float RANGE = 10f;
    [HideInInspector]public GameObject player;
    [HideInInspector]public GameObject mainCamera;
    [HideInInspector]public ScoreManagement scoreManagement;
    private PauseManagement pauseManagement;
    private WeaponManagement weaponManagement;
    private GameObject pressToBuyText;


    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        pressToBuyText = GameObject.Find("Press To Buy Text");
        pauseManagement = player.GetComponent<PauseManagement>();
        scoreManagement = player.GetComponent<ScoreManagement>();
        weaponManagement = player.GetComponent<WeaponManagement>();
        player.GetComponent<PlayerHealth>().regenPerkText.gameObject.SetActive(false);
        weaponManagement.reloadSpeedPerkText.gameObject.SetActive(false);
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().movementSpeedPerkText.gameObject.SetActive(false);
    }

    void Update() {
        if (!pauseManagement.isPaused) {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit, RANGE)) {
                if (gameObject.tag == hit.collider.tag) {
                    pressToBuyText.GetComponent<Text>().text = "E - Buy perk - " + hit.collider.GetComponent<BuyPerk>().type + ": " + hit.collider.GetComponent<BuyPerk>().price;
                    if (Input.GetKeyDown("e")) {
                        buyPerk(hit);
                    }
                }
            }
        }
    }

    void buyPerk(RaycastHit hit) {
        if (scoreManagement.score >= price) {
            if (hit.collider.GetComponent<BuyPerk>().type == "Health regen" && !player.GetComponent<PlayerHealth>().regenPerk) {
                scoreManagement.substractScore(price);
                player.GetComponent<PlayerHealth>().regenPerk = true;
                player.GetComponent<PlayerHealth>().regenPerkText.gameObject.SetActive(true);
            }
            if (hit.collider.GetComponent<BuyPerk>().type == "Reload speed" && !weaponManagement.reloadSpeedPerk) {
                scoreManagement.substractScore(price);
                weaponManagement.reloadSpeedPerk = true;
                weaponManagement.reloadSpeedPerkText.gameObject.SetActive(true);
            }
            if (hit.collider.GetComponent<BuyPerk>().type == "Movement speed" && !player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().movementSpeedPerk) {
                scoreManagement.substractScore(price);
                player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().movementSpeedPerk = true;
                player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().movementSpeedPerkText.gameObject.SetActive(true);
            }
        } else {
            StartCoroutine(notEnoughMoneyWarning());
        }
    }

    private IEnumerator notEnoughMoneyWarning() {
        scoreManagement.scoreText.color = Color.red;
        yield return new WaitForSeconds(2);
        scoreManagement.scoreText.color = Color.black;
    }
}
