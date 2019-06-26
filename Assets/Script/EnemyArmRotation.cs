using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyArmRotation : NetworkBehaviour {

    public GameObject target;

    void Update() {
        if (!transform.root.GetComponent<NetworkIdentity>().isServer) return;
        if (target != null) {
            Vector2 difference = target.transform.position - transform.position;
            difference.Normalize();

            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            if (rotZ > 90 || rotZ < -90) {
                transform.rotation = Quaternion.Euler(180, 0f, -rotZ);
                transform.localPosition = new Vector2(-0.3f, transform.localPosition.y);

            }
            else {
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
                transform.localPosition = new Vector2(0.3f, transform.localPosition.y);
            }
        }
    }
}
