using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LoadNextDungeon : MonoBehaviour {

    RoomEntered roomEntered;
    void Start() {
        roomEntered = transform.root.GetComponent<RoomEntered>();
    }

    void Update() {
        if (!roomEntered.HasEnemy) {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<PolygonCollider2D>().enabled = true;
        }
    }
    void OnTriggerEnter2D(Collider2D entered) {
        if (entered.tag == "Player" && entered.GetComponent<NetworkIdentity>().isLocalPlayer)
            entered.GetComponent<PlayerController>().ReLoadDungeon();
    }
}
