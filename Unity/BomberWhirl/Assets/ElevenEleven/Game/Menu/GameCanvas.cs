namespace ElevenEleven.Game {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;

	internal class GameCanvas : Singleton<GameCanvas> {

		[SerializeField] TextMeshProUGUI timerText;
		[SerializeField] TextMeshProUGUI titleText; 
		[SerializeField] SpawnFollower spawnFollowerPrefab;

		Coroutine displayTitleCoroutine;

		protected override void Awake() {
			base.Awake();
		
			timerText.text = "";
			titleText.text = "";
		}

#if PARTY_MODE
		void Update() {
			if (Input.GetKeyDown(KeyCode.R)) {
				ShallowGames.GameOver();
			}
		}
#endif

		internal void SetTimer(string timer) {
			timerText.text = timer;
		}

		internal void DisplayTitle(string text, float timeToShow) {
			if (displayTitleCoroutine != null) {
				StopCoroutine(displayTitleCoroutine);
			}
			displayTitleCoroutine = StartCoroutine(DisplayTitleCoroutine(text, timeToShow));
		}

		internal void HideTitle() {
			if (displayTitleCoroutine != null) {
				StopCoroutine(displayTitleCoroutine);
			}
			titleText.text = "";
		}

		internal void PlayerSpawned(PlayerInput playerInput, GameObject spawnedObject) {
			PlayerSpawned(playerInput, spawnedObject, 128 * Vector2.one);
		}

		internal void PlayerSpawned(PlayerInput playerInput, GameObject spawnedObject, Vector2 size) {
			PlayerSpawned(playerInput, spawnedObject, size, Vector3.zero);			
		}

		internal void PlayerSpawned(PlayerInput playerInput, GameObject spawnedObject, Vector3 offset) {
			PlayerSpawned(playerInput, spawnedObject, 128 * Vector2.one, offset);			
		}

		internal void PlayerSpawned(PlayerInput playerInput, GameObject spawnedObject, Vector2 size, Vector3 offset) {
			SpawnFollower instance = Instantiate<SpawnFollower>(spawnFollowerPrefab);
			instance.transform.SetParent(transform);
			instance.Initialize(playerInput, spawnedObject, size, offset);
		}

		IEnumerator DisplayTitleCoroutine(string text, float timeToShow) {
			titleText.text = text;
			yield return new WaitForSeconds(timeToShow);
			titleText.text = "";
		}
	}
}