using UnityEngine;
using System.Collections;

public class Port : MonoBehaviour {
	public GameObject target; 
	public float adjust; 
	public AudioSource port; 
	public GameObject teleAnimation;  

	private GameObject poof;

	void Start () {
 
	}

	void Update () {

	}

	IEnumerator Wait () {
		yield return new WaitForSeconds(4);
		Debug.Log ("wait"); 
	} 

	void teleport(GameObject bomb){
		Debug.Log ("teleport is called");
		Wait (); 
//		GameObject bomb = GameObject.FindGameObjectWithTag ("bomb"); 
		poof = (GameObject) Instantiate(teleAnimation, bomb.transform.position, Quaternion.identity);
		poof.name = "ME";
		poof.transform.parent = bomb.transform;
		Wait ();

		bomb.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + adjust, 0);  

	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("Yo");
		if (other.gameObject.tag == "bomb") { 
			port.Play ();
			teleport (other.gameObject);
		
			GameObject[] objs = GameObject.FindGameObjectsWithTag("portal");

			foreach (GameObject obj in objs){
				obj.transform.position = new Vector2(Random.Range(-8,11), Random.Range(1,5));
		
		} 
	}


}
}
