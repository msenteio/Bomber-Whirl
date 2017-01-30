using UnityEngine;
using System.Collections;
//sing InControl; 

public class playerMovement : MonoBehaviour 
{
//	public float speed          = 3.0f;
//	public float jumpSpeed          = 200.0f;
//	public bool grounded            = true;
//	public float time           = 4.0f;     
//	public bool allowfire;
//
//	private Rigidbody2D rigidbody; 
//	private InputDevice inputdevice;
//	public GameObject aim; 
//	public InputDevice device;
//
//
//	// Use this for initialization
	void Start () 
	{
//
//
//		rigidbody = GetComponent<Rigidbody2D>();
//
//
	}
//
//	// Update is called once per frame
	void FixedUpdate () 
	{
//
//		device = InputManager.ActiveDevice;
//		Vector3 x = device.LeftStick.X * transform.right * Time.deltaTime * speed;
//
//
//		if (speed > 0) {
//
		}
//
//		//if (time <= 2)
//		//{
//		if((device.Action1.WasPressed) && (allowfire = true))
//		{
//			Fire();
//
//		}
//
//		if(device.Action2.WasPressed)
//		{
//			Jump();
//
//		}
//
//		//}
//
//		transform.Translate(x);
//
//		//Restrict Rotation upon jumping of player object
//		transform.rotation = Quaternion.LookRotation(Vector3.forward);
//
//
//	}
//	void Jump()
//	{
//		if (grounded == true)
//		{
//			rigidbody.AddForce(Vector3.up* jumpSpeed);
//
//
//			grounded = false;
//		}
//
//	}
//	void OnCollisionEnter2D (Collision2D hit)
//	{
//		grounded = true;
//		// check message upon collition for functionality working of code.
//		Debug.Log ("I am colliding with something");
//	}
//
//	IEnumerator Wait () {
//		yield return new WaitForSeconds(10);
//		Debug.Log ("wait"); 
//	} 
//	//
//
//	void Fire(){
//		Debug.Log ("fire 1"); 
//		allowfire = false; 
//		aim = Instantiate(aim, transform.position + transform.right * 1.5f, transform.rotation);
//		aim.GetComponent<Rigidbody2D>().velocity = aim.transform.right * 9;
//		Wait (); 
//		allowfire = true;
		//Destroy(aim, 7.0f);
//	}

		//aim.transform.position.y = Input.GetAxis(Mousey); 
	


}