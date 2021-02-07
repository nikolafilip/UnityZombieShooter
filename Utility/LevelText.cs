using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    public TextMesh levelText;
    void Start() {
        levelText.text = "Level: " + StaticVars.endLevel;
    }
}
