using UnityEngine;
using UnityEngine.Networking;

public class RoomEntered : NetworkBehaviour { 

    [System.Serializable]
    public class EnemyContainer {
        public GameObject type;
        public Vector3 pos;
    }

    public EnemyContainer[] enemies;
    [SyncVar] public bool HasEnemy;

    [SyncVar] public bool RigthIsGate;
    [SyncVar] public bool LeftIsGate;
    [SyncVar] public bool DownIsGate;
    [SyncVar] public bool UpIsGate;

    // Use this for initialization
    void Start() {
        if (!isServer) return;
        if (enemies.Length == 0) HasEnemy = false;
        else HasEnemy = true;
    }

    // Update is called once per frame
    void Update() {
        if (!isServer || !HasEnemy) return;
        for (int i = 0; i < enemies.Length; i++) {
            if (enemies[i].type != null) {
                return;
            }
        }
        HasEnemy = false;
    }
    void OnTriggerEnter2D(Collider2D entered) {
        if (entered.transform.root.GetComponent<NetworkIdentity>().isLocalPlayer) {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            if (HasEnemy) {
                GetComponent<Grid>().enabled = true;

                if (!entered.transform.root.GetComponent<NetworkIdentity>().isServer) return;
                for (int i = 0; i < enemies.Length; i++) {
                    if (enemies[i].type != null) {
                        enemies[i].type = Instantiate(enemies[i].type);
                        enemies[i].type.transform.position = transform.position + enemies[i].pos;

                        enemies[i].type.GetComponent<EnemyController>().grid = transform.GetComponent<Grid>();
                        NetworkServer.Spawn(enemies[i].type);
                    }
                }
            }
        }
    } 
    void OnTriggerExit2D(Collider2D exited) {
        if (exited.GetComponent<NetworkIdentity>().isLocalPlayer) {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}

   
