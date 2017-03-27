namespace ElevenEleven {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.EventSystems;
	using ElevenEleven.Game;

	internal class PauseMenu : Singleton<PauseMenu> {

		[SerializeField] Button resumeButton;
		[SerializeField] Button quitButton;
		[SerializeField] bool startOn = false;

		CanvasGroup m_canvasGroup;
		CanvasGroup canvasGroup {
			get {
				if (m_canvasGroup == null) {
					m_canvasGroup = GetComponent<CanvasGroup>();
				}
				return m_canvasGroup;
			}
		}

		public static bool paused {
			get { return Instance.memberPaused; }
			private set { Instance.memberPaused = value; 
//				if(value) {
//					//maybe one second delay
//					//EventSystem.current.SetSelectedGameObject(Instance.resumeButton.gameObject);
//					StartCoroutine("LinkMenu");
//				}
			}
		}

		bool memberPaused {
			get {
				return Time.timeScale == 0.0f;
			}
			set {
				GetComponent<Animator>().SetBool("showing", value);				
				Time.timeScale = value ? 0.0f : 1.0f;
				if(value) {
					//maybe one second delay
					//EventSystem.current.SetSelectedGameObject(Instance.resumeButton.gameObject);
					StartCoroutine("LinkMenu");
				} else {
					EventSystem.current.SetSelectedGameObject(null);
				}
			}
		}

		protected override void Awake() {
			base.Awake();

			GetComponent<Animator>().SetBool("showing", startOn);

			resumeButton.onClick.AddListener(Resume);
			quitButton.onClick.AddListener(Quit);
		}

		protected override void OnDestroy() {
			base.OnDestroy();

			resumeButton.onClick.RemoveListener(Resume);
			quitButton.onClick.RemoveListener(Quit);
		}

		void Update() {
			if (!GameManager.Instance.gameStarted) {
				return;
			}

			foreach (var device in ShallowGamesInputModule.instance.Devices) {
				if (device.CommandWasPressed) {
					memberPaused = !memberPaused;
				}
			}
		}

		public void Pause() {
			paused = true;
		}

		public void Resume() {
			paused = false;
		}

		public void Quit() {
			paused = false;
			ShallowGames.GameOver();
		}

		IEnumerator LinkMenu() {
			yield return new WaitForSecondsRealtime(0.1f);
			EventSystem.current.SetSelectedGameObject(Instance.resumeButton.gameObject);
		}
	}
}