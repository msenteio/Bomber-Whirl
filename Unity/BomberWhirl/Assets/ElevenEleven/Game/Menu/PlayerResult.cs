namespace ElevenEleven.Game {
    using UnityEngine;
    using System.Collections;
    using ElevenEleven;
    using ElevenEleven.Game;
    using TMPro;

    [ExecuteInEditMode]
    internal class PlayerResult : MonoBehaviour {

        [SerializeField] int playerID;
        [SerializeField] TextMeshProUGUI m_playerText;
        TextMeshProUGUI playerText {
            get { return m_playerText; }
        }
	
        Color color {
            get { return playerText.color; }
            set { playerText.color = value; }
        }

		IEnumerator Start() {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
#endif
            {
                if (ShallowGames.ContainsScore(playerID)) {
                    color = PlayerManager.Instance.GetColor(playerID);
                    SetScore(ShallowGames.GetPlayerScore(playerID).PrevScore);
                    yield return new WaitForSeconds(1.0f);
					SetScore(ShallowGames.GetPlayerScore(playerID).RoundScore);
                } else {
                    gameObject.SetActive(false);
                }
            }
		}

#if UNITY_EDITOR
        void Update() {
            if (!UnityEditor.EditorApplication.isPlaying) {
                name = "Player Result " + playerID;
				SetScore(0);
            }
        }
#endif

		void SetScore(int score) {
			playerText.text = string.Format("<size=32>Player {0}</size>\nScore {1}", playerID + 1, score);
		}
    }
}
