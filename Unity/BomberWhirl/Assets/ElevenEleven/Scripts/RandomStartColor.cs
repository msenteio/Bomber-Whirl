namespace ElevenEleven {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public abstract class RandomStartColor : MonoBehaviour {

		[SerializeField] bool m_randomHue = true;
		[Range(0, 1)] [SerializeField] float m_minHue = 0;
		[Range(0, 1)] [SerializeField] float m_maxHue = 1;

		[SerializeField] bool m_randomSaturation = false;
		[Range(0, 1)] [SerializeField] float m_minSaturation = 0;
		[Range(0, 1)] [SerializeField] float m_maxSaturation = 1;

		[SerializeField] bool m_randomBrightness = false;
		[Range(0, 1)] [SerializeField] float m_minBrightness = 0;
		[Range(0, 1)] [SerializeField] float m_maxBrightness = 1;

		[SerializeField] bool m_randomAlpha = false;
		[Range(0, 1)] [SerializeField] float m_minAlpha = 0;
		[Range(0, 1)] [SerializeField] float m_maxAlpha = 1;

		public abstract Color color {
			get; set;
		}

		void Start() {
			HSBColor hsbColor = new HSBColor(color);
			if (m_randomHue) {
				hsbColor.hue = Random.Range(m_minHue, m_maxHue);
			}
			if (m_randomSaturation) {
				hsbColor.saturation = Random.Range(m_minSaturation, m_maxSaturation);
			}
			if (m_randomBrightness) {
				hsbColor.brightness = Random.Range(m_minBrightness, m_maxBrightness);
			}
			if (m_randomAlpha) {
				hsbColor.alpha = Random.Range(m_minAlpha, m_maxAlpha);
			}
			color = hsbColor.ToColor();
		}
	}
}