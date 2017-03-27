namespace ElevenEleven.Game {
	using UnityEngine;
	using System.Collections;

	public class ContainCameraItems : MonoBehaviour {

		[SerializeField] float minSize = 0;
		//		[SerializeField] float moveSpeed = 0.5f;
		//		[SerializeField] float zoomSpeed = 0.5f;
		[SerializeField] float lerpDesiredPositionFactor = 10;
		[SerializeField] float lerpVelocityFactor = 10;

		Vector2 desiredPosition;
		float desiredSize;
		Vector2 velocity;
		float cameraDeltaSize;

		Camera m_camera;
		new Camera camera {
			get {
				return m_camera;
			}
		}

		void Awake() {
			m_camera = GetComponent<Camera>();
		}

		void OnEnable() {
			desiredPosition = (Vector2)transform.position;
			desiredSize = camera.orthographicSize;
		}

		void LateUpdate() {
			Vector2 minBounds = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 maxBounds = new Vector2(float.MinValue, float.MinValue);

			if (CameraItems.items.Count > 0) {
				foreach (var item in CameraItems.items) {
					for (int i = 0; i < 2; i++) {
						if (item.minBounds[i] < minBounds[i]) {
							minBounds[i] = item.minBounds[i];
						}
						if (item.maxBounds[i] > maxBounds[i]) {
							maxBounds[i] = item.maxBounds[i];
						}
					}
				}
			}

			Vector2 bounds = maxBounds - minBounds;
			Vector2 calcPosition = 0.5f * (minBounds + maxBounds);
			float calcSize = Mathf.Max(minSize, 0.5f * Mathf.Max(bounds.x / camera.aspect, bounds.y));

			//			transform.position = Vector2.MoveTowards(transform.position, desiredPosition, moveSpeed * Time.deltaTime).ToVector3(transform.position.z);
			//			camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, desiredSize, zoomSpeed * Time.deltaTime);

			desiredPosition = Vector2.Lerp(desiredPosition, calcPosition, lerpDesiredPositionFactor * Time.deltaTime);
			desiredSize = Mathf.Lerp(desiredSize, calcSize, lerpDesiredPositionFactor * Time.deltaTime);

			//			velocity = Vector2.Lerp(velocity, desiredPosition - (Vector2)transform.position, lerpVelocityFactor * Time.deltaTime);
			//			cameraDeltaSize = Mathf.Lerp(cameraDeltaSize, desiredSize - camera.orthographicSize, lerpVelocityFactor * Time.deltaTime);
			//
			//			camera.transform.position += Time.deltaTime * velocity.ToVector3();
			//			camera.orthographicSize += Time.deltaTime * cameraDeltaSize;
			camera.transform.position = desiredPosition.ToVector3(camera.transform.position.z);
			camera.orthographicSize = desiredSize;
			camera.orthographicSize = Mathf.Max(minSize, camera.orthographicSize);
		}
	}
}