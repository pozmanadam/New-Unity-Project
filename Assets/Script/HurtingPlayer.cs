using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HurtingPlayer : NetworkBehaviour {

    public int damage;

    void OnTriggerEnter2D(Collider2D entered) {
        if (entered.gameObject.tag == "Player") {
            if (!entered.transform.GetComponent<NetworkIdentity>().isServer) {
                return;
            }
            entered.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damage);
        }
    }
}
