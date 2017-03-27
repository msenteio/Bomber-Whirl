namespace ElevenEleven {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using ElevenEleven;

	public class CycleParticleColor : MonoBehaviour {

		HSBColor color = new HSBColor(0.0f, 128.0f / 255.0f, 225.0f / 255.0f);

		void Awake() {
			color = new HSBColor(GetComponent<ParticleSystem>().main.startColor.color);
		}

		void Update() {
			color.hue += 0.1f * Time.deltaTime;
			color.hue %= 1.0f;

			ParticleSystem particle = GetComponent<ParticleSystem>();
			var main = particle.main;
			var startColor = main.startColor;
			startColor.color = color.ToColor();
			main.startColor = startColor;
//			particle.main = main;
		}
	}
}
