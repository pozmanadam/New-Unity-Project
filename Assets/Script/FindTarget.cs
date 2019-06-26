using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FindTarget : NetworkBehaviour {
    GameObject[] Players;
    GameObject targetPath;
    GameObject targetArm;
    GameObject targetPathNew;
    GameObject targetArmNew;
    EnemyController pathTarget;
    EnemyWeaponShooter isFireing;
    EnemyArmRotation rotationTarget;
    // Use this for initialization
    void Start () {
        if (!isServer) return;
        Players = GameObject.FindGameObjectsWithTag("Player");
        pathTarget = GetComponent<EnemyController>();
        isFireing = GetComponent<EnemyWeaponShooter>();
        if (transform.Find("Hand") != null)
        rotationTarget = transform.Find("Hand").GetComponent<EnemyArmRotation>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isServer) return;

        foreach (var item in Players) {
            if (item != null) {
                //pathtarget
                if (targetPathNew == null) targetPathNew = item;
                else {
                    if(item != targetPathNew)
                    if ((item.transform.position - transform.position).magnitude < (targetPathNew.transform.position - transform.position).magnitude) {
                        targetPathNew = item;
                    }
                }
                //armtarget
                if (rotationTarget != null) {
                    targetArmNew = null;
                    var hit = Physics2D.Raycast(transform.position, item.transform.position - transform.position, (item.transform.position - transform.position).magnitude, 1 << LayerMask.NameToLayer("BlockingLayer"));
                    if (hit.transform == null) {
                        if (targetArmNew == null) targetArmNew = item;
                        else {
                            if (item != targetArmNew)
                                if ((item.transform.position - transform.position).magnitude < (targetArmNew.transform.position - transform.position).magnitude) {
                                targetArmNew = item;
                            }
                        }

                    }
                }
            }

        }
        if(targetArmNew != targetArm) {
            targetArm = targetArmNew;
            if(isFireing != null) {
                isFireing.isFiring = targetArm == null ? false : true;
            }
            if (rotationTarget != null)
                rotationTarget.target = targetArm;
        }
        
        if(targetPathNew != targetPath) {
            targetPath = targetPathNew;
            if(pathTarget != null)
            pathTarget.target = targetPath.transform;
        }

    }
}
