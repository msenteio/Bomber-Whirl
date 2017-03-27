using UnityEngine;
using System.Collections;

namespace ElevenEleven {
    [System.Serializable]
    public class TransformData {

        public static readonly TransformData identity = new TransformData(Vector3.zero, Quaternion.identity, Vector3.one);

        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public bool global;

        TransformData(Vector3 position, Quaternion rotation, Vector3 scale, bool global = false) {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.global = global;
        }

        TransformData(Transform t, bool global = false) {
            this.global = global;

            if (global) {
                position = t.position;
                rotation = t.rotation;
                scale = t.localScale;
            } else {
                position = t.localPosition;
                rotation = t.rotation;
                scale = t.localScale;
            }
        }

        public void CopyTo(Transform t) {
            if (global) {
                t.position = position;
                t.rotation = rotation;
                t.localScale = scale;
            } else {
                t.localPosition = position;
                t.localRotation = rotation;
                t.localScale = scale;
            }
        }

        public static TransformData CopyFrom(Transform t, bool global = false) {
            return new TransformData(t, global);
        }

        public static void CopyTo(Transform t, TransformData data) {
            data.CopyTo(t);
        }

        public static TransformData Lerp(TransformData from, TransformData to, float dt) {
            return new TransformData(
                Vector3.Lerp(from.position, to.position, dt),
                Quaternion.Lerp(from.rotation, to.rotation, dt),
                Vector3.Lerp(from.scale, to.scale, dt),
                from.global && to.global);
        }
    }

    public static class TransformDataExtension {

        public static TransformData ToData(this Transform t, bool global = false) {
            return TransformData.CopyFrom(t, global);
        }

        public static void SetData(this Transform t, TransformData data) {
            data.CopyTo(t);
        }
    }
}