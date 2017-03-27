namespace ElevenEleven.Game {
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using UnityEngine.SceneManagement;
    using System.Collections;
    using TMPro;
    using InControl;
    
    internal class GameSelect : MonoBehaviour {
        
        [SerializeField] TextMeshProUGUI m_gameText;
        TextMeshProUGUI gameText {
            get { return m_gameText; }
        }

        [SerializeField] WrappedSlider m_slider;
        WrappedSlider slider {
            get { return m_slider; }
        }

        ElevenConfig m_currentGame;
        ElevenConfig currentGame {
            get { return m_currentGame; }
            set {
                m_currentGame = value;
                GameUpdated();
            }
        }

        void Start() {
            slider.wholeNumbers = true;
            slider.minValue = 0;
            slider.maxValue = Config.Instance.games.Count;

            slider.onValueChanged.AddListener(ValueChanged);
            currentGame = Config.Instance.games[0];
        }
        
        void OnDestroy() {
            slider.onValueChanged.RemoveListener(ValueChanged);
#if USING_INCONTROL
       	 	InControlInputModule.instance.Device = null;
#endif
        }

        void Update() {
            UpdateActiveDevice();
        }

        void UpdateActiveDevice() {
            bool foundDevice = false;
            for (int i = 0; i < 4; i++) {
                if (PlayerManager.IsHuman(i)) {
#if USING_INCONTROL
			        InControlInputModule.instance.Device = PlayerManager.GetHuman(i).device;
#endif
					foundDevice = true;
                    break;
                }
            }

            if (!foundDevice) {
#if USING_INCONTROL
                InControlInputModule.instance.Device = null;
#endif
			}
        }

        void ValueChanged(float value) {
            currentGame = Config.Instance.games[(int)value];
        }

        void GameUpdated() {
            gameText.text = currentGame.gameName;
        }

        public void Play() {
            if (currentGame != null) {
				ShallowGames.NewGame(currentGame);
            } else {
                Debug.LogWarning("The currentGame is not set. No game to load.");
            }
        }
    }
}