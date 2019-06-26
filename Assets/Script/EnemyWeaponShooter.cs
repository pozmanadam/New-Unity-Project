using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyWeaponShooter : NetworkBehaviour {

    WeaponStats weaponStats;
    public EnemyBulletController bullet;

    public bool isFiring;


    // Use this for initialization
    void Start() {
        isFiring = false;
        weaponStats = transform.Find("Hand").GetChild(0).GetComponent<WeaponStats>();
    }

    // Update is called once per frame
    void Update() {
        if (!isServer) return;
        if (weaponStats.cooldownCounter > 0)
        weaponStats.cooldownCounter -= Time.deltaTime;
        if (isFiring && weaponStats.cooldownCounter <= 0 ) {
            weaponStats.cooldownCounter = weaponStats.fireRate;

            switch (weaponStats.shootType) {
                case WeaponStats.ShootTypes.Single: {
                        SingleShoot();
                        break;
                    }
                case WeaponStats.ShootTypes.Spread: {
                        SpreadShoot();
                        break;
                    }
            }
        }
    }
    
    void SingleShoot() {
        var newBullet = Instantiate(bullet, weaponStats.spawnPoint.position, weaponStats.spawnPoint.rotation) as EnemyBulletController;
        newBullet.damage = (int)weaponStats.damage;
        newBullet.speed = weaponStats.speed;
        newBullet.accuracy = weaponStats.accuracy;
        NetworkServer.Spawn(newBullet.gameObject);
    }
    void SpreadShoot() {
        for (int i = 0; i < 4; i++) {
            var newBullet = Instantiate(bullet, weaponStats.spawnPoint.position, weaponStats.spawnPoint.rotation) as EnemyBulletController;
            newBullet.damage = (int)weaponStats.damage;
            newBullet.speed = weaponStats.speed;
            newBullet.accuracy = weaponStats.accuracy;
            NetworkServer.Spawn(newBullet.gameObject);
        }
    }
}
