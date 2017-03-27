namespace ElevenEleven.Game {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LoadingText : MonoBehaviour {

        TextMeshProUGUI m_text;
        TextMeshProUGUI text {
            get {
                if (m_text == null) {
                    m_text = GetComponent<TextMeshProUGUI>();
                }
                return m_text;
            }
        }

        IEnumerator Start() {
            while (true) {
                for (int i = 0; i <= 3; i++) {
                    text.text = "Loading" + "...".Substring(0, i);
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }
}