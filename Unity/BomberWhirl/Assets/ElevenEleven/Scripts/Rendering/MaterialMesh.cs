namespace ElevenEleven.Rendering {
    using UnityEngine;
    using System.Collections;

    [System.Serializable]
    class MaterialMesh {
        public Material material;
        public Mesh mesh;

        public MaterialMesh(Material material, Mesh mesh) {
            this.material = material;
            this.mesh = mesh;
        }
    }
}