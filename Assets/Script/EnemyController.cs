using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyController : NetworkBehaviour { 
    public Transform target;
    public int speed = 1;


    public Grid grid;
    Vector3[] path;

    Rigidbody2D rigidBody;

    void Start() {
        if (!isServer) return;
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath() {

        if (target == null) yield return new WaitForSeconds(0.2f);
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound,grid);

        float moveThreshold = 0.5f;
        Vector3 targetPositionOld = target.position;

        while (true) {
            yield return new WaitForSeconds(0.5f);
            if ((target.position - targetPositionOld).sqrMagnitude > moveThreshold) {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound,grid);
                targetPositionOld = target.position;
            }
        }
    }

    public void OnPathFound(Vector3[] points, bool succes) {
        if (succes) {
            path = points;

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {

        int index = 0;
        if(path.Length > 1)
        while (Physics2D.Raycast(transform.position, path[index+1] - transform.position, Vector2.Distance(path[index+1], transform.position), 1 << LayerMask.NameToLayer("BlockingLayer")).transform == null) {
            index++;
            if (index == path.Length - 1) {
                break;
            }

        }

        rigidBody.velocity = (path[index]- transform.position).normalized * speed;


        while (true) {
            var distance = (path[index] -transform.position);
            if (Mathf.Abs(distance.x) < 0.1f && Mathf.Abs(distance.y) < 0.1f) {
                index++;
                if (index == path.Length) {
                    rigidBody.velocity = Vector3.zero;
                    break;
                }

                while (Physics2D.Raycast(transform.position, path[index+1] - transform.position, Vector2.Distance(path[index+1], transform.position), 1 << LayerMask.NameToLayer("BlockingLayer")).transform == null) {

                    index++;
                    if (index == path.Length - 1) {
                        break;
                    }
                }


                rigidBody.velocity = (path[index] - transform.position).normalized * speed;
            }

            yield return null;
        }

    }


}