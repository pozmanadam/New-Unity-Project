using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TreasureSpawn : NetworkBehaviour {

    public GameObject[] TreasurePoll;

	void Start () {
        if (!isServer) return;
        var item = Instantiate(TreasurePoll[Random.Range(0, TreasurePoll.Length)]);
        item.transform.position = transform.position;
        NetworkServer.Spawn(item);
	}
}
