using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class OnTriggerEnterEvent2D : OnTriggerEvent2D {

	void OnTriggerEnter2D(Collider2D other) {
		base.ColliderTriggered.Invoke();
		base.DetailedColliderTriggered.Invoke(other);
	}
}
