using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManagement : MonoBehaviour
{
    public GameObject AKPrefab;
    public GameObject GlockPrefab;
    public GameObject M4A1Prefab;
    public Dictionary<string, Vector3> weaponPositionsByTag;
    public Dictionary<string, GameObject> weaponPrefabByTag;
    public Dictionary<string, Quaternion> weaponRotationByTag;
    
    [HideInInspector]public string[] weaponNamePerSlot;
    [HideInInspector]public int[] weaponCurrentAmmoPerSlot;
    [HideInInspector]public int[] weaponTotalAmmoPerSlot;
    [HideInInspector]public int[] maxAmmoPerMagPerSlot;
    [HideInInspector]public float[] reloadTimePerSlot;
    
    // 0 - first slot, 1 - second slot
    [HideInInspector]public int activeSlot;

    public Text firstSlotText;
    public Text secondSlotText;
    public GameObject startingWeapon;
    public Text reloadText;
    
    [HideInInspector]public GameObject player;
    [HideInInspector]public GameObject mainCamera;
    private PauseManagement pauseManagement;

    [HideInInspector]public Text ammoText;
    public Text reloadSpeedPerkText;
    
    private bool secondSlotEmpty = true;
    [HideInInspector]public bool isReloading = false;

    private ScoreManagement scoreManagement;

    [HideInInspector]public bool reloadSpeedPerk = false;

    void Start() {
        fillInConstants();
        initiateArrays();

        scoreManagement = gameObject.GetComponent<ScoreManagement>();

        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        activeSlot = 0;

        GameObject newWeapon = GameObject.Instantiate(startingWeapon, mainCamera.transform.position, mainCamera.transform.rotation, mainCamera.transform) as GameObject;
        newWeapon.transform.localPosition = weaponPositionsByTag[startingWeapon.GetComponent<Constants>().Name];
        newWeapon.transform.localRotation = weaponRotationByTag[startingWeapon.GetComponent<Constants>().Name];

        weaponNamePerSlot[activeSlot] = newWeapon.GetComponent<Constants>().Name;
        maxAmmoPerMagPerSlot[activeSlot] = newWeapon.GetComponent<Constants>().maxAmmoPerMag;
        weaponTotalAmmoPerSlot[activeSlot] = newWeapon.GetComponent<Constants>().totalAmmo;
        weaponCurrentAmmoPerSlot[activeSlot] = maxAmmoPerMagPerSlot[activeSlot];
        reloadTimePerSlot[activeSlot] = newWeapon.GetComponent<Constants>().reloadSpeedSeconds;

        setAmmoText(weaponCurrentAmmoPerSlot[activeSlot], weaponTotalAmmoPerSlot[activeSlot]);

        weaponNamePerSlot[1] = "Empty";

        firstSlotText.text = weaponNamePerSlot[0];
        firstSlotText.color = Color.red;

        secondSlotText.text = weaponNamePerSlot[1];
        
        player = GameObject.FindGameObjectWithTag("Player");
        pauseManagement = player.GetComponent<PauseManagement>();

        reloadText.gameObject.SetActive(false);
    }

    void Update() {
        if (!pauseManagement.isPaused && !isReloading) {
            if (weaponCurrentAmmoPerSlot[activeSlot] == 0) {
                ammoText.color = Color.red;
            } else {
                ammoText.color = Color.black;
            }

            if (weaponNamePerSlot[1] != "Empty") {
                if (Input.GetKeyDown("q")) {
                    switchWeapon();
                }
            }
        }
    }

    void fillInConstants() {
        weaponPositionsByTag = new Dictionary<string, Vector3>();
        weaponPrefabByTag = new Dictionary<string, GameObject>();
        weaponRotationByTag = new Dictionary<string, Quaternion>();

        Vector3 position;
        position.x = 0.3291168f;
        position.y = -0.3225468f;
        position.z = 0.7810516f;

        Quaternion rotation = Quaternion.Euler(0,0,0);

        weaponPositionsByTag.Add("AK47", position);
        weaponPrefabByTag.Add("AK47", AKPrefab);
        weaponRotationByTag.Add("AK47", rotation);

        position.x = 0.193f;
        position.y = -0.202f;
        position.z = 0.356f;

        weaponPositionsByTag.Add("Glock", position);
        weaponPrefabByTag.Add("Glock", GlockPrefab);
        weaponRotationByTag.Add("Glock", rotation);

        position.x = 0.33f;
        position.y = -0.32f;
        position.z = 0.78f;

        rotation = Quaternion.Euler(0, 180, 0);

        weaponPositionsByTag.Add("M4A1", position);
        weaponPrefabByTag.Add("M4A1", M4A1Prefab);
        weaponRotationByTag.Add("M4A1", rotation);
    }

    void initiateArrays() {
        weaponNamePerSlot = new string[2];
        weaponCurrentAmmoPerSlot = new int[2];
        weaponTotalAmmoPerSlot = new int[2];
        maxAmmoPerMagPerSlot = new int[2];
        reloadTimePerSlot = new float[2];
    }

    public void reload() {
        if (maxAmmoPerMagPerSlot[activeSlot] != weaponCurrentAmmoPerSlot[activeSlot] && weaponTotalAmmoPerSlot[activeSlot] > 0) {
            StartCoroutine(reloadCoroutine());
        }
    }

    private IEnumerator reloadCoroutine() {
        reloadText.gameObject.SetActive(true);
        isReloading = true;
        yield return new WaitForSeconds(reloadSpeedPerk ? reloadTimePerSlot[activeSlot] / 2 : reloadTimePerSlot[activeSlot]);
        int ammoToReload = maxAmmoPerMagPerSlot[activeSlot] - weaponCurrentAmmoPerSlot[activeSlot];
        if (ammoToReload <= weaponTotalAmmoPerSlot[activeSlot]) {
            weaponCurrentAmmoPerSlot[activeSlot] = maxAmmoPerMagPerSlot[activeSlot];
            weaponTotalAmmoPerSlot[activeSlot] -= ammoToReload;
        } else {
            weaponCurrentAmmoPerSlot[activeSlot] += weaponTotalAmmoPerSlot[activeSlot];
            weaponTotalAmmoPerSlot[activeSlot] = 0;
        }
        ammoText.color = Color.black;
        setAmmoText(weaponCurrentAmmoPerSlot[activeSlot], weaponTotalAmmoPerSlot[activeSlot]);
        reloadText.gameObject.SetActive(false);
        isReloading = false;
    }
    
    public void substractAmmo() {
        weaponCurrentAmmoPerSlot[activeSlot] = weaponCurrentAmmoPerSlot[activeSlot] - 1;
        setAmmoText(weaponCurrentAmmoPerSlot[activeSlot], weaponTotalAmmoPerSlot[activeSlot]);
    }

    public int getCurrentAmmo() {
        return weaponCurrentAmmoPerSlot[activeSlot];
    }

    public void buyWeapon(string weaponName) {
        Constants weaponConstants = weaponPrefabByTag[weaponName].GetComponent<Constants>();
        if (scoreManagement.score >= weaponConstants.weaponCost) {
            setNewWeapon(weaponName);
        } else {
            if (weaponNamePerSlot[activeSlot] != weaponName && (weaponNamePerSlot[0] == weaponName || weaponNamePerSlot[1] == weaponName)) {
                switchWeapon();
            }
            StartCoroutine(notEnoughMoneyWarning());
        }
    }

    private IEnumerator notEnoughMoneyWarning() {
        scoreManagement.scoreText.color = Color.red;
        yield return new WaitForSeconds(2);
        scoreManagement.scoreText.color = Color.black;
    } 

    public void setNewWeapon(string weaponName) {
        if (activeSlot == 0 && weaponNamePerSlot[1] == "Empty") {
            if (weaponNamePerSlot[0] != weaponName) {
                activeSlot = 1;
                firstSlotText.color = Color.black;
                secondSlotText.color = Color.red;
                secondSlotEmpty = false;
            } else {
                if (!(weaponCurrentAmmoPerSlot[activeSlot] == maxAmmoPerMagPerSlot[activeSlot] && weaponTotalAmmoPerSlot[activeSlot] == weaponPrefabByTag[weaponNamePerSlot[activeSlot]].GetComponent<Constants>().totalAmmo)) {
                    weaponCurrentAmmoPerSlot[activeSlot] = maxAmmoPerMagPerSlot[activeSlot];
                    weaponTotalAmmoPerSlot[activeSlot] = weaponPrefabByTag[weaponNamePerSlot[activeSlot]].GetComponent<Constants>().totalAmmo;
                    scoreManagement.substractScore(weaponPrefabByTag[weaponNamePerSlot[activeSlot]].GetComponent<Constants>().weaponCost);
                }
            }
        }
        if (weaponNamePerSlot[0] != weaponName && weaponNamePerSlot[1] != weaponName && !secondSlotEmpty) {
            Destroy(GameObject.FindGameObjectWithTag("Weapon"));
            GameObject newWeapon = GameObject.Instantiate(weaponPrefabByTag[weaponName], mainCamera.transform.position, mainCamera.transform.rotation, mainCamera.transform) as GameObject;
            newWeapon.transform.localPosition = weaponPositionsByTag[weaponName];
            newWeapon.transform.localRotation = weaponRotationByTag[weaponName];

            Constants newWeaponConstants = newWeapon.GetComponent<Constants>();

            weaponNamePerSlot[activeSlot] = newWeaponConstants.Name;
            maxAmmoPerMagPerSlot[activeSlot] = newWeaponConstants.maxAmmoPerMag;
            if (!(weaponTotalAmmoPerSlot[activeSlot] == newWeaponConstants.totalAmmo && weaponCurrentAmmoPerSlot[activeSlot] == newWeaponConstants.maxAmmoPerMag)) {
                weaponTotalAmmoPerSlot[activeSlot] = newWeaponConstants.totalAmmo;
                weaponCurrentAmmoPerSlot[activeSlot] = maxAmmoPerMagPerSlot[activeSlot];
                scoreManagement.substractScore(newWeaponConstants.weaponCost);
            }
        } else if (!secondSlotEmpty) {

            if (weaponNamePerSlot[activeSlot] != weaponName) {
                switchWeapon();
            }
            if (!(weaponCurrentAmmoPerSlot[activeSlot] == maxAmmoPerMagPerSlot[activeSlot] && weaponTotalAmmoPerSlot[activeSlot] == weaponPrefabByTag[weaponNamePerSlot[activeSlot]].GetComponent<Constants>().totalAmmo)) {
                weaponCurrentAmmoPerSlot[activeSlot] = maxAmmoPerMagPerSlot[activeSlot];
                weaponTotalAmmoPerSlot[activeSlot] = weaponPrefabByTag[weaponNamePerSlot[activeSlot]].GetComponent<Constants>().totalAmmo;
                scoreManagement.substractScore(weaponPrefabByTag[weaponNamePerSlot[activeSlot]].GetComponent<Constants>().weaponCost);
            }
        }

        setAmmoText(weaponCurrentAmmoPerSlot[activeSlot], weaponTotalAmmoPerSlot[activeSlot]);
        setWeaponText();
    }

    private void setAmmoText(int currentAmmo, int totalAmmo) {
        ammoText.text = "Ammo: " + currentAmmo + " / " + totalAmmo;
    }

    void switchWeapon() {
        if (activeSlot == 0) {
            activeSlot = 1;
            firstSlotText.color = Color.black;
            secondSlotText.color = Color.red;
        } else {
            activeSlot = 0;
            firstSlotText.color = Color.red;
            secondSlotText.color = Color.black;
        }

        Destroy(GameObject.FindGameObjectWithTag("Weapon"));
        
        GameObject newWeapon = GameObject.Instantiate(weaponPrefabByTag[weaponNamePerSlot[activeSlot]], mainCamera.transform.position, mainCamera.transform.rotation, mainCamera.transform) as GameObject;
        newWeapon.transform.localPosition = weaponPositionsByTag[weaponNamePerSlot[activeSlot]];
        newWeapon.transform.localRotation = weaponRotationByTag[weaponNamePerSlot[activeSlot]];

        setAmmoText(weaponCurrentAmmoPerSlot[activeSlot], weaponTotalAmmoPerSlot[activeSlot]);
    }

    void setWeaponText() {
        firstSlotText.text = weaponNamePerSlot[0];
        secondSlotText.text = weaponNamePerSlot[1];
    }

    public int getPrice(string weaponName) {
        Constants weaponConstants = weaponPrefabByTag[weaponName].GetComponent<Constants>();
        return weaponConstants.weaponCost;
    }
}