using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl; 

public class Aim : MonoBehaviour {
	public InputDevice device;
	bool _xPressed; 
	bool xPressed {
		get 
		{
			return _xPressed; 
		} 
		
		set 
		{
			if (value != _xPressed)
			{
				_xPressed = value;
				if (!value) 
				{
					Fire ();
				}
			} 
		}
		
	}

	float inputAng;
	public GameObject aim; 
	public bool allowfire;
	public float shotForce = 7;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		device = InputManager.ActiveDevice;
		SetXPressed ();
		if (xPressed == true) {
			Aiming ();
		}
	}

	void SetXPressed ()
	{
		if (device.Action1.IsPressed) {
			xPressed = true;
		}
		else {
			xPressed = false;
		}
	}

	void Aiming()
	{
		inputAng = Mathf.Atan2 (device.LeftStick.Y, device.LeftStick.X) * Mathf.Rad2Deg;
		Debug.Log (inputAng);
	}


	void Fire()
	{
		Debug.Log ("fire 1"); 
		allowfire = false; 
		GameObject newProjectile = Instantiate(aim, transform.position + transform.right * 1.5f, Quaternion.identity) as GameObject;
		Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
		Vector3 direction = new Vector3(device.LeftStick.X, device.LeftStick.Y);
		rb.AddForce(direction * shotForce, ForceMode2D.Impulse);
	}
}
