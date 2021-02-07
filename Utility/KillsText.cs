using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillsText : MonoBehaviour
{
    public TextMesh killsText;
    void Start()
    {
        killsText.text = "Kills: " + StaticVars.endKills;
    }
}
