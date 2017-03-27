namespace ElevenEleven {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	[ExecuteInEditMode]
	public class CycleImageColor : CycleColor {
		public override Color color {
			get {
				return GetComponent<Image>().color;
			}
			set {
				GetComponent<Image>().color = value;
			}
		}
	}
}