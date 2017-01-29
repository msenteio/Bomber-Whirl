namespace UnityEngine.SceneManagement

{

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other){
		Debug.Log("Yo");
		if (other.gameObject.tag == "bomb") { 
			Scene loadedLevel = SceneManager.GetActiveScene ();
			SceneManager.LoadScene (loadedLevel.buildIndex);
		}
}
}
}