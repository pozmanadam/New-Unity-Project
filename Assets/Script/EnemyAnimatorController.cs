using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyAnimatorController : NetworkBehaviour {

    private Animator animator;
    private Vector2 lastPosition;

    // Use this for initialization
    void Start () {
        if (!isServer) return;
        animator = GetComponent<Animator>();
        lastPosition = new Vector2(0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (!isServer) return;
        if (lastPosition.x != transform.position.x) {
            animator.SetBool("isMoving", true);

            if (lastPosition.x > transform.position.x)
                animator.SetFloat("xInput", -1);
            else
                animator.SetFloat("xInput", 1);       
            lastPosition = new Vector2(transform.position.x, lastPosition.y);
        }
        if (lastPosition.y != transform.position.y) {
            animator.SetBool("isMoving", true);

            if (lastPosition.y > transform.position.y) {
                animator.SetFloat("yInput", -1);
            }
            else
                animator.SetFloat("yInput", 1);

            lastPosition = new Vector2( lastPosition.x,transform.position.y);
        }
        animator.SetFloat("yLastInput", lastPosition.y);
        animator.SetFloat("xLastInput", lastPosition.x);

        
    }
}
