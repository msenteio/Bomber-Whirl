namespace ElevenEleven.Game {
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEditor;
	using UnityEditor.UI;

	[CustomEditor(typeof(ElevenConfig), true)]
	public class ElevenConfigEditor : Editor {
       
		SerializedProperty gameName;
		SerializedProperty layers;
		SerializedProperty tags;
		SerializedProperty sortingLayers;
		SerializedProperty mainGamePath;

		Vector2 scrollPosition;
		bool collisionFoldout;

		void OnEnable() {
            
			gameName = serializedObject.FindProperty("m_gameName");
			layers = serializedObject.FindProperty("m_layers");
			tags = serializedObject.FindProperty("m_tags");
			sortingLayers = serializedObject.FindProperty("m_sortingLayers");
			mainGamePath = serializedObject.FindProperty("m_mainGamePath");
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			serializedObject.Update();

//			EditorGUILayout.PropertyField(gameName);
//			EditorGUILayout.PropertyField(layers);
//			EditorGUILayout.PropertyField(collisions);
//			EditorGUILayout.PropertyField(tags);
//			EditorGUILayout.PropertyField(sortingLayers);

			collisionFoldout = EditorGUILayout.Foldout(collisionFoldout, "Collision Mask");
			if (collisionFoldout) {
				
				float size = 14 * (layers.arraySize);
				float labelWidth = 64;
				scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(14 + size));//new Rect(0, 100, Screen.width, 120));
				GUILayout.Label("", GUILayout.Width(labelWidth + size), GUILayout.Height(labelWidth + size));

				for (int i = 0; i < layers.arraySize; i++) {
					var layerInfoI = layers.GetArrayElementAtIndex(i);
					if (i < layers.arraySize) {
						var layerIName = layerInfoI.FindPropertyRelative("m_name");

						GUI.Label(new Rect(0, labelWidth + i * 14, labelWidth, 16), layerIName.stringValue);

						GUIUtility.RotateAroundPivot(90, new Vector2(labelWidth + 14 * (layers.arraySize - i), 0));
						GUI.Label(new Rect(labelWidth + 14 * (layers.arraySize - i ), 0, labelWidth, 16), layerIName.stringValue);
						GUI.matrix = Matrix4x4.identity;
					}

					for (int j = 0; j < layers.arraySize; j++) {
						if (i < j) {
							continue;
						}

						var layerIMask = layerInfoI.FindPropertyRelative("m_mask");
						int maskI = layerIMask.intValue;

						var layerInfoJ = layers.GetArrayElementAtIndex(j);
						var layerJNum = layerInfoJ.FindPropertyRelative("m_layer").intValue;

						bool enabled = (maskI & (1 << layerJNum)) != 0;
				
						EditorGUI.BeginChangeCheck();
						enabled = GUI.Toggle(new Rect(labelWidth + 14 * (layers.arraySize - i - 1), labelWidth + 14 * j, 16, 16), enabled, "");
						if (EditorGUI.EndChangeCheck()) {
							var layerJMask = layerInfoJ.FindPropertyRelative("m_mask");

							if (enabled) {
								layerIMask.intValue |= (1 << j);
								layerJMask.intValue |= (1 << i);
							} else {
								layerIMask.intValue &= ~(1 << j);
								layerJMask.intValue &= ~(1 << i);
							}
						}
					}
				}

				GUILayout.EndScrollView();
			}

			SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(mainGamePath.stringValue);
			EditorGUI.BeginChangeCheck();
			scene = (SceneAsset)EditorGUILayout.ObjectField(scene, typeof(SceneAsset), false);
			if (EditorGUI.EndChangeCheck()) {
                string path = AssetDatabase.GetAssetPath(scene);
                mainGamePath.stringValue = path;
			}

			serializedObject.ApplyModifiedProperties();
		}
    }
}