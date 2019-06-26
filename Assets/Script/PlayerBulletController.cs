using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerBulletController : MonoBehaviour
{
    public float speed;
    public float damage;
    HurtingEnemy hurtEnemy;
    Rigidbody2D rigidBody;
    public float accuracy;
    
    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        hurtEnemy = GetComponent<HurtingEnemy>();
        hurtEnemy.damage = damage;

        transform.Rotate(new Vector3(0f, 0f, Random.Range(transform.rotation.z - accuracy, transform.rotation.z + accuracy)));

        rigidBody.AddForce(transform.right* -1 * Random.Range(speed-accuracy,speed+accuracy) );
    }
    void OnTriggerEnter2D(Collider2D entered) {
        if (entered.gameObject.layer == LayerMask.NameToLayer("BlockingLayer") || entered.gameObject.tag == "Enemy" || entered.gameObject.tag == "Exit") {
            NetworkServer.Destroy(gameObject);
        }
    }
}