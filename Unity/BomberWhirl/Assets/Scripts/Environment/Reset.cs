
namespace UnityEngine.SceneManagement
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			Restart ();
		
	}


void Restart() {
	if (Input.GetKeyDown(KeyCode.R)) {
				Scene loadedLevel = SceneManager.GetActiveScene ();
				SceneManager.LoadScene (loadedLevel.buildIndex);
	}
}
}
}