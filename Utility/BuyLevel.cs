using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyLevel : MonoBehaviour
{
    public float RANGE = 10f;
    public int price = 1000;
    public int level = 1;
    [HideInInspector]public GameObject player;
    [HideInInspector]public GameObject mainCamera;
    [HideInInspector]public ScoreManagement scoreManagement;
    private PauseManagement pauseManagement;
    private GameObject pressToBuyText;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        scoreManagement = player.GetComponent<ScoreManagement>();
        pressToBuyText = GameObject.Find("Press To Buy Text");
        pressToBuyText.GetComponent<Text>().text = "";
        pauseManagement = player.GetComponent<PauseManagement>();
    }

    void Update() {
        if (!pauseManagement.isPaused) {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit, RANGE)) {
                if (gameObject.tag == hit.collider.tag) {
                    pressToBuyText.GetComponent<Text>().text = "E - Buy Level " + level + ": " + price;
                    if (Input.GetKeyDown("e")) {
                        buyLevel();
                    }
                }
            }
        }
    }

    void buyLevel() {
        if (scoreManagement.score >= price) {
            scoreManagement.substractScore(price);
            scoreManagement.currentLevel += 1;
            GameObject[] barriers = GameObject.FindGameObjectsWithTag("BarrierLevel" + level);
            foreach (GameObject barrier in barriers) {
                Destroy(barrier);
            }
            foreach(GameObject zombieSpawnPoint in scoreManagement.zombieSpawnsByLevel[level]) {
                zombieSpawnPoint.GetComponent<ZombieSpawn>().isActive = true;
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
