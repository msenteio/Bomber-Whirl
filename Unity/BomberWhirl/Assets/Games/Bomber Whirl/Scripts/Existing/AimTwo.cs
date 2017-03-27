namespace BomberWhirl {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	//using InControl; 

	public class AimTwo : MonoBehaviour {
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
					if (!value) 
					{
						Fire ();
					}
				} 
			}

		}

		float inputAng;
		public int playerId; 

		float horizontalAxis;
		float verticalAxis;
		public bool allowfire;
		public float shotForce = 200f;
		private Animator bombergirl; 
		public GameObject bomb;
		public GameObject playerTwo; 

		// Use this for initialization
		void Start () {

			bombergirl = GetComponent<Animator> (); 

		}

		// Update is called once per frame
		void FixedUpdate () {
			float horizontalAxis = Input.GetAxis("Horizontal" + playerId); 
			float verticalAxis = Input.GetAxis("Vertical" + playerId); 

			//device = InputManager.ActiveDevice;
			SetXPressed ();
			if (xPressed == true) {
				Aiming ();
			}
		}

		void SetXPressed ()
		{
			float horizontalAxis = Input.GetAxis("Horizontal" + playerId); 
			float verticalAxis = Input.GetAxis("Vertical" + playerId); 

			if (Input.GetButtonUp("X1")) {
				xPressed = true;
			}
			else {
				xPressed = false;
			}
		}

		void Aiming()
		{
			inputAng = Mathf.Atan2 (verticalAxis, horizontalAxis) * Mathf.Rad2Deg;
			Debug.Log (inputAng);
		}


		void Fire()
		{
			float horizontalAxis = Input.GetAxis("Horizontal" + playerId); 
			float verticalAxis = Input.GetAxis("Vertical" + playerId); 
			Debug.Log ("fire 1"); 
			allowfire = false; 
			GameObject newProjectile = Instantiate(bomb, playerTwo.transform.position - playerTwo.transform.right * 1.2f, Quaternion.identity) as GameObject; 
			bombergirl.SetLayerWeight (0, 0); 
			bombergirl.SetLayerWeight (1, 1);
			bombergirl.SetLayerWeight (2, 0);
			Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
			Vector3 direction = new Vector3(horizontalAxis, verticalAxis);
			rb.AddForce(direction * shotForce, ForceMode2D.Impulse);
		}
	}
}