namespace ElevenEleven.Rendering {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using ElevenEleven;

    [System.Serializable]
    public class MeshDetail {
        public MeshFilter meshFilter;
        public int subMeshIndex;

        public GameObject gameObject {
            get { return meshFilter.gameObject; }
        }

        public Mesh mesh {
            get { return meshFilter.mesh; }
        }

        public Matrix4x4 transform {
            get { return meshFilter.transform.localToWorldMatrix; }
        }

        public MeshDetail(MeshFilter meshFilter, int subMeshIndex) {
            this.meshFilter = meshFilter;
            this.subMeshIndex = subMeshIndex;
        }
    }
}