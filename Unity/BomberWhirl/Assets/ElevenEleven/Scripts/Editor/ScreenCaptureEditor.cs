/// <summary>
/// Copyright (c) 2016 11:11 Studios LLC
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this 
/// software and associated documentation files (the "Software"), to deal in the Software 
/// without restriction, including without limitation the rights to use, copy, modify, merge, 
/// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons 
/// to whom the Software is furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included 
/// in all copies or substantial portions of the Software.
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
/// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
/// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
/// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
/// </summary>

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ElevenEleven {
    public class ScreenCaptureEditor : EditorWindow {
        public int screenScale = 1;
        public string screenshotName = "Screenshot.png";

        // Add menu item named "My Window" to the Window menu
        [MenuItem("11:11 Studios/Screen Capture")]
        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
			EditorWindow.GetWindow<ScreenCaptureEditor>("Screen Capture", true);
        }

        void OnGUI() {
            EditorGUILayout.LabelField("Screen Scale");
            screenScale = EditorGUILayout.IntField(screenScale);

            EditorGUILayout.LabelField("Screenshot Name");
            string tmp = EditorGUILayout.TextField(screenshotName);
            if (screenshotName != tmp) {
                UpdateScreenName(tmp);
            }

            Vector2 size = GetMainGameViewSize();
            EditorGUILayout.LabelField((screenScale * size.x) + " x " + (screenScale * size.y));

            EditorGUILayout.BeginHorizontal();

            //		if (GUILayout.Button("Change Language")) {
            //			Localization.Language = Localization.Language == "en" ? "zh" : "en";
            //		}

            if (GUILayout.Button("Take Screenshot")) {
                UpdateScreenName(screenshotName);
                Debug.Log("Saving to " + Directory.GetParent(Application.dataPath).FullName + "/" + screenshotName + "...");
                Application.CaptureScreenshot(screenshotName, screenScale);
                //Debug.Log("Saved!");
            }

            EditorGUILayout.EndHorizontal();
        }

        void UpdateScreenName(string tmp) {
            tmp = tmp.Replace(".png", "");
            screenshotName = tmp + ".png";
            int i = 1;
            while (File.Exists(Directory.GetParent(Application.dataPath).FullName + "/" + screenshotName)) {
                screenshotName = tmp + " (" + i + ").png";
                i++;
            }
		}

		public static Vector2 GetMainGameViewSize() {
			System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)Res;
        }
    }
}