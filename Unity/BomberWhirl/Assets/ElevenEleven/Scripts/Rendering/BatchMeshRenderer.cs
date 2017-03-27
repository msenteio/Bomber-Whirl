namespace ElevenEleven.Rendering {
    using UnityEngine;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    //[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class BatchMeshRenderer : MonoBehaviour {

        [SerializeField]
        MeshManager m_meshManager;
        MeshManager Manager {
            get {
                return m_meshManager;
            }
            set {
                if (m_meshManager != value) {
                    RemoveFromManager();
                    m_meshManager = value;
                    AddToManager();
                }
            }
        }

        [SerializeField] Material m_sourceMaterial;
        public Material SourceMaterial {
            get { return m_sourceMaterial; }
            private set { m_sourceMaterial = value; }
        }

        Material m_currentMaterial;
        public Material CurrentMaterial {
            get {
                //if (m_currentMaterial == null) {
                //    CurrentMaterial = SourceMaterial;
                //}
                return m_currentMaterial;
            }
            set {
                if (m_currentMaterial != value) {
                    RemoveFromManager();
                    m_currentMaterial = value;
                    AddToManager();
                }
            }
        }

        [SerializeField] List<MeshDetail> m_meshDetails = new List<MeshDetail>();
        public ReadOnlyCollection<MeshDetail> MeshDetails {
            get { return m_meshDetails.AsReadOnly(); }
            //private set { m_meshDetails = value; }
        }

        MeshRenderer m_renderer;
        MeshRenderer Renderer {
            get {
                if (m_renderer == null) {
                    m_renderer = GetComponent<MeshRenderer>();
                }
                return m_renderer;
            }
        }

        MeshFilter m_filter;
        MeshFilter Filter {
            get {
                if (m_filter == null) {
                    m_filter = GetComponent<MeshFilter>();
                }
                return m_filter;
            }
        }

        public BatchMeshRenderer(Material material) {
            this.SourceMaterial = material;
        }

        public Color Color {
            get { return (CurrentMaterial ?? SourceMaterial).GetColor("_Color"); }
            set {
                if (Color != value) {
                    MaterialController.NewColor(this, value);
                    Renderer.sharedMaterial = CurrentMaterial;
                }
            }
        }

        public Color Emission {
            get { return (CurrentMaterial ?? SourceMaterial).GetColor("_EmissionColor"); }
            set {
                if (Emission != value) {
                    MaterialController.NewEmission(this, value);
                    Renderer.sharedMaterial = CurrentMaterial;
                }
            }
        }

        public Mesh Mesh {
            get { return Filter.mesh; }
            set {
                Filter.mesh = value;
                NotifyRebake();
            }
        }

        void Awake() {
            MaterialController.GetStandardMaterial(this);
        }

        void Start() {
            for (int i = 0; i < Filter.mesh.subMeshCount; i++) {
                m_meshDetails.Add(new MeshDetail(Filter, i));
            }

            if (Manager == null) {
                Manager = GetComponentInParent<MeshManager>();
                OnEnable();
            }
        }

        void OnEnable() {
            AddToManager();
        }

        void OnDisable() {
            RemoveFromManager();
        }

        void NotifyRebake() {
            if (Manager != null) {
                Manager.NotifyRebake();
            }
        }

        void AddToManager() {
            if (Manager != null) {
                Manager.AddMaterialContainer(this);
            }
        }

        void RemoveFromManager() {
            if (Manager != null) {
                Manager.RemoveMaterialContainer(this);
            }
        }

        internal void SetRendering(bool local) {
            Renderer.enabled = local;
        }
    }
}