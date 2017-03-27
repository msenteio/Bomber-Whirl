using UnityEngine;
using System.Collections;

namespace ElevenEleven {
    public class Line : MonoBehaviour {

        [SerializeField] float m_width;
        public float width {
            get { return m_width; }
            set { m_width = value; }
        }

        [SerializeField] Material m_material;
        Material material {
            get { return m_material; }
            set { m_material = value; }
        }

        void Start() {

        }
    }
}