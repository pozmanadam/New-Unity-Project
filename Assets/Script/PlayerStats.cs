using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : NetworkBehaviour {

    public float moveSpeed;
    [SyncVar]
    public int playerCurrentHealth;
    [SyncVar]
    public int playerFullHealth;
    public int playerMaxHealth;

    // Use this for initialization
    void Start() {
        if (playerMaxHealth > 20) { playerMaxHealth = 20; }
        if (isLocalPlayer)
            GameObject.Find("HealthBar").GetComponent<UIHeartManager>().playerHealth = this;
    }

}
