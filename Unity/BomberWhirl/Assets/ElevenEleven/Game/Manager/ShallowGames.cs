namespace ElevenEleven {
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections;
    using System.Collections.Generic;
	using ElevenEleven.Game;

    [System.Serializable]
    public class PlayerScore {

        [SerializeField]
        int m_playerID;
        public int playerID {
            get { return m_playerID; }
            private set { m_playerID = value; }
        }

        [SerializeField]
        int m_prevScore;
        public int PrevScore {
            get { return m_prevScore; }
            internal set { m_prevScore = value; }
        }

        [SerializeField]
        int m_score;
        public int Score {
            get { return m_score; }
            internal set { m_score = value; }
        }
        
        [SerializeField]
        int m_roundScore;
        public int RoundScore {
            get { return m_roundScore; }
            internal set { m_roundScore = value; }
        }

        public PlayerScore(int playerID) {
            this.playerID = playerID;
        }
    }

    [System.Serializable]
    public struct PlayerScoreUpdate {
        [SerializeField]
        int m_playerID;
        public int playerID {
            get { return m_playerID; }
            private set { m_playerID = value; }
        }

        [SerializeField]
        int m_currentScore;
        public int currentScore {
            get { return m_currentScore; }
            private set { m_currentScore = value; }
        }

        [SerializeField]
        int m_prevScore;
        public int prevScore {
            get { return m_prevScore; }
            private set { m_prevScore = value; }
        }

        public PlayerScoreUpdate(int playerID, int currentScore, int prevScore) {
            m_playerID = playerID;
            m_currentScore = currentScore;
            m_prevScore = prevScore;
        }
    }

    [System.Serializable]
    internal class ScoreUpdated : UnityEvent<PlayerScoreUpdate> { }

    public class ShallowGames {

        static Dictionary<int, PlayerScore> m_scores = new Dictionary<int, PlayerScore>();
        internal static Dictionary<int, PlayerScore> scores {
            get { return m_scores; }
            set { m_scores = value; }
        }

        static ScoreUpdated m_scoreUpdated = new ScoreUpdated();
        internal static ScoreUpdated scoreUpdated {
            get { return m_scoreUpdated; }
        }

        static bool m_gameOver = false;
        static bool gameOver {
            get { return m_gameOver; }
            set { m_gameOver = value; }
        }

        public static bool ContainsScore(int playerID) {
            return scores.ContainsKey(playerID);
        }

        public static int GetScore(int playerID) {
            return GetPlayerScore(playerID).RoundScore;
        }

        static internal PlayerScore GetPlayerScore(int playerID) {
            return scores[playerID];
		}

		public static void IncrementScore(int playerID, int value) {
			SetScore(playerID, GetPlayerScore(playerID).RoundScore + value);
		}

		public static void SetScore(int playerID, int value) {
			if (!gameOver) {
				scoreUpdated.Invoke(new PlayerScoreUpdate(playerID, value, GetPlayerScore(playerID).RoundScore));
				GetPlayerScore(playerID).RoundScore = value;
			}
		}

		public static bool Paused {
			get { return PauseMenu.paused; }
		}

        public static void NewGame(ElevenConfig currentGame) {
            Reset();
            foreach (var playerID in PlayerManager.players.Keys) {
				if (!scores.ContainsKey(playerID)) {
	                scores.Add(playerID, new PlayerScore(playerID));
				}
            }
				
            Config.Instance.activeGame = currentGame;
            SceneLoader.Instance.Load(currentGame.mainGameScene, false);
        }

		public static void SetTimer(string timer) {
			if (GameCanvas.Instance != null) {
				GameCanvas.Instance.SetTimer(timer);
			}
		}

		public static void DisplayTitle(string text, float timeToShow = 5.0f) {
			if (GameCanvas.Instance != null) {
				GameCanvas.Instance.DisplayTitle(text, timeToShow);		
			}
		}

		public static void HideTitle() {
			if (GameCanvas.Instance != null) {
				GameCanvas.Instance.HideTitle();		
			}
		}

		public static void PlayerSpawned(PlayerInput input, GameObject spawnedObject) {
			if (GameCanvas.Instance != null) {
				GameCanvas.Instance.PlayerSpawned(input, spawnedObject);
			}
		}

		public static void PlayerSpawned(PlayerInput input, GameObject spawnedObject, Vector2 size) {
			if (GameCanvas.Instance != null) {
				GameCanvas.Instance.PlayerSpawned(input, spawnedObject, size);
			}
		}

		public static void PlayerSpawned(PlayerInput input, GameObject spawnedObject, Vector2 size, Vector3 offset) {
			if (GameCanvas.Instance != null) {
				GameCanvas.Instance.PlayerSpawned(input, spawnedObject, size, offset);
			}
		}

		public static void PlayerSpawned(PlayerInput input, GameObject spawnedObject, Vector3 offset) {
			if (GameCanvas.Instance != null) {
				GameCanvas.Instance.PlayerSpawned(input, spawnedObject, offset);
			}
		}

        public static void GameOver() {
            gameOver = true;

//            foreach (var score in scores.Values) {
//                score.PrevScore = score.Score;
//                score.Score += score.RoundScore;
//                score.RoundScore = 0;
//            }

			Config.Instance.activeGame = null;
            SceneLoader.Instance.Load("Results");
        }

        static internal void Reset() {
            gameOver = false;

			foreach (var item in scores) {
				item.Value.RoundScore = 0;
			}
        }

		public static int NameToLayer(string name) {
			return ElevenMask.NameToLayer(name);
		}

		public static string LayerToName(int layer) {
			return ElevenMask.LayerToName(layer);
		}

		public static int GetMask(params string[] layerNames) {
			return ElevenMask.GetMask(layerNames);
		}
    }
}
