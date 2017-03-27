
namespace ElevenEleven.Game {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using ElevenEleven;
#if UNITY_EDITOR
    using UnityEditor;
	using System.Reflection;
#endif

    [System.Serializable]
    public class LayerInfo {
        [SerializeField] int m_layer;
        public int layer {
            get { return m_layer; }
            private set { m_layer = value; }
        }

        [SerializeField] string m_name;
        public string name {
            get { return m_name; }
            private set { m_name = value; }
        }

		[SerializeField] int m_mask;
		public int mask {
			get { return m_mask; }
			set { m_mask = value; }
		}

		public LayerInfo(int layer, string name, int mask) {
            this.layer = layer;
            this.name = name;
			this.mask = mask;
        }

		public override string ToString() {
			return name;
		}
    }

    [System.Serializable]
    public class SortingLayerInfo {
		[SerializeField] int m_layer;
		public int layer {
			get { return m_layer; }
			private set { m_layer = value; }
		}

		[SerializeField] string m_name;
		public string name {
			get { return m_name; }
			private set { m_name = value; }
		}

        [SerializeField]
        int m_uniqueID;
        public int uniqueID {
            get { return m_uniqueID; }
            private set { m_uniqueID = value; }
        }

		public SortingLayerInfo(int layer, string name, int uniqueID) {
			this.layer = layer;
			this.name = name;
            this.uniqueID = uniqueID;
        }
    }

//	[System.Serializable]
//	public class Collision : Pair<int, int> { 
//		public Collision(int first, int second) : base(first, second) { }
//	}

    public class ElevenConfig : ScriptableObject {

        static List<ElevenConfig> m_games;
        public static List<ElevenConfig> games {
            get {
                ElevenConfig[] tmpGames = Resources.LoadAll<ElevenConfig>("Game Configs");
                if (m_games == null || m_games.Count > tmpGames.Length) {
                    m_games = new List<ElevenConfig>(tmpGames);
                } else {
                    foreach (var game in tmpGames) {
                        if (!m_games.Contains(game)) {
                            m_games.Add(game);
                        }
                    }
                }
                return m_games;
            }
        }

#if UNITY_EDITOR
        public static void UpdateProjectFromGames() {
            ResetProject();
            foreach (var game in games) {
                game.UpdateProjectFromConfig();
            }
        }

        static void ResetProject() {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            tagManager.Update();

            SerializedProperty tagsProp = tagManager.FindProperty("tags");
            SerializedProperty sortLayersProp = tagManager.FindProperty("m_SortingLayers");
            
            tagsProp.ClearArray();

            for (int i = 0; i < sortLayersProp.arraySize; i++) {
                SerializedProperty entry = sortLayersProp.GetArrayElementAtIndex(i);
                SerializedProperty name = entry.FindPropertyRelative("name");
                SerializedProperty unique = entry.FindPropertyRelative("uniqueID");
                SerializedProperty locked = entry.FindPropertyRelative("locked");

                Debug.Log(name.stringValue + " => " + unique.longValue + " => " + locked.boolValue);
                //while (entry.Next(true)) {
                //Debug.Log(entry.name + " " + entry.propertyPath + " " + entry.propertyType);
                //Debug.Log(entry.propertyPath + " " + entry.intValue);
                //}
            }

            for (int i = sortLayersProp.arraySize - 1; i >= 0; i--) {
                SerializedProperty entry = sortLayersProp.GetArrayElementAtIndex(i);
                SerializedProperty name = entry.FindPropertyRelative("name");
                if (name.stringValue != "Default") {
                    sortLayersProp.DeleteArrayElementAtIndex(i);
                }
            }

            tagManager.ApplyModifiedProperties();
        }
#endif

        [SerializeField] string m_gameName;
        public string gameName {
            get { return m_gameName; }
            private set { m_gameName = value; }
        }

        [SerializeField]
        Texture m_previewImage;
        public Texture previewImage {
            get { return m_previewImage; }
            set { m_previewImage = value; }
        }

		[SerializeField]
		int m_gameLength = 60;
		public int gameLength {
			get { return m_gameLength; }
		}

        [SerializeField]
        List<LayerInfo> m_layers = new List<LayerInfo>();
        public List<LayerInfo> layers {
            get { return m_layers; }
            private set { m_layers = value; }
        }

//        [SerializeField]
//        List<Collision> m_collisions = new List<Collision>();
//        public List<Collision> collisions {
//            get { return m_collisions; }
//            private set { m_collisions = value; }
//		}
//
//		[SerializeField]
//		List<int> m_collisionMasks = new List<int>();
//		public List<int> collisionMasks {
//			get { return m_collisionMasks; }
//		}

		[SerializeField]
		List<string> m_tags = new List<string>();
		public List<string> tags {
			get { return m_tags; }
			private set { m_tags = value; }
		}

		[SerializeField]
		List<SortingLayerInfo> m_sortingLayers = new List<SortingLayerInfo>();
		public List<SortingLayerInfo> sortingLayers {
			get { return m_sortingLayers; }
			private set { m_sortingLayers = value; }
		}

		[SerializeField] string m_mainGamePath;
		public string mainGamePath {
			get { return m_mainGamePath; }
			set { m_mainGamePath = value; }
		}

        public string mainGameScene {
            get {
				return mainGamePath;
//                int assetsLength = "Assets/".Length;
//                int fileTypeLength = ".unity".Length;
//                return mainGamePath.Substring(assetsLength, mainGamePath.Length - assetsLength - fileTypeLength);
            }
        }

		Dictionary<string, int> nameToLayer = new Dictionary<string, int>();
		Dictionary<int, string> layerToName = new Dictionary<int, string>();

		void OnEnable() {
			foreach (var layer in layers) {
                if (!string.IsNullOrEmpty(layer.name)) {
                    nameToLayer.Add(layer.name, layer.layer);
                    layerToName.Add(layer.layer, layer.name);
                }
			}
		}

		public void SetActive() {
			UpdateGamePhysics();
		}

		void UpdateGamePhysics() {
			Dictionary<int, LayerInfo> layersDict = new Dictionary<int, LayerInfo>();
			foreach (var layer in layers) {
				layersDict.Add(layer.layer, layer);
			}

			for (int i = 0; i < 32; i++) {
				for (int j = 0; j <= i; j++) {
					bool ignore = true;
					if (layersDict.ContainsKey(i)) {
						ignore = (layersDict[i].mask & (1 << j)) == 0;
					}
					Physics2D.IgnoreLayerCollision(i, j, ignore);
					Physics.IgnoreLayerCollision(i, j, ignore);
				}
			}

//			for (int i = 0; i < 32; i++) {
//				for (int j = 0; j < 32; j++) {
//					Physics2D.IgnoreLayerCollision(i, j, true);
//					Physics.IgnoreLayerCollision(i, j, true);
//				}
//			}
//
//			foreach (var collision in collisions) {
//				Physics2D.IgnoreLayerCollision(collision.first, collision.second, false);		
//				Physics.IgnoreLayerCollision(collision.first, collision.second, false);
//			}
		}

		public int NameToLayer(string name) {
			return nameToLayer[name];
		}

		public string LayerToName(int layer) {
			return layerToName[layer];
		}

		public int GetMask(params string[] layerNames) {
			int toReturn = 0;
			for (int i = 0; i < layerNames.Length; i++) {
				toReturn |= (1 << NameToLayer(layerNames[i]));
			}
			return toReturn;
		}

		public static ElevenConfig GetPlayerProjectConfig(string name) {
			ElevenConfig config = Resources.Load<ElevenConfig>("Game Configs/" + name);
			return config;
		}

#if UNITY_EDITOR

		static string ProjectConfigPath(string name, string resourcesPath) {
			return string.Format(resourcesPath + "/Game Configs/{0} Config.asset", name);
		}

        public static ElevenConfig GetEditorProjectConfig(string name) {
            return GetEditorProjectConfig(name, "Assets/Resources");
        }

        public static ElevenConfig GetEditorProjectConfig(string name, string resourcesPath) {
			string pathName = ProjectConfigPath(name, resourcesPath);
            ElevenConfig toReturn = (ElevenConfig)AssetDatabase.LoadAssetAtPath(pathName, typeof(ElevenConfig));
            if (toReturn == null) {
	 			if (!AssetDatabase.IsValidFolder(resourcesPath + "/Game Configs")) {
					if (!AssetDatabase.IsValidFolder(resourcesPath)) {
                        int index = resourcesPath.LastIndexOf('/');
                        AssetDatabase.CreateFolder(resourcesPath.Substring(0, index), resourcesPath.Substring(index + 1));
					}
					AssetDatabase.CreateFolder(resourcesPath, "Game Configs");
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
				toReturn = ElevenConfig.CreateInstance<ElevenConfig>();
				toReturn.FillFromCurrentProject(name);
                AssetDatabase.CreateAsset(toReturn, pathName);
            }

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
            return toReturn;
        }

        public static void DeleteProjectConfig(string name) {
            DeleteProjectConfig(name, "Assets/Resources");
        }

        public static void DeleteProjectConfig(string name, string resourcesPath) {
            string pathName = ProjectConfigPath(name, resourcesPath);
			AssetDatabase.DeleteAsset(pathName);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		void FillFromCurrentProject(string name) {            
			gameName = name;	
			FillLayersFromProject();
			FillCollisionsFromProject();
			FillSortingLayersFromProject();
            FillTagsFromProject();
		}

		void FillCollisionsFromProject() {
//			collisions.Clear();
//			for (int i = 0; i < 32; i++) {
//				for (int j = i; j < 32; j++) {
//					int iLayerMask = Physics2D.GetLayerCollisionMask (i);
//					if (((1 << j) & (iLayerMask)) != 0) {
//						Collision foundCollision = collisions.Find (delegate(Collision obj) {
//							return obj.first == i && obj.second == j;
//						});
//						if (foundCollision == null) {
//							collisions.Add (new Collision (i, j));
//						}
//					}
//				}
//			}
//
//			collisionMasks.Clear();
//			for (int i = 0; i < 32; i++) {
//				collisionMasks.Add(Physics2D.GetLayerCollisionMask (i));
//			}
		}

		void FillLayersFromProject() {
            layers.Clear();

			// Open tag manager
			SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
			SerializedProperty layersProp = tagManager.FindProperty("layers");
            
			for (int i = 0; i < layersProp.arraySize; i++) {
				SerializedProperty l = layersProp.GetArrayElementAtIndex(i);
				if (!string.IsNullOrEmpty(l.stringValue)) {
					Debug.Log (Physics2D.GetLayerCollisionMask (i));
					layers.Add(new LayerInfo(i, l.stringValue, Physics2D.GetLayerCollisionMask (i)));
				}
			}
		}

        int FindDefaultSortingLayer() {
            SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty sortLayersProp = manager.FindProperty("m_SortingLayers");
            return FindDefaultSortingLayer(sortLayersProp);
        }

        int FindDefaultSortingLayer(SerializedProperty sortLayersProp) {
            for (int i = 0; i < sortLayersProp.arraySize; i++) {
                SerializedProperty entry = sortLayersProp.GetArrayElementAtIndex(i);
                SerializedProperty name = entry.FindPropertyRelative("name");
                if (name.stringValue == "Default") {
                    return i;
                }
            }

            throw new System.MissingFieldException("Could not find \"Default\" sorting layer. Let Denver know if this happens!");
        }

        void FillSortingLayersFromProject() {
            sortingLayers.Clear();

            SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
			SerializedProperty sortLayersProp = manager.FindProperty("m_SortingLayers");

            int defaultLayerIndex = FindDefaultSortingLayer(sortLayersProp);

            for (int i = 0; i < sortLayersProp.arraySize; i++) {
                SerializedProperty entry = sortLayersProp.GetArrayElementAtIndex(i);
                SerializedProperty name = entry.FindPropertyRelative("name");
                SerializedProperty unique = entry.FindPropertyRelative("uniqueID");
                SerializedProperty locked = entry.FindPropertyRelative("locked");
                //			    Debug.Log(name.stringValue + " => " + unique.intValue + " => " + locked.boolValue);
                if (name.stringValue != "Default") {
                    sortingLayers.Add(new SortingLayerInfo(i - defaultLayerIndex, name.stringValue, unique.intValue));
                }
            }
		}

        void FillTagsFromProject() {
            tags.Clear();

            // Open tag manager
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            tags.Clear();
            for (int i = 0; i < tagsProp.arraySize; i++) {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                tags.Add(t.stringValue);
            }
        }

		public void UpdateProjectFromConfig() {
			AppendProjectTags();
            AppendSortingLayers();
        }

        void AppendProjectTags() {
            // Open tag manager
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            tagManager.Update();

            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            // Adding Tags
            foreach (var s in tags) {
	            // First check if it is not already present
	            bool found = false;
	            for (int i = 0; i < tagsProp.arraySize; i++) {
	                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
	                if (t.stringValue.Equals(s)) { found = true; break; }
	            }

	            if (!found) {
					tagsProp.InsertArrayElementAtIndex(0);
	                SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
	                n.stringValue = s;
	            }
			}

			tagManager.ApplyModifiedPropertiesWithoutUndo();
        }

        void AppendSortingLayers() {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            tagManager.Update();
            SerializedProperty sortLayersProp = tagManager.FindProperty("m_SortingLayers");
            
            // sort layers based on layer number
            sortingLayers.Sort(delegate (SortingLayerInfo a, SortingLayerInfo b) {
                return a.layer - b.layer;
            });

            foreach (var sortingLayer in sortingLayers) {
                // check if tag is present
                bool found = false;
                bool uniqueIDSame = false;
                for (int i = 0; i < sortLayersProp.arraySize; i++) {
                    SerializedProperty entry = sortLayersProp.GetArrayElementAtIndex(i);
                    SerializedProperty t = entry.FindPropertyRelative("name");
                    SerializedProperty unique = entry.FindPropertyRelative("uniqueID");
                    if (t.stringValue.Equals(sortingLayer.name)) {
                        found = true;
                        break;
                    } else if (unique.intValue == sortingLayer.layer) {
                        found = true;
                        uniqueIDSame = true;
                        break;
                    }
                }

                if (found) {
                    if (uniqueIDSame) {
                        throw new System.NotSupportedException(
                            string.Format("Oops. It looks like a minor error. Occurred. " +
                            "Please let Denver know the following information: \nLayer Name: {0}\nUnique ID: {1}",
                            sortingLayer.name, sortingLayer.layer)
                        );
                    } else {
                        throw new System.NotSupportedException(
                            string.Format("Sorting layer, {0}, is not unique. Please change its name.",
                            sortingLayer.name)
                        );
                    }
                }
            }


            int defaultLayerIndex = FindDefaultSortingLayer(sortLayersProp);

            foreach (var sortingLayer in sortingLayers) {
                int index;
                if (sortingLayer.layer < 0) {
                    index = defaultLayerIndex;
                    defaultLayerIndex++;
                } else {
                    index = defaultLayerIndex + sortingLayer.layer;
                }
                sortLayersProp.InsertArrayElementAtIndex(index);

                SerializedProperty entry = sortLayersProp.GetArrayElementAtIndex(index);
                SerializedProperty name = entry.FindPropertyRelative("name");
                SerializedProperty unique = entry.FindPropertyRelative("uniqueID");
                SerializedProperty locked = entry.FindPropertyRelative("locked");
                
                name.stringValue = sortingLayer.name;
                unique.longValue = (uint)sortingLayer.uniqueID;
                locked.boolValue = false;
            }

            tagManager.ApplyModifiedPropertiesWithoutUndo();
        }

//        private static Assembly editorAsm;
//        private static MethodInfo AddSortingLayer_Method;
//
//        ///  add a new sorting layer with default name 
//        public static void AddSortingLayer() {
//            if (AddSortingLayer_Method == null) {
//                if (editorAsm == null) editorAsm = Assembly.GetAssembly(typeof(Editor));
//                System.Type t = editorAsm.GetType("UnityEditorInternal.InternalEditorUtility");
//                AddSortingLayer_Method = t.GetMethod("AddSortingLayer", (BindingFlags.Static | BindingFlags.NonPublic), null, new System.Type[0], null);
//            }
//            AddSortingLayer_Method.Invoke(null, null);
//        }
#endif
    }
}