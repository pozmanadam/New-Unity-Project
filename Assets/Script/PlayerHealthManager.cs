using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHealthManager : MonoBehaviour {


    PlayerStats playerStats;

    void Start() {
        playerStats = GetComponent<PlayerStats>();
    }

    public void HurtPlayer(int damageCaused) {
        playerStats.playerCurrentHealth -= damageCaused;
        if(playerStats.playerCurrentHealth <= 0) {
            Cursor.visible = true;
            GameObject.Find("OnlineMenu").GetComponent<Menu>().LeaveRoom();
        }
    }

    public void HealPlayer(int health) {
        playerStats.playerCurrentHealth += health;
        if (playerStats.playerCurrentHealth > playerStats.playerFullHealth) {
            playerStats.playerCurrentHealth = playerStats.playerFullHealth;
        }
    }

    public void MaxHealthUp(int health) {
        playerStats.playerFullHealth += health;
        if (playerStats.playerFullHealth > playerStats.playerMaxHealth) {
            playerStats.playerFullHealth = playerStats.playerMaxHealth;
        }
    }

}
