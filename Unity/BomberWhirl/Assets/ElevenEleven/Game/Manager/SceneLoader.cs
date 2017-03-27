namespace ElevenEleven.Game {
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections;
    using System.Collections.Generic;
    using ElevenEleven;
    using InControl;

    internal class SceneLoader : Singleton<SceneLoader> {
        

        [SerializeField]
        string loadingSceneName = "Loading";

		[SerializeField]
		LoadSceneMode loadingMode = LoadSceneMode.Single;

        int? m_levelToLoadIndex = null;
        public int? LevelToLoadIndex {
            get { return m_levelToLoadIndex; }
            private set { m_levelToLoadIndex = value; }
        }

        string m_levelToLoad = null;
        public string LevelToLoad {
            get { return m_levelToLoad; }
            private set { m_levelToLoad = value; }
        }

        string m_previousLevel;
        public string PreviousLevel {
            get { return m_previousLevel; }
            private set { m_previousLevel = value; }
        }
        
        protected override void Awake() {
            base.Awake();
            SceneManager.sceneLoaded += SceneLoaded;
        }

        public void Load(string level, bool instant = false) {
            PreviousLevel = SceneManager.GetActiveScene().name;
            if (instant) {
                SceneManager.LoadScene(level);
            } else {
                LevelToLoad = level;
				SceneManager.LoadSceneAsync(loadingSceneName, loadingMode);
            }
        }

        public void Load(int level, bool instant = false) {
            PreviousLevel = SceneManager.GetActiveScene().name;
            if (instant) {
                SceneManager.LoadScene(level);
            } else {
                LevelToLoadIndex = level;
				SceneManager.LoadSceneAsync(loadingSceneName, loadingMode);
            }
        }

        void SceneLoaded(Scene scene, LoadSceneMode loadMode) {
            StartCoroutine(SceneLoadedCoroutine(scene, loadMode));
        }
        
        IEnumerator SceneLoadedCoroutine(Scene scene, LoadSceneMode loadMode) {
            yield return new WaitForSeconds(1.0f);
            
            if (scene.name == loadingSceneName) {
                if (!string.IsNullOrEmpty(LevelToLoad)) {
                    string toLoad = LevelToLoad;
                    LevelToLoad = null;
                    LevelToLoadIndex = null;
                    SceneManager.LoadSceneAsync(toLoad);
                } else if (LevelToLoadIndex.HasValue) {
                    int toLoad = LevelToLoadIndex.Value;
                    LevelToLoad = null;
                    LevelToLoadIndex = null;
                    SceneManager.LoadSceneAsync(toLoad);
                } else {
                    SceneManager.LoadSceneAsync(0);
                }
            }
        }
    }
}
