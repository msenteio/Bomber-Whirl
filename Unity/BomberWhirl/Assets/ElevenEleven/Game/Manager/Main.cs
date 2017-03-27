namespace ElevenEleven.Game {
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections;

    internal class Main {

        static bool loaded;
        public static bool Loaded {
            get {
                return loaded;
            }
            private set {
                loaded = value;
            }
        }

        static string m_levelToLoad = "Menu";
        static string LevelToLoad {
            get { return m_levelToLoad; }
            set { m_levelToLoad = value; }
        }

        public static void LoadDefaults() {
            if (!Loaded) {
                LoadSingletons();
                Debug.Log(SceneManager.GetActiveScene());
                SceneLoader.Instance.Load(SceneManager.GetActiveScene().name, true);
            }
        }

        public static void FreshStart() {
            if (!Loaded) {
                LoadSingletons();
                SceneLoader.Instance.Load(LevelToLoad, true);
            }
        }

        static void LoadSingletons() {
            if (!Loaded) {
#if UNITY_EDITOR
                Debug.ClearDeveloperConsole();
#endif

                Cursor.visible = false;
                GameObject.Instantiate(Resources.Load<GameObject>("Singleton"));

                Loaded = true;
            }
        }
    }
}
