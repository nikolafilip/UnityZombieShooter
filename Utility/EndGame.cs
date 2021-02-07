using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public float RANGE = 10f;
    public int price = 5000;
    [HideInInspector]public GameObject player;
    [HideInInspector]public GameObject mainCamera;
    [HideInInspector]public ScoreManagement scoreManagement;
    private GameObject pressToBuyText;
    
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        scoreManagement = player.GetComponent<ScoreManagement>();
        pressToBuyText = GameObject.Find("Press To Buy Text");
        pressToBuyText.GetComponent<Text>().text = "";
    }

    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit, RANGE)) {
            if (gameObject.tag == hit.collider.tag) {
                pressToBuyText.GetComponent<Text>().text = "E - End game: " + price;
                if (Input.GetKeyDown("e")) {
                    endGame();
                }
            }
        }
    }

    void endGame() {
        if (scoreManagement.score >= price) {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.SetCursorLock(false);
            StaticVars.endKills = scoreManagement.kills;
            SceneManager.LoadScene(4);
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
