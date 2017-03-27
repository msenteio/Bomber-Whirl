namespace BomberWhirl {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	public class HitOne : MonoBehaviour {

		public AudioSource winner;
		public GameObject text; 
		private GameObject win;
		public int playerId; 

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		IEnumerator Wait () {
			yield return new WaitForSeconds(3);
			Destroy (win); 
			Scene loadedLevel = SceneManager.GetActiveScene ();
			SceneManager.LoadScene (loadedLevel.buildIndex);
			Debug.Log ("wait"); 
		} 

		void OnCollisionEnter2D(Collision2D other){
			Debug.Log("Yo");
			if (other.gameObject.tag == ("bomb1")) { 
				GameObject player2 = GameObject.FindGameObjectWithTag ("player2"); 
				win = (GameObject) Instantiate(text, player2.transform.position, Quaternion.identity);
				win.name = "ME";
				win.transform.parent = player2.transform;
				winner.Play ();
				StartCoroutine ("Wait"); 

			}
		}
	}
}