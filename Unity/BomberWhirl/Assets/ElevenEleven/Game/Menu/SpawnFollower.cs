namespace ElevenEleven.Game {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using TMPro;

	internal class SpawnFollower : MonoBehaviour {

		[SerializeField] TextMeshProUGUI textMesh; 
		Vector2 size;
		Vector3 offset;

		GameObject toFollow;
		bool canFollow {
			get {
				return toFollow != null && toFollow.activeInHierarchy;
			}
		}

		RectTransform m_rectTransform;
		RectTransform rectTransform {
			get {
				if (m_rectTransform == null) {
					m_rectTransform = GetComponent<RectTransform>();
				}
				return m_rectTransform;
			}
		}

		CanvasGroup m_canvasRenderer;
		CanvasGroup canvasRenderer {
			get {
				if (m_canvasRenderer == null) {
					m_canvasRenderer = GetComponent<CanvasGroup>();
				}
				return m_canvasRenderer;
			}
		}

		public void Initialize(PlayerInput input, GameObject follow, Vector2 size, Vector3 offset) {
			textMesh.text = "Player " + (input.InputID + 1);
			this.toFollow = follow;
			this.size = size;
			this.offset = offset;

			textMesh.color = input.color;
			foreach (var image in GetComponentsInChildren<Image>()) {
				image.color = input.color;
			}

			StartCoroutine(FollowPlayerCoroutine());
		}

		void LateUpdate() {
			if (canFollow) {
				Vector2 anchor = Camera.main.WorldToViewportPoint(toFollow.transform.position + offset);
				rectTransform.anchorMin = anchor;
				rectTransform.anchorMax = anchor;
				rectTransform.anchoredPosition = Vector2.zero;
	//			rectTransform.anchoredPosition
	//			rectTransform.pivo = new Vector2(0.5f, 0.5f);
			}
		}

		IEnumerator FollowPlayerCoroutine() {

			float time = 0.35f;

			Rect screenSize = GetComponentInParent<Canvas>().pixelRect;
			float maxSize = 2 * Mathf.Max(screenSize.width, screenSize.height);

			float dt = 0.0f;
			while (dt < time && canFollow) {
				dt += Time.deltaTime;
				canvasRenderer.alpha = (ElevenTools.SinInterpolation(0.5f, 1.0f, dt / time));
				rectTransform.sizeDelta = ElevenTools.SinInterpolation(new Vector2(maxSize, maxSize), size, dt / time);
				yield return null;
			}

			float displayTime = 3.0f;
			dt = 0.0f;
			while (dt < displayTime && canFollow) {
				dt += Time.deltaTime;
				yield return null;
			}

			dt = 0.0f;
			float startAlpha = canvasRenderer.alpha;
			while (dt < time) {
				dt += Time.deltaTime;
				canvasRenderer.alpha = (ElevenTools.SinInterpolation(startAlpha, 0.0f, dt / time));
				yield return null;
			}

			Destroy(gameObject);
		}
	}
}