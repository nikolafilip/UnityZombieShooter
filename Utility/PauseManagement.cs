using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseManagement : MonoBehaviour
{
    public GameObject pauseText;
    [HideInInspector]public bool isPaused = false;
    void Start() {
        pauseText.gameObject.SetActive(false); 
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            isPaused = !isPaused;
            if (isPaused) {
                Time.timeScale = 0f;
                pauseText.gameObject.SetActive(true);
            }  else {
                Time.timeScale = 1f;
                pauseText.gameObject.SetActive(false);
            }
        }
    }
}
