namespace ElevenEleven.Game {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class CenterScrollRectItem : MonoBehaviour {


		RectTransform m_rectTransform;
		RectTransform rectTransform {
			get { 
				if (m_rectTransform == null) {
					m_rectTransform = GetComponent<RectTransform>();
				}
				return m_rectTransform;
			}
		}

		protected virtual void Update() {
			RectTransform parentRect = transform.parent.GetComponent<RectTransform>(); 
			ScrollRect scrollRect = gameObject.GetComponentInParent<ScrollRect>();
			Vector3 position = new Vector2(0, -transform.localPosition.y - 0.5f * scrollRect.GetComponent<RectTransform>().rect.height);
			if (float.IsInfinity(position.y)) {
				position.y = 0;
			}

			parentRect.localPosition = Vector3.Lerp(parentRect.localPosition, position, 5 * Time.deltaTime);
//			scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, position, 10 * Time.deltaTime);
		}
	}
}