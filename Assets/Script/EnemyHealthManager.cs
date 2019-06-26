using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyHealthManager : NetworkBehaviour {

    public float EnemyCurrentHealth;
    private EnemyItemDrop enemyItemDrop;

    // Use this for initialization
    void Start() {
        enemyItemDrop = GetComponent<EnemyItemDrop>();
    }
    
    public void HurtEnemy(float damageCaused) {
        EnemyCurrentHealth -= damageCaused;
        if(EnemyCurrentHealth <= 0) {
            enemyItemDrop.ItemDrop();
            NetworkServer.Destroy(transform.gameObject);
        }
    }
}
