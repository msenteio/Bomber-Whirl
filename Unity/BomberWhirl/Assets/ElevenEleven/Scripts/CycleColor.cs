namespace ElevenEleven {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public abstract class CycleColor : MonoBehaviour {

		public abstract Color color {
			get;
			set;
		}

		[SerializeField] float m_cycleSpeed = 0.1f;
		[SerializeField] bool m_useGameTime = false;

		float deltaTime {
			get { return m_useGameTime ? Time.deltaTime : Time.unscaledDeltaTime; }
		}

		void Update() {
			HSBColor hsbColor = new HSBColor(color);
			hsbColor.hue += deltaTime * m_cycleSpeed;
			hsbColor.hue %= 1.0f;
			color = hsbColor.ToColor();
		}
	}
}