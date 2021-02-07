using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagement : MonoBehaviour
{
    public int pointsPerBodyShot;
    public int pointsPerHeadShot;
    [HideInInspector]public int score = 0;
    [HideInInspector]public int kills = 0;
    public Text killsText;
    public Text scoreText;
    [HideInInspector]public Dictionary<int, GameObject[]> zombieSpawnsByLevel;
    [HideInInspector]public int zombieCount = 0;
    public int zombieLimit = 30;
    [HideInInspector]public int currentLevel = 1;

    void Start()
    {
        setScoreText();
        setKillsText();

        zombieSpawnsByLevel = new Dictionary<int, GameObject[]>();
        for (int i = 2; i <= 8; i++) {
            zombieSpawnsByLevel.Add(i, GameObject.FindGameObjectsWithTag("ZombieSpawnLevel" + i));
            foreach (GameObject zombieSpawn in zombieSpawnsByLevel[i]) {
                zombieSpawn.GetComponent<ZombieSpawn>().isActive = false;
            }
        }
    }

    void Update() {
        
    }

    public void addScore(string bodyPart) {
        if (bodyPart == "Head") {
            score += pointsPerHeadShot;
        }
        if (bodyPart == "Body") {
            score += pointsPerBodyShot;
        }
        setScoreText();
    }

    void setScoreText() {
        scoreText.text = "Score: " + score;
    }

    public void substractScore(int amount) {
        score -= amount;
        setScoreText();
    }

    public void addKill() {
        kills++;
        setKillsText();
    }

    public void setKillsText() {
        killsText.text = "Kills: " + kills;
    }
}
