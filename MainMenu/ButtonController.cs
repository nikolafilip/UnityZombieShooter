using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {
    public string ButtonType;
    void Start(){
        GetComponent<Renderer>().material.color = Color.black;
    }

    void OnMouseEnter(){
        GetComponent<Renderer>().material.color = Color.red;
    }

    void OnMouseExit() {
        GetComponent<Renderer>().material.color = Color.black;
    }

    void OnMouseUp() {
        if (ButtonType == "Start") {
            SceneManager.LoadScene(1);
        }
        if (ButtonType == "Quit") {
            Application.Quit();
        }
        if (ButtonType == "Credits") {
            SceneManager.LoadScene(2);
        }
        if (ButtonType == "Main Menu") {
            SceneManager.LoadScene(0);
        }
    }
}