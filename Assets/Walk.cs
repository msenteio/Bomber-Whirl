using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl; 

public class Walk : MonoBehaviour {
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
			} 
		}

	}

	float inputAng;

	private Rigidbody2D myRB;

	// Use this for initialization
	void Start () {
		myRB = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void FixedUpdate () {

		device = InputManager.ActiveDevice;
		SetXPressed ();
		if (xPressed == false) {
			Walking ();
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

	void Walking()
	{
		if (Mathf.Abs (device.LeftStick.X) > .25f || Mathf.Abs (device.LeftStick.Y) > .25f) {
			inputAng = Mathf.Atan2 (device.LeftStick.Y, device.LeftStick.X) * Mathf.Rad2Deg;
			if (inputAng < 45 && inputAng > -45) {
				myRB.MovePosition (transform.position + Vector3.right * .1f);
			} else if (inputAng > 135 || inputAng < -135) {
				myRB.MovePosition (transform.position - Vector3.right * .1f);
			}
		} else {
			myRB.MovePosition (transform.position);
		}
	}
}
