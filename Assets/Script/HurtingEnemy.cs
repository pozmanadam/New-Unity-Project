using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HurtingEnemy : NetworkBehaviour {

    public float damage;

    public void OnTriggerEnter2D(Collider2D entered) {
        if (!transform.root.GetComponent<NetworkIdentity>().isServer) {
            return;
        }
        if (entered.gameObject.tag == "Enemy") {
            entered.gameObject.GetComponent<EnemyHealthManager>().HurtEnemy(damage);
        }
    }
}
