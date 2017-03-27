namespace ElevenEleven.Game {
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
    using System.Collections;
    using InControl;
    using System.Collections.Generic;

    [System.Serializable]
    public class GameReady : UnityEvent<bool> { }

	public class Lobby : Singleton<Lobby> {

        [SerializeField] UnityEvent m_gameReady;
        UnityEvent gameReady {
            get { return m_gameReady; }
        }

        [SerializeField] UnityEvent m_gameNotReady;
        UnityEvent gameNotReady {
            get { return m_gameNotReady; }
        }

        HashSet<PlayerInput> activePlayers = new HashSet<PlayerInput>();

        bool m_ready = false;
        bool ready {
            get { return m_ready; }
            set {
                if (m_ready != value) {
                    m_ready = value;
                    if (value) {
                        gameReady.Invoke();
                    } else {
                        gameNotReady.Invoke();
                    }
                }
            }
        }

		protected override void Awake() {
            base.Awake();

            //PlayerManager.Clear();

			foreach (var player in PlayerManager.players.Values) {
				activePlayers.Add(player);
			}
		}
		
        void Start() {
            PlayerManager.inputAdded.AddListener(PlayerAdded);
            PlayerManager.inputRemoved.AddListener(PlayerRemoved);
			UpdateReadiness();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            PlayerManager.inputAdded.RemoveListener(PlayerAdded);
            PlayerManager.inputRemoved.RemoveListener(PlayerRemoved);
        }

		void Update() {
            CheckForNewDevices();
        }

		void CheckForNewDevices() {
            foreach (var device in InputManager.Devices) {
                if (device.Action1.WasPressed && !PlayerManager.Contains(device)) {
                    PlayerManager.AddHuman(device);
                } else if (device.Action2.WasPressed && PlayerManager.Contains(device)) {
#if !PARTY_MODE
                    PlayerManager.RemoveHuman(device);
#endif
                }
            }
        }

		Dictionary<PlayerInput, Coroutine> playerAddedCoroutines = new Dictionary<PlayerInput, Coroutine>();

        void PlayerAdded(PlayerInput player) {
			playerAddedCoroutines.Add(player, StartCoroutine(PlayerAddedCoroutine(player)));
		}

		IEnumerator PlayerAddedCoroutine(PlayerInput player) {
			yield return null;
			yield return new WaitForSeconds(1.0f);

#if !PARTY_MODE
			ShallowGamesInputModule.instance.Devices.Add(player.device);
			activePlayers.Add(player);
#endif

			UpdateReadiness();
		}

        void PlayerRemoved(PlayerInput player) {
			ShallowGamesInputModule.instance.Devices.Remove(player.device);
			activePlayers.Remove(player);
			UpdateReadiness();
        }

		void UpdateReadiness() {
#if UNITY_EDITOR
			ready = activePlayers.Count >= 1;
#else
			ready = activePlayers.Count >= 2;
#endif
		}
    }
}