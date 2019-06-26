using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerArmRotation : NetworkBehaviour{

    Transform Hand;

    void Start() {
        Hand = transform.Find("Hand");
    }

    void Update() {
        if (Camera.main != null && isLocalPlayer) {
            Vector2 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize ();

            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Hand.rotation = Quaternion.Euler(0f, 0f, rotZ);

            if (rotZ > 90 || rotZ < -90) {
                Hand.rotation = Quaternion.Euler(180, 0f, -rotZ);
                Hand.localPosition = new Vector2(-0.3f, Hand.localPosition.y);

            }
            else {
                Hand.rotation = Quaternion.Euler(0f, 0f, rotZ);
                Hand.localPosition = new Vector2(0.3f, Hand.localPosition.y);
            }
        }
    }

}
