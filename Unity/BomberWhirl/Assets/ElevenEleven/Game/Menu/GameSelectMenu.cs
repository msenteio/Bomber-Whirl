namespace ElevenEleven.Game {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public class GameSelectMenu : MonoBehaviour {

		static List<int> randomOrder = new List<int>();
		static int gameIndex = -1;

		[SerializeField] GameSelectButton selectButtonPrefab;
		[SerializeField] GameObject selectGameContentParent;
		[SerializeField] GamePreviewItem previewItemPrefab;
		[SerializeField] GameObject previewGameContentParent;
		[SerializeField] Color[] buttonColors;
		[SerializeField] Image bufferImagePrefab;

		List<GameSelectButton> games = new List<GameSelectButton>();

		void Start() {
			buttonColors.Shuffle();

			CreateBuffer(previewGameContentParent, buttonColors[(Config.Instance.games.Count - 1) % buttonColors.Length]);
			CreateBuffer(selectGameContentParent, buttonColors[(Config.Instance.games.Count - 1) % buttonColors.Length]);
			for (int i = 0; i < Config.Instance.games.Count; i++) {
				var game = Config.Instance.games[i];

				GameSelectButton selectInstance = Instantiate<GameSelectButton>(selectButtonPrefab);
				GamePreviewItem previewInstance = Instantiate<GamePreviewItem>(previewItemPrefab);
				Color color = buttonColors[i % buttonColors.Length];

				previewInstance.Initialize(game, selectInstance, color);
				previewInstance.transform.SetParent(previewGameContentParent.transform);
				previewInstance.transform.localScale = Vector3.one;
//				previewInstance.transform.SetSiblingIndex(previewGameContentParent.transform.childCount - 2);

				selectInstance.Initialize(game, previewInstance, color);
				selectInstance.transform.SetParent(selectGameContentParent.transform);
				selectInstance.transform.localScale = Vector3.one;
//				selectInstance.transform.SetSiblingIndex(selectGameContentParent.transform.childCount - 2);
			
				games.Add(selectInstance);
			}
			CreateBuffer(previewGameContentParent, buttonColors[(Config.Instance.games.Count - 1) % buttonColors.Length]);
			CreateBuffer(selectGameContentParent, buttonColors[(Config.Instance.games.Count) % buttonColors.Length]);

			if (games.Count > 0) {
				EventSystem.current.SetSelectedGameObject(games[games.Count / 2].gameObject);
			}


#if PARTY_MODE
			WaitToStart();
#endif
		}

		void CreateBuffer(GameObject parent, Color color) {
			Image bufferImage = Instantiate<Image>(bufferImagePrefab);
			bufferImage.transform.SetParent(parent.transform);
			bufferImage.transform.localScale = Vector3.one;
			bufferImage.color = color.NewAlpha(bufferImage.color.a);
		}

#if PARTY_MODE
		void WaitToStart() {
			StartCoroutine(WaitToStartCoroutine());
		}

		IEnumerator WaitToStartCoroutine() {
			while (!InControl.InputManager.AnyKeyIsPressed) {
				bool buttonPressed = false;
				foreach (var controller in InControl.InputManager.Devices) {
					if (controller.AnyButtonIsPressed) {
						buttonPressed = true;
						break;
					}
				}

				if (buttonPressed) {
					break;
				} else {
					yield return null;
				}
			}

			GameSelectButton activeGame = null;
			for (int i = 0; i < 10; i++) {
				activeGame = games[Random.Range(0, games.Count)];
				EventSystem.current.SetSelectedGameObject(activeGame.gameObject);
				yield return new WaitForSeconds(0.25f);
			}

			if (randomOrder.Count == 0) {
				for (int i = 0; i < games.Count; i++) {
					randomOrder.Add(i);
				}
				randomOrder.Shuffle();

				if (gameIndex == randomOrder[randomOrder.Count - 1]) {
					int temp = randomOrder[0];
					randomOrder[0] = randomOrder[randomOrder.Count - 1];
					randomOrder[randomOrder.Count - 1] = temp;
				}
			}

			gameIndex = randomOrder[randomOrder.Count - 1];
			activeGame = games[gameIndex];
			randomOrder.RemoveAt(randomOrder.Count - 1);
			EventSystem.current.SetSelectedGameObject(activeGame.gameObject);
		
			yield return new WaitForSeconds(1.0f);
			activeGame.PlayGame();
		}
#endif
	}
}