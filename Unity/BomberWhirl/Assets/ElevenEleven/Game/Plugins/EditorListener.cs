#if UNITY_EDITOR
namespace ElevenEleven.Game {
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System.Collections.Generic;

    [InitializeOnLoad]
    public class EditorListener {

        static bool m_paused = false;
        public static bool Paused {
            get { return m_paused; }
            set { m_paused = value; }
        }

        class PlayerScene {
            public string path;
            public bool first;

            public PlayerScene(string path, bool first) {
                this.path = path;
                this.first = first;
            }
        }

        static List<PlayerScene> neededScenes = new List<PlayerScene>() {
            new PlayerScene("Assets/ElevenEleven/Game/Scenes/Main.unity", true),
            new PlayerScene("Assets/ElevenEleven/Game/Scenes/Menu.unity", false),
            new PlayerScene("Assets/ElevenEleven/Game/Scenes/Results.unity", false),
            new PlayerScene("Assets/ElevenEleven/Game/Scenes/Loading.unity", false)
        };

        static EditorListener() {
            //EditorApplication.hierarchyWindowChanged += SceneChanged;
            EditorApplication.projectWindowChanged += ProjectChanged;
            EditorApplication.update += Update;
        }

        static void ProjectChanged() {

        }

        static void Update() {
            if (!Paused && !EditorApplication.isPlayingOrWillChangePlaymode) {
                AddLoadDefaults();
                AddDefaultScenes();
                AddElevenConfig();
                UpdateSingleton();

                // We suppress other EventSystems from being created
                RemoveExtraneousEventSystems();
            }
        }

        static void AddElevenConfig() {
            if (PlayerSettings.productName != "Shallow Games") {
                ElevenConfig.GetEditorProjectConfig(PlayerSettings.productName);
            }
        }

        static void AddLoadDefaults() {
            LoadDefaults defaults = GameObject.FindObjectOfType<LoadDefaults>();
            if (defaults == null) {
                GameObject go = new GameObject("Shallow Games Defaults");
                go.AddComponent<LoadDefaults>();
            } else {
                defaults.gameObject.name = "Shallow Games Defaults";
            }
        }

        static void AddDefaultScenes() {
            AddScene(neededScenes);
        }

        static void AddScene(List<PlayerScene> scenesToAdd) {

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

            List<PlayerScene> toAdd = new List<PlayerScene>();
            foreach (var scene in scenesToAdd) {
                int foundIndex = scenes.FindIndex(delegate (EditorBuildSettingsScene obj) {
                    return obj.path == scene.path;
                });

                if (scene.first ? foundIndex != 0 : foundIndex < 0) {
                    if (scene.first && foundIndex >= 0) {
                        scenes.RemoveAt(foundIndex);
                    }
                    toAdd.Add(scene);
                } else {
                    if (!scenes[foundIndex].enabled) {
                        scenes[foundIndex].enabled = true;
                    }
                }
            }

            foreach (var add in toAdd) {
                var sceneToAdd = new EditorBuildSettingsScene(add.path, true);
                if (add.first) {
                    scenes.Insert(0, sceneToAdd);
                } else {
                    scenes.Insert(Mathf.Min(1, scenes.Count), sceneToAdd);
                }
            }
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        static void RemoveExtraneousEventSystems() {
            EventSystem[] eventSystems = GameObject.FindObjectsOfType<EventSystem>();
            for (int i = eventSystems.Length - 1; i >= 0; i--) {
                if (eventSystems[i].transform.parent == null || eventSystems[i].transform.parent.name != "Singleton") {
                    GameObject.DestroyImmediate(eventSystems[i].gameObject);
                }
            }
        }

        static void UpdateSingleton() {
            GameObject singleton = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/ElevenEleven/Resources/Singleton.prefab", typeof(GameObject));
            Config config = null;
            config = singleton.GetComponentInChildren<Config>();

            bool changed = false;
            var games = ElevenConfig.games;
            int elevenGameIndex = games.FindIndex(delegate (ElevenConfig obj) {
                return obj != null && obj.name == "Shallow Games Config";
            });

            if (elevenGameIndex >= 0) {
                if (config.elevenGame != games[elevenGameIndex]) {
                    config.elevenGame = games[elevenGameIndex];
                }
                games.RemoveAt(elevenGameIndex);
            }

            for (int i = config.games.Count - 1; i >= 0; i--) {
                if (!games.Contains(config.games[i])) {
                    config.games.RemoveAt(i);
                    changed = true;
                }
            }

            // Add new games
            foreach (var game in games) {
                if (!config.games.Contains(game)) {
                    config.games.Add(game);
                    changed = true;
                }
            }

            if (changed) {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
#endif