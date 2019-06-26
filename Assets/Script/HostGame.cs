using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour { 
    private uint roomSize = 2;
    private string serverName = "Default";
    private NetworkManager networkManager;

    void Start() {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }
    }

    public void SetServerName(string serverName) {
        this.serverName = serverName;
    }

    public void CreateRoom() {
        if (serverName != "" && serverName != null) {
            networkManager.matchMaker.CreateMatch(serverName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }
}