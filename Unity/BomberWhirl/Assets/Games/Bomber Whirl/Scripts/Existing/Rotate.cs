﻿namespace BomberWhirl {
	using UnityEngine;

	public class Rotate : MonoBehaviour {
		void Update() 
		{
			transform.Rotate (Vector3.back);
		}
	}
}