using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public Sprite[] sprites; 

	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnBall", 1, 1); 

	}

	// Update is called once per frame
	void Update () {

	}

	void SpawnBall(){
		GameObject go = Instantiate(Resources.Load("Prefabs/Circle")) as GameObject;

		//int num = GetComponent<GetColor> ().GetSpriteColor (); 

		//go.GetComponent<SpriteRenderer>().sprite = sprites[num]; 

	}
}
