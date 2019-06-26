using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class Menu : MonoBehaviour {

    public static bool menuIsActive = false;

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menuIsActive) {
                Resume();
            }
            else
                Pause();
        }
	}
    public void Resume() {
        menuIsActive = false;
        Cursor.visible = false;
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    void Pause() {
        Cursor.visible = true;
        menuIsActive = true;
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private NetworkManager networkManager;

    // Use this for initialization
    void Start() {
        networkManager = NetworkManager.singleton;
    }

    public void LeaveRoom() {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
    public void QuitGame() {
        LeaveRoom();
        Application.Quit();
    }
}
