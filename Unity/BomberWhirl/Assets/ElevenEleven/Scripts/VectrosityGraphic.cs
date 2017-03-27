/// <summary>
/// Copyright (c) 2016 11:11 Studios LLC
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this 
/// software and associated documentation files (the "Software"), to deal in the Software 
/// without restriction, including without limitation the rights to use, copy, modify, merge, 
/// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons 
/// to whom the Software is furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included 
/// in all copies or substantial portions of the Software.
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
/// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
/// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
/// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

namespace ElevenEleven {
    public class VectrosityGraphic : MonoBehaviour {

        [SerializeField]
        List<Vector3> points;
        public virtual List<Vector3> Points {
            get { return points; }
            set { points = value; }
        }

        [SerializeField]
        bool connect = true;

        [SerializeField]
        float width = 1.0f;
        public float Width {
            get { return width; }
            set {
                width = value;
                if (Line != null) {
                    Line.lineWidth = width;
                }
            }
        }

        [SerializeField]
        Color color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public virtual Color Color {
            get {
                return color;
            }
            set {
                color = value;
                if (Line != null) {
                    Line.color = color;
                }
            }
        }

        //[SerializeField]
        VectorLine line = null;
        public VectorLine Line {
            get { return line; }
            protected set { line = value; }
        }

        [SerializeField]
        LineType lineType = LineType.Continuous;
        [SerializeField]
        Joins joins = Joins.Weld;

        [SerializeField]
        Material material;
        public Material Material {
            get {
                if (line == null) {
                    return material;
                } else {
                    return line.material;
                }
            }
        }

        [SerializeField] bool m_draw3D = true;
        public bool draw3D {
            get { return m_draw3D; }
            set {
                m_draw3D = value;
                Draw();
            }
        }

        //    PolygonCollider2D m_polygonCollider;
        //    PolygonCollider2D polygonCollider
        //    {
        //        get
        //        {
        //            if (m_polygonCollider == null)
        //            {
        //                m_polygonCollider = GetComponent<PolygonCollider2D>();
        //            }
        //            return m_polygonCollider;
        //        }
        //    }

        void Start() {
            //        if (polygonCollider != null)
            //        {
            //            var colliderPoints = new List<Vector2>();
            //            foreach (Vector3 point in Points)
            //            {
            //                colliderPoints.Add(new Vector2(point.x, point.y));
            //            }
            //            polygonCollider.SetPath(0, colliderPoints.ToArray());
            //        }

            if (connect) {
                Points.Add(Points[0]);
            }

            Line = new VectorLine("Graphics " + name, Points, null, Width, lineType, joins);
            if (material != null) {
                Line.material = new Material(material);
            }
            Line.drawTransform = transform;
            Line.color = Color;

            //VectorLine.SetCanvasCamera(Camera.main);
            //Draw();
            //Line.rectTransform.SetParent(transform);
        }

        void OnDestroy() {
            if (Line != null) {
                VectorLine.Destroy(ref line);
            }
        }

        void Update() {
            Draw();
        }

        void Draw() {
            if (draw3D) {
                Line.Draw3D();
            } else {
                Line.Draw();
            }
        }

        void OnDrawGizmos() {
            var prevColor = Gizmos.color;
            Gizmos.color = Color;

            if (Points != null) {
                int increment = (lineType == LineType.Continuous) ? 1 : 2;
                for (int i = 0; i < (connect ? Points.Count : Points.Count - 1); i += increment) {
                    Vector3 first = transform.TransformPoint(Points[i]);
                    Vector3 second = transform.TransformPoint(Points[(i + 1) % Points.Count]);
                    Gizmos.DrawLine(first, second);
                }
            }

            Gizmos.color = prevColor;
        }
    }
}