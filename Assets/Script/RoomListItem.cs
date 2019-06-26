using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class RoomListItem : MonoBehaviour {

    public delegate void JoinRoomDelegate(MatchInfoSnapshot match);
    private JoinRoomDelegate OnJoinCallback;
    public Text roomNameText;
    private MatchInfoSnapshot match;

    public void SetMatch(MatchInfoSnapshot match, JoinRoomDelegate OnJoinCallback) {
        this.match = match;
        this.OnJoinCallback = OnJoinCallback;
        if(match.currentSize < match.maxSize) {
            roomNameText.text = match.name + "(Open)";
        }
        else roomNameText.text = match.name + "(Full)";
    }

    public void JoinServer() {
        OnJoinCallback.Invoke(match);
    }

}