using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyBulletController : MonoBehaviour
{

    public float speed;
    public int damage;
    HurtingPlayer hurtPlayer;
    Rigidbody2D rigidBody;
    public float accuracy;


    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        hurtPlayer = GetComponent<HurtingPlayer>();
        hurtPlayer.damage = damage;

        transform.Rotate(new Vector3(0f, 0f, Random.Range(transform.rotation.z - accuracy, transform.rotation.z + accuracy)));

        rigidBody.AddForce(transform.right * -1 * Random.Range(speed - accuracy, speed + accuracy));
    }

    void OnTriggerEnter2D(Collider2D entered) {
        if (entered.gameObject.layer == LayerMask.NameToLayer("BlockingLayer") || entered.gameObject.tag == "Player") {
            NetworkServer.Destroy(gameObject);
        }
    }
}
