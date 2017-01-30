using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	private AudioSource wall; 
	private GameObject poof; 
	public GameObject teleAnimation;  
	// Use this for initialization
	void Start () {

		wall = GetComponent<AudioSource> (); 
		//teleAnimation = GetComponent<Animator> (); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator Wait () {
		print("Coroutine started");
		//yield return new WaitForSeconds(1);
		poof = (GameObject) Instantiate(teleAnimation, this.transform.position, Quaternion.identity);
		poof.name = "ME";
		poof.transform.parent = this.transform;
		Debug.Log ("wait"); 
		yield return new WaitForSeconds(1);
		GameObject deadbomb = this.gameObject;
		Destroy(GameObject. Find("aim(Clone)"));
	} 

	void OnCollisionEnter2D(Collision2D other){
		Debug.Log("Yo");
		if (other.gameObject.tag == "wall") { 
			wall.Play(); 
//			poof = (GameObject) Instantiate(teleAnimation, this.transform.position, Quaternion.identity);
//			poof.name = "ME";
//			poof.transform.parent = this.transform; 
			print("Coroutine about to start");
			StartCoroutine("Wait");
			//StopCoroutine("Wait");
			 

		}
	}
}
