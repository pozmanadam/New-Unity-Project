using UnityEngine;
using UnityEngine.Networking;

public class WeaponStats : MonoBehaviour {
    public enum ShootTypes { Single, Spread };

    public ShootTypes shootType;
    public float fireRate;
    public float damage;
    public float cooldownCounter;
    public float speed;
    public int ammunition;
    public float accuracy;

    public Transform spawnPoint;

	// Use this for initialization
	void Start () {
        if(transform.childCount > 0)
        spawnPoint = transform.GetChild(0);
	}
}
