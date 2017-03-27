namespace ElevenEleven.Game {
    using UnityEngine;
    using System.Collections;
    using ElevenEleven;
    using ElevenEleven.Game;
    using TMPro;

    [ExecuteInEditMode]
    internal class DisplayedPlayerScore : MonoBehaviour {

        [SerializeField] int playerID;
        [SerializeField]
        GameObject m_notJoinedParent;
        GameObject notJoinedParent {
            get { return m_notJoinedParent; }
        }

        [SerializeField]
        GameObject m_joinedParent;
        GameObject joinedParent {
            get { return m_joinedParent; }
		}

        [SerializeField]
        TextMeshProUGUI m_playerText;
        TextMeshProUGUI playerText {
            get { return m_playerText; }
        }

        [SerializeField]
        TextMeshProUGUI m_playerScoreText;
        TextMeshProUGUI playerScoreText {
            get { return m_playerScoreText; }
        }
		 
        Color color {
            get { return playerText.color; }
            set {
                Color color = value.NewAlpha(0.75f);
                playerText.color = color;
                playerScoreText.color = color;
            }
        }

        PlayerInput m_currentPlayer = null;
        PlayerInput currentPlayer {
            get { return m_currentPlayer; }
            set {
                m_currentPlayer = value;
                CurrentPlayerUpdated();
            }
        }

        void Start() {
			playerText.text = string.Format("Player {0}", playerID + 1);

            PlayerManager.inputAdded.AddListener(PlayerAdded);
            PlayerManager.inputRemoved.AddListener(PlayerRemoved);
			ShallowGames.scoreUpdated.AddListener(ScoreUpdated);

#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
#endif
            {
                color = PlayerManager.Instance.GetColor(playerID);

                if (PlayerManager.Contains(playerID)) {
					if (Lobby.Instance != null) {
						PlayerScore playerScore = ShallowGames.GetPlayerScore(playerID);
						ScoreUpdated(new PlayerScoreUpdate(playerID, playerScore.Score, playerScore.Score));
					}

					notJoinedParent.SetActive(false);
					joinedParent.SetActive(true);
				} else {
                    if (Lobby.Instance == null) {
                        notJoinedParent.SetActive(false);
                        joinedParent.SetActive(false);
                    } else {
                        notJoinedParent.SetActive(true);
                        joinedParent.SetActive(false);
                    }
                }
            }
        }

        void OnDestroy() {
            ShallowGames.scoreUpdated.RemoveListener(ScoreUpdated);
        }

		public void Waiting() {
			playerScoreText.text = "Waiting";
		}

		public void PlayerReady() {
			playerScoreText.text = "Ready";
		}

		public void GameStarted() {
			playerScoreText.text = "0";
		}

        void ScoreUpdated(PlayerScoreUpdate playerScoreUpdate) {
            if (playerScoreUpdate.playerID == playerID) {
				int score = playerScoreUpdate.currentScore;
				playerScoreText.text = string.Format("{0}", score);
            }
        }

#if UNITY_EDITOR
        void Update() {
            if (!UnityEditor.EditorApplication.isPlaying) {
				string text = string.Format("Player {0}", playerID + 1);
				name = text;
                playerText.text = text;
                playerScoreText.text = "0";
            }
        }
#endif

        void PlayerAdded(PlayerInput player) {
            if (player.InputID == playerID) {
                currentPlayer = player;
				color = player.color;
                
                notJoinedParent.SetActive(false);
				joinedParent.SetActive(true);
				PlaySound();
            }
        }

        void PlayerRemoved(PlayerInput player) {
            if (player == currentPlayer) {
                currentPlayer = null;
                notJoinedParent.SetActive(true);
                joinedParent.SetActive(false);
				PlaySound();
            }
        }

        void CurrentPlayerUpdated() {
            if (currentPlayer == null) {
                playerText.text = string.Format("Player {0}", playerID + 1);
            } else {
                playerText.text = "Connected";
            }
        }

		bool soundPlaying = false;

		void PlaySound() {
			if (!soundPlaying) {
				soundPlaying = true;
				StartCoroutine(PlaySoundCoroutine());
			}
		}

		IEnumerator PlaySoundCoroutine() {
			soundPlaying = true;

			ShallowMusic.Play("Speech/Player", BeatValue.Quarter);
			yield return new WaitForBeat(BeatValue.Quarter);

			ShallowMusic.Play("Speech/" + (playerID + 1), BeatValue.Eighth);
			yield return new WaitForBeat(BeatValue.Eighth);

			if (currentPlayer != null) {
				ShallowMusic.Play("Speech/Joined", BeatValue.Eighth);
			} else {
				ShallowMusic.Play("Speech/Left", BeatValue.Eighth);
			}

			soundPlaying = false;
		}
    }
}
