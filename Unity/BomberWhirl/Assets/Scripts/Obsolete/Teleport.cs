using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {
	public GameObject target; 
	public float adjust; 
	//private playerMovement pc; 
	private bool teleportNow; 
	public bool TELEPORTNOW
	{
		get	{
			return teleportNow;
		}
		set {
			Debug.Log ("in set");

			if (value != teleportNow) {
				Debug.Log ("value switch");

				if (!teleportNow) {
					Debug.Log ("teleport() called");

					teleport ();
				}
				teleportNow = value;

			} else {
				return;
			}
		}
	}
	//public Animator teleport; 
	public AudioSource port; 
	public GameObject teleAnimation; 
	public GameObject player; 
	private float timer; 
	private float teleportTimer; 

	//public PlayerController script; 

	private GameObject poof;

	void Start () {
		TELEPORTNOW = false;
		teleportNow = false; 

		//teleport = GetComponent<Animator> (); 
		//pc = player.GetComponent<playerMovement>(); 
	}

	void Update () {
		timer += Time.deltaTime; 
		teleportTimer += Time.deltaTime; 
		Debug.Log (teleportTimer);


		if (timer >= 1) {
			Destroy (poof); 
		}
		if (teleportTimer >= 1f) {

			TELEPORTNOW = true;;
		}
	}

	IEnumerator Wait () {
		yield return new WaitForSeconds(2);
		Debug.Log ("wait"); 
	} 

	void teleportInitiate(){
		teleportTimer = 0; 
		TELEPORTNOW = false;

		timer = 0;
		poof = (GameObject) Instantiate(teleAnimation, player.transform.position, Quaternion.identity);
		poof.name = "ME";
		poof.transform.parent = player.transform;
		//pc.enabled = false; 

		Debug.Log ("teleportImer =0");

	}

	void teleport(){
		Debug.Log ("teleport is called");

		player.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + adjust, 0);
		//pc.enabled = true; 



	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("Yo");
		if (other.tag == "Player") { 
			port.Play ();  
			teleportInitiate ();
			//Destroy (teleAnimation); 
		} 
	}
}
