namespace ElevenEleven {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class CameraItems : MonoBehaviour {

		static HashSet<CameraItems> m_items = new HashSet<CameraItems>();
		public static HashSet<CameraItems> items {
			get { return m_items; }
		}

		[SerializeField] float m_radius;
		public float radius {
			get { return m_radius; }
		}

		public Vector2 maxBounds {
			get { return (Vector2)transform.position + new Vector2(radius, radius); }
		}

		public Vector2 minBounds {
			get { return (Vector2)transform.position - new Vector2(radius, radius); }
		}

		void OnEnable() {
			items.Add(this);
		}

		void OnDisable() {
			items.Remove(this);
		}

		void OnDrawGizmos() {
			Gizmos.DrawWireSphere(transform.position, radius);
		}
	}
}