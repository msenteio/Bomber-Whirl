namespace ElevenEleven.Game {
    using UnityEngine;
    using System.Collections;
    using ElevenEleven;
    using ElevenEleven.Game;
    using TMPro;

    [ExecuteInEditMode]
    internal class PlayerSelect : MonoBehaviour {

        [SerializeField] int playerID;
        [SerializeField] TextMeshProUGUI m_playerText;
        TextMeshProUGUI playerText {
            get { return m_playerText; }
        }

        PlayerInput m_currentPlayer = null;
        PlayerInput currentPlayer {
            get { return m_currentPlayer; }
            set {
                m_currentPlayer = value;
                CurrentPlayerUpdated();
            }
        }
		 
        Color color {
            get { return playerText.color; }
            set { playerText.color = value; }
        }

        void Start() {
            PlayerManager.inputAdded.AddListener(PlayerAdded);
            PlayerManager.inputRemoved.AddListener(PlayerRemoved);

			PlayerManager.TryGetHuman(playerID, out m_currentPlayer);

#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
#endif
            { 
                color = PlayerManager.Instance.GetColor(playerID);
            }

            CurrentPlayerUpdated();
        }

        void OnDestroy() {
            PlayerManager.inputAdded.AddListener(PlayerAdded);
            PlayerManager.inputRemoved.AddListener(PlayerRemoved);
        }

#if UNITY_EDITOR
        void Update() {
            if (!UnityEditor.EditorApplication.isPlaying) {
                name = "Player Select " + playerID;
                playerText.text = string.Format("Player {0}\nConnected", playerID + 1);
            }
        }
#endif

        void PlayerAdded(PlayerInput player) {
            if (player.InputID == playerID) {
                currentPlayer = player;
				color = player.color;
            }
        }

        void PlayerRemoved(PlayerInput player) {
            if (player == currentPlayer) {
                currentPlayer = null;
            }
        }

        void CurrentPlayerUpdated() {
            if (currentPlayer == null) {
                playerText.text = string.Format("Player {0}", playerID + 1);
            } else {
                playerText.text = "Connected";
            }
        }
    }
}
