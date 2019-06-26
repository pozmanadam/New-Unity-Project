using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerWeaponShooter : NetworkBehaviour
{

    public WeaponStats weaponStats;
    public GameObject bullet;
    private WeaponStats BonusStats;


	// Use this for initialization
	void Start () {
        BonusStats = transform.GetComponent<WeaponStats>();
    }
	
	// Update is called once per frame
    
	void Update () {
        if (!isLocalPlayer || Menu.menuIsActive) { return; }
        if (weaponStats.cooldownCounter > 0)
        weaponStats.cooldownCounter -= Time.deltaTime;
        if (Input.GetMouseButton(0) && weaponStats.cooldownCounter <= 0 && weaponStats.ammunition != 0) {
            weaponStats.cooldownCounter = weaponStats.fireRate + BonusStats.fireRate;

            switch (weaponStats.shootType) {
                case WeaponStats.ShootTypes.Single: {
                        CmdSingleShoot();
                        break;
                    }
                case WeaponStats.ShootTypes.Spread: {
                        CmdSpreadShoot();
                        break;
                    }
            }
            weaponStats.ammunition--;
        }
	}
    [Command]
    void CmdSingleShoot() {
        var newBullet = Instantiate(bullet, weaponStats.spawnPoint.position, weaponStats.spawnPoint.rotation) as GameObject;
        newBullet.GetComponent<PlayerBulletController>().damage = weaponStats.damage + BonusStats.damage;
        newBullet.GetComponent<PlayerBulletController>().speed = weaponStats.speed + BonusStats.speed;
        newBullet.GetComponent<PlayerBulletController>().accuracy = weaponStats.accuracy +BonusStats.accuracy;
        NetworkServer.Spawn(newBullet);
    }
    [Command]
    void CmdSpreadShoot() {
        for (int i = 0; i < 7; i++) {
            var newBullet = Instantiate(bullet, weaponStats.spawnPoint.position, weaponStats.spawnPoint.rotation) as GameObject;
            newBullet.GetComponent<PlayerBulletController>().damage = weaponStats.damage + BonusStats.damage;
            newBullet.GetComponent<PlayerBulletController>().speed = weaponStats.speed + BonusStats.speed;
            newBullet.GetComponent<PlayerBulletController>().accuracy = weaponStats.accuracy + BonusStats.accuracy;
            NetworkServer.Spawn(newBullet);
        }
    }
}
