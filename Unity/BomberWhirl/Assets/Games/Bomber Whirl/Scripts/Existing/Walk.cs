namespace BomberWhirl {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	//using InControl; 

	public class Walk : MonoBehaviour {
		//public InputDevice device;
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

		public float jumpSpeed          = 200.0f;
		float inputAng;
		public bool grounded            = true;
		private Animator bombergirl;
		private Rigidbody2D myRB;
		float horizontalAxis;
		float verticalAxis;
		public int playerId;


		// Use this for initialization
		void Start () {
			myRB = GetComponent<Rigidbody2D> ();
			bombergirl = GetComponent<Animator> (); 

		}

		// Update is called once per frame
		void FixedUpdate () {
			float horizontalAxis = Input.GetAxis("Horizontal" + playerId); 
			float verticalAxis = Input.GetAxis("Vertical" + playerId); 
			//device = InputManager.ActiveDevice;
			SetXPressed ();
			if (xPressed == false) {
				Walking ();
			}

			//		if(device.Action2.WasPressed)
			//		{
			//			Jump();
			//
			//		}
		}

		void SetXPressed ()
		{
			if (Input.GetButtonUp("X" + playerId)) {
				xPressed = true;
			}
			else {
				xPressed = false;
			}
		}

		void Jump()
		{
			if (grounded == true) {
				myRB.AddForce(Vector3.up* jumpSpeed);



				grounded = false;
			}
		}

		void Walking()
		{
			float horizontalAxis = Input.GetAxis("Horizontal" + playerId); 
			float verticalAxis = Input.GetAxis("Vertical" + playerId); 	
			if (Mathf.Abs (horizontalAxis) > .25f || Mathf.Abs (verticalAxis) > .25f) {
				inputAng = Mathf.Atan2 (verticalAxis, horizontalAxis) * Mathf.Rad2Deg;
				if (inputAng < 45 && inputAng > -45) {
					myRB.MovePosition (transform.position + Vector3.right * .1f);
					bombergirl.SetLayerWeight (0, 1); 
					bombergirl.SetLayerWeight (0, 0);
					bombergirl.SetLayerWeight (2, 0);
				} else if (inputAng > 135 || inputAng < -135) {
					myRB.MovePosition (transform.position - Vector3.right * .1f);
					bombergirl.SetLayerWeight (0, 1); 
					bombergirl.SetLayerWeight (0, 0);
					bombergirl.SetLayerWeight (2, 0);
				}
			} else {
				myRB.MovePosition (transform.position);
				bombergirl.SetLayerWeight (0, 0); 
				bombergirl.SetLayerWeight (1, 0);
				bombergirl.SetLayerWeight (2, 1);


			}
		}
	}
}