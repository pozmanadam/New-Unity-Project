using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyItemDrop : NetworkBehaviour {

    public GameObject[] itemPoll;
    public void ItemDrop() {
        if( Random.Range(0, 1) < 0.25f) {
            NetworkServer.Spawn((GameObject)Instantiate(itemPoll[Random.Range(0, itemPoll.Length)], transform.position, new Quaternion(0, 0, 0, 0)));
        }
    }
}
