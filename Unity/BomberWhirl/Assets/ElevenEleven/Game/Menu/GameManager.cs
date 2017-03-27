namespace ElevenEleven.Game {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	internal class GameManager : Singleton<GameManager> {

		bool m_gameStarted = false;
		public bool gameStarted {
			get { return m_gameStarted; }
			private set { m_gameStarted = value; }
		}

		private bool m_wait;
		public bool wait {
			get {
				return m_wait;
			}

			set {
				m_wait = value;
			}
		}

		[SerializeField] DisplayedPlayerScore[] playerScores;
		[SerializeField] CanvasGroup waitingToStartGroup;

		void Start() {
			Time.timeScale = 0.0f;
			StartCoroutine(WaitToStart());
		}

		IEnumerator WaitToStart() {
			yield return new WaitForSecondsRealtime(0.5f);

			Dictionary<int, bool> playersReady = new Dictionary<int, bool>();
			foreach (var item in PlayerManager.players) {
				playersReady.Add(item.Key, false);
				playerScores[item.Key].Waiting();
			}

			bool startGame = false;
			while (!startGame) {
				foreach (var item in PlayerManager.players) {
					if (item.Value.FirstActionWasPressed) {
						playersReady[item.Key] = true;
						playerScores[item.Key].PlayerReady();

						startGame = true;
						foreach (var playerReady in playersReady.Values) {
							if (!playerReady) {
								startGame = false;
								break;
							}
						}
					}
				}

				yield return null;
			}

			// we can start game
			for (int i = 3; i > 0; i--) {
				ShallowGames.DisplayTitle("Starting in\n" + i);
				yield return new WaitForSecondsRealtime(1.0f);
			}

			foreach (var item in PlayerManager.players) {
				playerScores[item.Key].GameStarted();
			}

			ShallowGames.HideTitle();
			gameStarted = true;
			Time.timeScale = 1.0f;
			waitingToStartGroup.alpha = 0.0f;

			while(wait) {
				yield return null;
			}

			for (float dt = Config.Instance.activeGame.gameLength; dt >= 0; dt -= Time.deltaTime) {
				string indexStr = Mathf.Ceil(dt).ToString();//.ToString(dt < 1.0f ? "0.0" : "0");

				if (dt <= 3) {
					ShallowGames.DisplayTitle(indexStr);
					ShallowGames.SetTimer("");
				} else {
					ShallowGames.SetTimer(indexStr);
				}

				yield return null;
			}

			Time.timeScale = 0.0f;
			ShallowGames.DisplayTitle("Game!");
			yield return new WaitForSecondsRealtime(2.0f);

			Time.timeScale = 1.0f;
			ShallowGames.GameOver();
		}
	}
}