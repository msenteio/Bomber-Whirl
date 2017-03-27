#if UNITY_EDITOR
namespace ElevenEleven {
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.IO;
    using Microsoft.CSharp;
    using ElevenEleven.Game;

    public class ShallowGamesWindow : EditorWindow {

        static bool m_busy = false;
        public static bool Busy {
            get { return m_busy; }
            private set {
                m_busy = value;
                EditorListener.Paused = value;
            }
        }

        const BuildAssetBundleOptions bundleOptions = BuildAssetBundleOptions.StrictMode;
        readonly BuildTarget[] targets = new BuildTarget[] {
            BuildTarget.StandaloneWindows,
			//BuildTarget.StandaloneOSXUniversal,
			//BuildTarget.StandaloneLinux,
		};

        const string infoText =
            "After your project is complete and fully tested, you will use this tool to create the Asset Bundles required to " +
            "export for each particular platform that Shallow Games support. These Asset Bundles include the dependencies of each " +
            "of your scenes that are enabled in the Build Settings.";

        static string bundleName;
        static string bundleVariant;

        static bool shallowBuild {
            get {
                return PlayerSettings.productName == "Shallow Games";
            }
        }

        [MenuItem("11:11 Studios/Shallow Games")]
        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow<ShallowGamesWindow>("Shallow Games", true);
        }

        void OnDestroy() {
            Busy = false;
        }
        
        void OnGUI() {
            GUILayout.Label("Create Asset Bundle");
            GUILayout.TextArea(infoText);
            GUILayout.Space(8);

            if (string.IsNullOrEmpty(bundleName)) {
                bundleName = PlayerSettings.productName;
            }
            if (string.IsNullOrEmpty(bundleVariant)) {
                bundleVariant = PlayerSettings.bundleVersion;
            }

            bundleName = EditorGUILayout.TextField("Bundle Name", bundleName);
            bundleVariant = EditorGUILayout.TextField("Bundle Variant", bundleVariant);

            //if (GUILayout.Button("Asset Bundle")) {
            //    BuildAllAssetBundles();
            //}

            if (GUILayout.Button("Unity Package")) {
                CreateUnityPackage();
            }

            if (shallowBuild) {
                if (GUILayout.Button("Create Config")) {
                    ElevenConfig.GetEditorProjectConfig("Shallow Games", "Assets/ElevenEleven/Resources");
                }

                if (GUILayout.Button("Import All Game Configs")) {
                    ElevenConfig.UpdateProjectFromGames();
                }

				bool partyBuild = false;
				bool newValue = GUILayout.Toggle(partyBuild, "Party Build?");
				if (partyBuild != newValue) {
					if (newValue) {

					} else {

					}
				}
            }
        }

        bool TrySetup(out string path) {

            path = EditorUtility.SaveFilePanel("Save Package to Directory", EditorPrefs.GetString("ShallowGames.UnityPackageDirectory", ""), bundleName, "unitypackage");

            if (path.Length != 0) {
                path = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                EditorPrefs.SetString("ShallowGames.UnityPackageDirectory", path.Substring(0, path.LastIndexOf(Path.AltDirectorySeparatorChar)));

                Busy = true;

                return true;
            } else {
                return false;
            }

            //if (!AssetDatabase.IsValidFolder("Assets/ElevenAssetBundles")) {
            //    AssetDatabase.CreateFolder("Assets", "ElevenAssetBundles");
            //}

            //ElevenConfig.GetEditorProjectConfig(bundleName);
        }

        void Finish() {
            Busy = false;
            //ElevenConfig.DeleteProjectConfig(bundleName);
        }

        List<string> GetAllSceneAssets() {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            List<string> assetNames = new List<string>();
            for (int i = 0; i < scenes.Length; i++) {
                if (scenes[i].enabled) {
                    assetNames.Add(scenes[i].path);
                }
            }

            // Get all scene dependencies
            assetNames = new List<string>(AssetDatabase.GetDependencies(assetNames.ToArray(), true));
            for (int i = assetNames.Count - 1; i >= 0; i--) {
                string asset = assetNames[i];

                bool isShallowItem = asset.Contains("Plugin") || asset.Contains("Standard Assets") || asset.Contains("InControl") || asset.Contains("Vectrosity") || asset.Contains("ElevenEleven");

                if ((shallowBuild ? !isShallowItem : isShallowItem)) {
                    assetNames.RemoveAt(i);
                }
            }

            return assetNames;
        }

        bool IsShallowItem(string asset) {
            return asset.Contains("Plugin") || asset.Contains("Standard Assets") || asset.Contains("InControl") || asset.Contains("Vectrosity") || asset.Contains("ElevenEleven");
        }

        List<string> GetAllAssetPathsInFolderName(string folderName) {
            List<string> resourcePaths = new List<string>();
            GetAllAssetPathsInFolderName(folderName, Application.dataPath, false, ref resourcePaths);

            // strip out absolute path to Assets path instead
            for (int i = 0; i < resourcePaths.Count; i++) {
                resourcePaths[i] = resourcePaths[i].Substring(resourcePaths[i].IndexOf("Assets"));
            }

            return resourcePaths;
        }

        void GetAllAssetPathsInFolderName(string folderName, string path, bool isResourcesPath, ref List<string> resourceFiles) {

            if (!isResourcesPath && new DirectoryInfo(path).Name.Equals(folderName, System.StringComparison.InvariantCultureIgnoreCase)) {
                isResourcesPath = true;
            }

            if (isResourcesPath) {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files) {
                    if (!file.EndsWith(".meta", System.StringComparison.InvariantCultureIgnoreCase)) {
                        resourceFiles.Add(file);
                    }
                }
            }

            string[] paths = Directory.GetDirectories(path);
            foreach (string childPath in paths) {
                GetAllAssetPathsInFolderName(folderName, childPath, isResourcesPath, ref resourceFiles);
            }
        }

        List<string> GetAllAssetsInFolderName(string folderName) {
            List<string> assetNames = GetAllAssetPathsInFolderName(folderName);

            string test = AssetDatabase.AssetPathToGUID(assetNames[0]);

            // Get all scene dependencies
            assetNames = new List<string>(AssetDatabase.GetDependencies(assetNames.ToArray()));
            for (int i = assetNames.Count - 1; i >= 0; i--) {
                string asset = assetNames[i];

                if ((shallowBuild ? !IsShallowItem(asset) : IsShallowItem(asset))) {
                    assetNames.RemoveAt(i);
                }
            }

            return assetNames;
        }

        List<string> GetAllAssets() {
            HashSet<string> assetNames = new HashSet<string>();
            assetNames.UnionWith(GetAllSceneAssets());
            assetNames.UnionWith(GetAllAssetsInFolderName("Resources"));
            assetNames.UnionWith(GetAllAssetsInFolderName("Plugins"));
            assetNames.UnionWith(GetAllSourceFiles());

            return new List<string>(assetNames);
        }

        List<string> GetAllSourceFiles() {
            string[] allAssets = AssetDatabase.GetAllAssetPaths();
            List<string> sourceFiles = new List<string>();

            string dataPathParent = Directory.GetParent(Application.dataPath).FullName;
            foreach (string asset in allAssets) {
                if (IsShallowItem(asset) ? shallowBuild : !shallowBuild) {
                    string lowerPath = asset.ToLowerInvariant();

                    if (lowerPath.EndsWith(".cs") || lowerPath.EndsWith(".shader") || lowerPath.EndsWith(".cginc")) {
						sourceFiles.Add(asset);//dataPathParent + "\\" + asset.Replace('/', Path.DirectorySeparatorChar));
                    }
                }
            }
            return sourceFiles;
        }

        void BuildAllAssetBundles() {
            string path;
            if (!TrySetup(out path)) {
                return;
            }

            List<string> assets = GetAllAssets();

            AssetBundleBuild scenesBundles = new AssetBundleBuild();
            scenesBundles.assetNames = assets.ToArray();
            scenesBundles.assetBundleName = bundleName;
            scenesBundles.assetBundleVariant = bundleVariant;

            if (string.IsNullOrEmpty(bundleName)) {
                bundleName = PlayerSettings.productName;
            }
            if (string.IsNullOrEmpty(bundleVariant)) {
                bundleVariant = PlayerSettings.productName;
            }

            CompileAssembly(GetAllSourceFiles().ToArray());
            for (int i = 0; i < targets.Length; i++) {
                BuildPipeline.BuildAssetBundles(path, bundleOptions, targets[i]);
            }

            Finish();
        }

        void CreateUnityPackage() {
            string path;
            if (!TrySetup(out path)) {
                return;
            }

            if (string.IsNullOrEmpty(bundleName)) {
                bundleName = PlayerSettings.productName;
            }
            if (string.IsNullOrEmpty(bundleVariant)) {
                bundleVariant = PlayerSettings.productName;
            }

            AssetDatabase.ExportPackage(GetAllAssets().ToArray(), path,
                ExportPackageOptions.Default | ExportPackageOptions.Interactive);

            Finish();
        }

        void CompileAssembly(string[] sourceFiles) {
            CompilerParameters compilerParameters = new CompilerParameters {
                GenerateExecutable = false,
                GenerateInMemory = false,
                IncludeDebugInformation = true,
                CompilerOptions = "/target:library /optimize /warn:0"
            };

            //            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.OutputAssembly = "Assets/ElevenAssetBundles";
            compilerParameters.ReferencedAssemblies.Add(
                (
                    EditorApplication.applicationContentsPath +
                        "/Managed/UnityEngine.dll"
                ).Replace('/', Path.DirectorySeparatorChar)
            );

            //			var options = new Dictionary<string,string> {
            //				{ "CompilerVersion", "v2.0" }
            //			};
            //			CodeDomProvider codeProvider = new CSharpCodeProvider(options);
            ////            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
            //            CompilerResults compilerResults = codeProvider.CompileAssemblyFromFile(compilerParameters, sourceFiles);
            //
            //            Debug.Log(compilerResults.PathToAssembly);
            //            if (compilerResults.Errors.Count > 0) {
            //                foreach (CompilerError error in compilerResults.Errors) {
            //                    Debug.LogError(error.ToString());
            //                    Debug.Log(error.FileName);
            //                }
            //            }

            var options = new Dictionary<string, string> {
                { "CompilerVersion", "v2.0" }
            };

            var codeProvider = new CSharpCodeProvider(options);

            var compilerResults = codeProvider.CompileAssemblyFromFile(compilerParameters, sourceFiles);

            Debug.Log(compilerResults.PathToAssembly);
            if (compilerResults.Errors.Count > 0) {
                foreach (CompilerError error in compilerResults.Errors) {
                    Debug.LogError(error.ToString());
                    Debug.Log(error.FileName);
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
#endif