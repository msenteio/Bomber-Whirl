namespace ElevenEleven.Rendering {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using ElevenEleven;
    
    public class MaterialComparer {
        public Color color;
        public Color emission;

        public MaterialComparer(Material material) {
            this.color = material.GetColor("_Color");
            this.emission = material.GetColor("_EmissionColor");
        }

        public MaterialComparer(Color color, Color emission) {
            this.color = color;
            this.emission = emission;
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 17;
                hash = hash * 31 + color.GetHashCode();
                hash = hash * 31 + emission.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj) {
            MaterialComparer mp = obj as MaterialComparer;
            if (mp != null) {
                return color == mp.color && emission == mp.emission;
            } else {
                return false;
            }
        }
    }
}