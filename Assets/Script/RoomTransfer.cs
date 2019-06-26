using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class RoomTransfer : NetworkBehaviour {
    public enum Directions { Up, Down, Left, Rigth };
    public Directions direction;

    public bool isGate;

    List<GameObject> GateBlocks = new List<GameObject>();
    RoomEntered roomEntered;

    void Start() {
        roomEntered = transform.root.GetComponent<RoomEntered>();
        switch (direction) {
            case Directions.Up: {
                    isGate = roomEntered.UpIsGate;
                    break;
                }
            case Directions.Down: {
                    isGate = roomEntered.DownIsGate;
                    break;
                }
            case Directions.Left: {
                    isGate = roomEntered.LeftIsGate;
                    break;
                }
            case Directions.Rigth: {
                    isGate = roomEntered.RigthIsGate;
                    break;
                }
            default:
                break;
        }
    }
    void Update() {
        if (!isGate || roomEntered.HasEnemy) return;
        foreach (var item in GateBlocks) {
            Destroy(item);
        }      
    }
    void OnTriggerEnter2D(Collider2D entered) {
        if (entered.gameObject.tag == "Player" && entered.GetComponent<NetworkIdentity>().isLocalPlayer) {
            entered.gameObject.GetComponent<PlayerController>().TransferPlayer(direction);
        }
        else {
            GateBlocks.Add(entered.gameObject);
        }
    }

}
