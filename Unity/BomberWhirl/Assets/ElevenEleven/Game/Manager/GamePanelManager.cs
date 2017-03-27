namespace ElevenEleven.Game {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	public class GamePanelManager : PrivateSingleton<GamePanelManager> {

		[SerializeField] GameObject gameCanvas;

		protected override void Awake() {
			base.Awake();
			SceneManager.sceneLoaded += SceneLoaded;
		}

		void SceneLoaded(Scene scene, LoadSceneMode loadMode) {
			ElevenConfig activeGame = Config.Instance.activeGame;

			if (activeGame != null && activeGame != Config.Instance.elevenGame && activeGame.mainGameScene == scene.path) {
				Instantiate<GameObject>(gameCanvas);
			}
		}
	}
}
