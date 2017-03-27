namespace ElevenEleven.Game {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

    [System.Serializable]
    internal class GameDetails {
        public string name;
        public string scene;

        private ElevenConfig m_config = null;
        ElevenConfig config {
            get {
                if (m_config == null) {
                    m_config = ElevenConfig.GetPlayerProjectConfig(name);
#if UNITY_EDITOR
                    if (m_config == null) {
                        m_config = ElevenConfig.GetEditorProjectConfig(name);
                    }
#endif
                }
                return m_config;
            }
        }
    }

	internal class Config : Singleton<Config> {
		
        [SerializeField]
        ElevenConfig m_elevenGame;
        public ElevenConfig elevenGame {
            get { return m_elevenGame; }
            set { m_elevenGame = value; }
        }

        [SerializeField]
        List<ElevenConfig> m_games;
        public List<ElevenConfig> games {
            get { return m_games; }
        }

		ElevenConfig m_prevGame;
		public ElevenConfig prevGame {
			get {
				return m_prevGame;
			}
			private set {
				m_prevGame = value;
			}
		}

        ElevenConfig m_activeGame;
        public ElevenConfig activeGame {
            get {
                return m_activeGame;
            }
            set {
				if (m_activeGame != value) {
					prevGame = m_activeGame;
					m_activeGame = value;

					if (activeGame != null) {
	                	activeGame.SetActive();
					} 
				}
            }
        }

        internal static bool Available {
			get { 
#if UNITY_EDITOR
				return Config.Instance.activeGame != null;
#else
				return true;
#endif
			}
		}

		public static int NameToLayer(string name) {
			return Config.Instance.activeGame.NameToLayer(name);
		}

		public static string LayerToName(int layer) {
			return Config.Instance.activeGame.LayerToName(layer);
		}

		public static int GetMask(params string[] layerNames) {
			int toReturn = 0;
			for (int i = 0; i < layerNames.Length; i++) {
				toReturn |= (1 << NameToLayer(layerNames[i]));
			}
			return toReturn;
		}

        protected override void Awake() {
            base.Awake();
            activeGame = elevenGame;
		}

		protected override void OnDestroy () {
			base.OnDestroy ();

#if UNITY_EDITOR
			ElevenConfig foundLocalGame = games.Find(delegate(ElevenConfig obj) {
				return obj.gameName == Application.productName;
			});

			if (foundLocalGame != null) {
				foundLocalGame.SetActive();
			}
#endif
		}
    }

	internal static class ElevenMask {

		public static int NameToLayer(string name) {
			if (Config.Available) {
				return Config.NameToLayer(name);
			} else {
				return LayerMask.NameToLayer(name);
			}
		}

		public static string LayerToName(int layer) {
			if (Config.Available) {
				return Config.LayerToName(layer);
			} else {
				return LayerMask.LayerToName(layer);
			}
		}

		public static int GetMask(params string[] layerNames) {
			if (Config.Available) {
				return Config.GetMask(layerNames);
			} else {
				return LayerMask.GetMask(layerNames);
			}
		}
	}
}
