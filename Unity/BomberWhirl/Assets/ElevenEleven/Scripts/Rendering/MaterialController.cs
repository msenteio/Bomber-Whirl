namespace ElevenEleven.Rendering {

    using UnityEngine;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using ElevenEleven;

    using ClonedMaterials = System.Collections.Generic.Dictionary<UnityEngine.Material, 
        System.Collections.Generic.Dictionary<MaterialComparer, UnityEngine.Material>>;
    
    public class MaterialController : Singleton<MaterialController> {
        
        static ClonedMaterials sourceToClone = new ClonedMaterials();
        internal static ClonedMaterials SourceToClone {
            get { return sourceToClone; }
            private set { sourceToClone = value; }
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            foreach (var item in SourceToClone) {
                Material original = item.Key;
                Dictionary<MaterialComparer, Material> clones = item.Value;
                foreach (Material material in clones.Values) {
                    if (original != material) {
                        // We only destroy clones. If the key is equal then that means
                        // this is an original resource and can not be destroyed
                        Destroy(material);
                    }
                }
            }
            SourceToClone.Clear();
        }

        public static int MaterialCount {
            get {
                int count = 0;
                foreach (var item in SourceToClone.Values) {
                    count += item.Count;
                }
                return count;
            }
        }

        public static void NewColor(BatchMeshRenderer source, Color color) {
            GetStandardMaterial(source, color, source.Emission);
        }

        public static void NewEmission(BatchMeshRenderer source, Color emission) {
            GetStandardMaterial(source, source.Color, emission);
        }

        public static void GetStandardMaterial(BatchMeshRenderer source) {
            GetStandardMaterial(source, source.Color, source.Emission);
        }

        public static void GetStandardMaterial(BatchMeshRenderer source, Color color) {
            GetStandardMaterial(source, color, source.Emission);
        }

        public static void GetStandardMaterial(BatchMeshRenderer source, Color color, Color emission) {

            UnityEngine.Profiling.Profiler.BeginSample("GetStandardMaterial");

            if (!SourceToClone.ContainsKey(source.SourceMaterial)) {
                // We need to assign this source if it doesn't exist.
                SourceToClone.Add(source.SourceMaterial, new Dictionary<MaterialComparer, Material>());

                // We also add the source as a "clone" of itself. That way the original source is still used
                MaterialComparer sourceMaterialComparer = new MaterialComparer(source.SourceMaterial);
                SourceToClone[source.SourceMaterial].Add(sourceMaterialComparer, source.SourceMaterial);
            }

            Dictionary<MaterialComparer, Material> clones = SourceToClone[source.SourceMaterial];
            MaterialComparer currentMaterialComparer = new MaterialComparer(source.Color, source.Emission);
            MaterialComparer newMaterialComparer = new MaterialComparer(color, emission);

            if (currentMaterialComparer != newMaterialComparer) {
                if (!clones.ContainsKey(newMaterialComparer)) {
                    Material clone = new Material(source.SourceMaterial);
                    clone.name = source.SourceMaterial.name + " " + newMaterialComparer.color.ToString();
                    clone.SetColor("_Color", newMaterialComparer.color);
                    clone.SetColor("_EmissionColor", newMaterialComparer.emission);
                    clones.Add(newMaterialComparer, clone);
                }

                source.CurrentMaterial = clones[newMaterialComparer];
            }
            
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}