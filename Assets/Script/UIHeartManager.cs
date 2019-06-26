using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIHeartManager : NetworkBehaviour {

    private int lastHealth;
    private int healthPerHeart;
    public PlayerStats playerHealth;
    public Sprite[] hearthImages; // 0-full 1-half 2-empty
    private Image[] hearths;

    // Use this for initialization
    void Start() {
        lastHealth = 0;
        healthPerHeart = 2;

        hearths = new Image[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            hearths[i] = transform.GetChild(i).GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update() {
        if (playerHealth == null) {
            return;
        }
        if (lastHealth != playerHealth.playerCurrentHealth) {
            UpdateHearths();
            lastHealth = playerHealth.playerCurrentHealth;
        }       
    }

    private void UpdateHearths() {
        int i;
        //set heart contaniers
        for ( i = 0; i < playerHealth.playerFullHealth / healthPerHeart; i++) {
            if (hearths[i].enabled == false) { hearths[i].enabled = true; }
        }
        //set full hearts
        for (i = 0; i < playerHealth.playerCurrentHealth / healthPerHeart; i++) {
            hearths[i].sprite = hearthImages[0];
        }
        //set half heart
        if (playerHealth.playerCurrentHealth % healthPerHeart >= healthPerHeart/2) {
            hearths[i].sprite = hearthImages[1];
            i++;
        }
        //set empty hearts
        for (int j = i; j < playerHealth.playerFullHealth; j++) {
            hearths[j].sprite = hearthImages[2];
        }
    }
}
