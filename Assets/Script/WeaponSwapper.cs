using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponSwapper : NetworkBehaviour {


    int equippedWeapon;
    [SyncVar]
    int selectedWeapon;
    int weaponCount;

    PlayerWeaponShooter weaponShooter;

    GameObject hand;

    // Use this for initialization
    void Start () {
        hand = transform.Find("Hand").gameObject;
        weaponCount = hand.transform.childCount;
        equippedWeapon = 0;
        if(isLocalPlayer)
        CmdSetSelected(0);
        
        weaponShooter = transform.GetComponent<PlayerWeaponShooter>();
        weaponShooter.weaponStats = hand.transform.GetChild(equippedWeapon).GetComponent<WeaponStats>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer) {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
                if (equippedWeapon >= hand.transform.childCount - 1) {
                    selectedWeapon = 0;
                }
                else {
                    selectedWeapon++;
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
                if (equippedWeapon <= 0) {
                    selectedWeapon = hand.transform.childCount - 1;
                }
                else {
                    selectedWeapon--;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                selectedWeapon = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                selectedWeapon = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                selectedWeapon = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                selectedWeapon = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                selectedWeapon = 4;
            }
            if (hand.transform.childCount != weaponCount) {
                selectedWeapon = hand.transform.childCount - 1;
                weaponCount = hand.transform.childCount;
            }
        }
        if (selectedWeapon != equippedWeapon && selectedWeapon <= hand.transform.childCount-1) {
            if(isLocalPlayer)
            CmdSetSelected(selectedWeapon);
            EquipWeapon(selectedWeapon);
        }


    }

    private void EquipWeapon(int nextWeapon) {
        hand.transform.GetChild(equippedWeapon).gameObject.SetActive(false);
        hand.transform.GetChild(selectedWeapon).gameObject.SetActive(true);
        equippedWeapon = selectedWeapon;
         
        weaponShooter.weaponStats = hand.transform.GetChild(equippedWeapon).GetComponent<WeaponStats>();
    }
    [Command]
    void CmdSetSelected(int index) {
        selectedWeapon = index;
    }
}
