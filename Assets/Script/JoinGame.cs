using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

public class JoinGame : MonoBehaviour
{

    List<GameObject> serverList = new List<GameObject>();
    public Text listStatus;
    public GameObject serverListItemPrefab;
    public Transform serverListContanier;
    private NetworkManager networkManager;

    void Start() {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }

        RefreshServerList();
    }

    void ClearServerList() {
        for (int i = 0; i < serverList.Count; i++) {
            Destroy(serverList[i]);
        }

        serverList.Clear();
    }

    public void RefreshServerList() {
        ClearServerList();

        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }

        networkManager.matchMaker.ListMatches(0, 5, "", true, 0, 0, OnMatchList);
        listStatus.text = "Loading Servers";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList) {
        listStatus.text = "";

        if (!success || matchList == null || matchList.Count == 0) {
            listStatus.text = "No servers";
            return;
        }

        foreach (MatchInfoSnapshot item in matchList) {
            GameObject serverListItemIns = Instantiate(serverListItemPrefab);
            serverListItemIns.transform.SetParent(serverListContanier);

            RoomListItem serverListItem = serverListItemIns.GetComponent<RoomListItem>();
            if (serverListItem != null) {
                serverListItem.SetMatch(item, OnJoinRoom);
            }

            serverList.Add(serverListItemIns);
        }
    }

    public void OnJoinRoom(MatchInfoSnapshot matchInfoSnapshot) {
        MatchInfo matchInfo = networkManager.matchInfo;
        if (matchInfo != null) {
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }
        networkManager.matchMaker.JoinMatch(matchInfoSnapshot.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        StopCoroutine("WaitForJoin");
        StartCoroutine("WaitForJoin");
    }

    IEnumerator WaitForJoin() {
        ClearServerList();

        int countdown = 10;
        while (countdown > 0) {
            listStatus.text = "joining " + countdown;

            yield return new WaitForSeconds(1);
            countdown--;
        }

        listStatus.text = "Failed to connect";
        yield return new WaitForSeconds(1);

        MatchInfo matchInfo = networkManager.matchInfo;
        if (matchInfo != null) {
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }

        RefreshServerList();

    }

}
