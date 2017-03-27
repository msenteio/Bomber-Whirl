namespace ElevenEleven.Rendering {

    using UnityEngine;
    using System.Collections.Generic;
    using ElevenEleven;

    using ClonedMaterials = System.Collections.Generic.Dictionary<UnityEngine.Material, 
        System.Collections.Generic.Dictionary<MaterialComparer, UnityEngine.Material>>;

    public class MeshManager : MonoBehaviour {

        // This could theoretically be 65536 but doesn't seem worth the future risk. We want a nice buffer.
        // For example: It would cause very strange edge cases if for whatever reason Unity only supports
        // 65520 becuase it uses the extra bytes for something new.
        const int MAX_MESH_VERTICES = 65000;

        [SerializeField]
        bool m_allowBaking = true;
        public bool AllowBaking {
            get { return m_allowBaking; }
            set { m_allowBaking = value; }
        }

        // Used to group up MaterialContainers that use the same CurrentMaterial. This is helpful for merging geometry
        Dictionary<Material, HashSet<BatchMeshRenderer>> m_cloneContainer = new Dictionary<Material, HashSet<BatchMeshRenderer>>();
        public Dictionary<Material, HashSet<BatchMeshRenderer>> CloneContainer {
            get { return m_cloneContainer; }
        }

        bool m_cloneContainerChanged = false;
        bool BakeNeeded {
            get { return m_cloneContainerChanged; }
            set { m_cloneContainerChanged = value; }
        }

        List<GameObject> m_combinedObjects = new List<GameObject>();
        GameObject GetNewCombinedObject() {
            GameObject m_combinedObject = new GameObject("Combined Object", typeof(MeshFilter), typeof(MeshRenderer));
            m_combinedObject.transform.SetParent(transform);
            m_combinedObjects.Add(m_combinedObject);
            return m_combinedObject;
        }

        protected virtual void OnDestroy() {
            CloneContainer.Clear();
            DestroyBakedObjects();
        }

        void DestroyBakedObjects() {
            UnityEngine.Profiling.Profiler.BeginSample("Destroy Baked Objects", this);

            for (int i = 0; i < m_combinedObjects.Count; i++) {
                Destroy(m_combinedObjects[i].GetComponent<MeshFilter>().sharedMesh);
                Destroy(m_combinedObjects[i]);
            }
            m_combinedObjects.Clear();

            UnityEngine.Profiling.Profiler.EndSample();
        }

        void LateUpdate() {
            if (BakeNeeded && AllowBaking) {
                ClearAndBake();
            }
        }

        public bool RemoveMaterialContainer(BatchMeshRenderer materialContainer) {
            if (materialContainer.CurrentMaterial != null && CloneContainer.ContainsKey(materialContainer.CurrentMaterial)) {
                if (CloneContainer[materialContainer.CurrentMaterial].Remove(materialContainer)) {
                    BakeNeeded = true;
                    return true;
                }
            }

            return false;
        }

        public void NotifyRebake() {
            BakeNeeded = true;
        }

        public bool AddMaterialContainer(BatchMeshRenderer materialContainer) {
            if (materialContainer.CurrentMaterial != null) {
                if (!CloneContainer.ContainsKey(materialContainer.CurrentMaterial)) {
                    CloneContainer.Add(materialContainer.CurrentMaterial, new HashSet<BatchMeshRenderer>());
                }

                if (!CloneContainer[materialContainer.CurrentMaterial].Contains(materialContainer)) {
                    CloneContainer[materialContainer.CurrentMaterial].Add(materialContainer);
                    BakeNeeded = true;

                    return true;
                }
            }

            return false;
        }

        public void ClearAndBake() {
            UnityEngine.Profiling.Profiler.BeginSample("ClearAndBake");

            BakeNeeded = false;
            DestroyBakedObjects();
            Bake();

            UnityEngine.Profiling.Profiler.EndSample();
        }

        public void Bake() {
            UnityEngine.Profiling.Profiler.BeginSample("Bake");

            //Lists that holds mesh data that belongs to each submesh
            List<MaterialMesh> materialMeshes = new List<MaterialMesh>();

            // First let's create our lists of CombineInstances based on the used Material
            foreach (var item in MaterialController.SourceToClone.Values) {
                foreach (Material material in item.Values) {
                    if (!CloneContainer.ContainsKey(material) || CloneContainer[material].Count == 0) {
                        // We continue on if this material isn't cloned anywhere
                        continue;
                    }

                    List<CombineInstance> combineInstances = new List<CombineInstance>();
                    int vertexCount = 0;
                    foreach (BatchMeshRenderer container in CloneContainer[material]) {
                        container.SetRendering(false);

                        foreach (MeshDetail meshDetail in container.MeshDetails) {
                            CombineInstance combine = new CombineInstance();
                            combine.mesh = meshDetail.mesh;
                            combine.subMeshIndex = meshDetail.subMeshIndex;
                            combine.transform = meshDetail.transform;
                            
                            if (vertexCount + combine.mesh.vertexCount > MAX_MESH_VERTICES) {
                                Mesh tmpMesh = new Mesh();
                                tmpMesh.CombineMeshes(combineInstances.ToArray(), true, true);
                                materialMeshes.Add(new MaterialMesh(material, tmpMesh));

                                combineInstances.Clear();
                                vertexCount = 0;
                            }

                            vertexCount += combine.mesh.vertexCount;
                            combineInstances.Add(combine);
                        }
                    }

                    Mesh mesh = new Mesh();
                    mesh.CombineMeshes(combineInstances.ToArray(), true, true);
                    materialMeshes.Add(new MaterialMesh(material, mesh));
                }
            }

            // Now let's combine all of these instances into one giant mesh
            {
                List<Material> materials = new List<Material>();
                List<CombineInstance> combineInstances = new List<CombineInstance>();
                int vertexCount = 0;

                for (int i = 0; i < materialMeshes.Count; i++) {
                    if (vertexCount + materialMeshes[i].mesh.vertexCount > MAX_MESH_VERTICES) {
                        Mesh tmpMesh = new Mesh();
                        tmpMesh.CombineMeshes(combineInstances.ToArray(), false, true);
                        for (int j = 0; j < combineInstances.Count; j++) {
                            Destroy(combineInstances[j].mesh);
                        }

                        GameObject tmp = GetNewCombinedObject();
                        tmp.GetComponent<MeshFilter>().mesh = tmpMesh;
                        tmp.GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();

                        combineInstances.Clear();
                        materials.Clear();
                        vertexCount = 0;
                    }

                    materials.Add(materialMeshes[i].material);

                    CombineInstance combine = new CombineInstance();
                    combine.mesh = materialMeshes[i].mesh;
                    combine.transform = Matrix4x4.identity;// transform.localToWorldMatrix;
                    combineInstances.Add(combine);

                    vertexCount += combine.mesh.vertexCount;
                }

                //Create the final combined mesh
                Mesh combinedMesh = new Mesh();
                combinedMesh.CombineMeshes(combineInstances.ToArray(), false, true);
                for (int i = 0; i < combineInstances.Count; i++) {
                    Destroy(combineInstances[i].mesh);
                }

                GameObject combinedObj = GetNewCombinedObject();
                combinedObj.GetComponent<MeshFilter>().mesh = combinedMesh;
                combinedObj.GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();
            }

            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}