using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl; 

public class Bomb : MonoBehaviour {

	public GameObject bullet;
	public float speed = 5.0f;
	public Rigidbody2D rigidbody; 

	// Use this for initialization
	void Start () {
		
		rigidbody = GetComponent<Rigidbody2D>();
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector2 target = Camera.main.ScreenToWorldPoint( new Vector2(Input.mousePosition.x,  Input.mousePosition.y) );
			Vector2 myPos = new Vector2(transform.position.x,transform.position.y + 1);
			Vector2 direction = target - myPos;
			direction.Normalize();
			Quaternion rotation = Quaternion.Euler( 0, 0, Mathf.Atan2 ( direction.y, direction.x ) * Mathf.Rad2Deg );
			GameObject projectile = (GameObject) Instantiate( bullet, myPos, rotation);
			projectile.transform.position = direction * speed;
		}
	}
}

