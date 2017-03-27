using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class TriggerEvent2D : UnityEvent<Collider2D> { }

public abstract class OnTriggerEvent2D : MonoBehaviour {

	[SerializeField] UnityEvent colliderTriggered;
	public UnityEvent ColliderTriggered {
		get { return colliderTriggered; }
		private set { colliderTriggered = value; }
	}

	[SerializeField] TriggerEvent2D detailedColliderTriggered;
	public TriggerEvent2D DetailedColliderTriggered {
		get { return detailedColliderTriggered; }
		private set { detailedColliderTriggered = value; }
	}

	// We want the checkbox to enable/disable available
	protected virtual void Start() { }
}
