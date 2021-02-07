using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    void Start() {
        
    }

    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            SceneManager.LoadScene(0);
        }
    }
}
