using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PickUpController : NetworkBehaviour {

    private PlayerStats playerStats;
    private WeaponStats weaponStats;
    private PlayerHealthManager playerHealth;

    // Use this for initialization
    void Start () {
        playerStats = GetComponent<PlayerStats>();
        weaponStats = GetComponent<WeaponStats>();
        playerHealth = GetComponent<PlayerHealthManager>();
    }

    void OnTriggerEnter2D(Collider2D entered) {
        if (entered.tag == "WeaponItem") {
            entered.tag = "Weapon";
            Destroy(entered.transform.GetComponent<Collider2D>());
            entered.transform.SetParent(transform.Find("Hand"));
            entered.transform.localPosition = new Vector2(0f, 0f);
            entered.transform.localRotation = new Quaternion(0, entered.transform.localRotation.y, 0, 0);

        }
        if(entered.tag == "Item") {
            if (entered.GetComponent<WeaponStats>() != null) {
                var weaponBonusStat = entered.GetComponent<WeaponStats>();

                weaponStats.accuracy += weaponBonusStat.accuracy;
                weaponStats.damage += weaponBonusStat.damage;
                weaponStats.fireRate += weaponBonusStat.fireRate;
                weaponStats.speed += weaponBonusStat.speed;
                weaponStats.ammunition += weaponStats.ammunition;

            }
            if (entered.GetComponent<PlayerStats>() != null) {
                var playerBonusStat = entered.GetComponent<PlayerStats>();

                playerStats.moveSpeed += playerBonusStat.moveSpeed;
                if (isServer) {
                    playerHealth.MaxHealthUp(playerBonusStat.playerFullHealth);
                    playerHealth.HealPlayer(playerBonusStat.playerCurrentHealth);
                }
            }
            NetworkServer.Destroy(entered.gameObject);
        }
    }
}
