namespace ElevenEleven.Game {
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System.Collections;

    internal class SelectOnEnable : MonoBehaviour {

        void OnEnable() {
            StartCoroutine(SelectOurselves());
        }

        IEnumerator SelectOurselves() {
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        void OnDisable() {
            if (EventSystem.current.currentSelectedGameObject == gameObject) {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}