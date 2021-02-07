using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 100;                            
    public float currentHealth;                                   
    public Text healthText;
    public int score = 0;  
    [HideInInspector]public GameObject player;
    public float regenPeriod = 1f;
    public bool regenPerk = false;
    public Text regenPerkText;
    public int autoRegenAmount = 10;
                 
    void Awake() {
        currentHealth = startingHealth;
        setHealthText();
    }
    
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("healthRegen", regenPeriod, regenPeriod);
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.SetCursorLock(false);
            StaticVars.endKills = player.GetComponent<ScoreManagement>().kills;
            StaticVars.endLevel = player.GetComponent<ScoreManagement>().currentLevel;
            SceneManager.LoadScene(3);
        }
        setHealthText();
    }

    void setHealthText() {
        if (currentHealth <= 0) {
            healthText.color = Color.red;
            currentHealth = 0;
        } else {
            healthText.color = Color.black;
        }
        healthText.text = "Health: " + currentHealth + " / " + startingHealth;
    }

    void healthRegen() {
        if (currentHealth < startingHealth) {
            if (currentHealth + (regenPerk ? autoRegenAmount * 2 : autoRegenAmount) > startingHealth) {
                currentHealth = startingHealth;
            } else {
                currentHealth += (regenPerk ? autoRegenAmount * 2 : autoRegenAmount);
            }
            setHealthText();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == "Bottom") {
            TakeDamage(100);
        }
    }
}
