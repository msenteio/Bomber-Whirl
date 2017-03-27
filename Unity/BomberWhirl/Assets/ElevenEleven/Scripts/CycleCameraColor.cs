namespace ElevenEleven {
	using UnityEngine;
	using System.Collections;

	public class CycleCameraColor : CycleColor {
		public override Color color {
			get {
				return GetComponent<Camera>().backgroundColor;
			}
			set {
				GetComponent<Camera>().backgroundColor = value;
			}
		}
	}
}