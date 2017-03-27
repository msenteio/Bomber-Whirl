namespace ElevenEleven {
    using UnityEngine;

    // System.Serializable is used to open Pair
    // up to Unity's inspector
    [System.Serializable]
    public class Pair<T, U> {

        public Pair() { }

        public Pair(T first, U second) {
            this.first = first;
            this.second = second;
        }

        [SerializeField]
        T m_first;
        public T first {
            get { return m_first; }
            set { m_first = value; }
        }

        [SerializeField]
        U m_second;
        public U second {
            get { return m_second; }
            set { m_second = value; }
        }
    }
}