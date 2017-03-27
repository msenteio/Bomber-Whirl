using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class OnTriggerExitEvent2D : OnTriggerEvent2D {

	void OnTriggerExit2D(Collider2D other) {
		base.ColliderTriggered.Invoke();
		base.DetailedColliderTriggered.Invoke(other);
	}
}
