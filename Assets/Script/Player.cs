using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class Player : NetworkBehaviour { 
    public ToggleEvent onToggleRemote;

    void Start() {
        if (!isLocalPlayer) {
            onToggleRemote.Invoke(true);
        }
    }
}