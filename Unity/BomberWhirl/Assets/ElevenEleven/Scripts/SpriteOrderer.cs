﻿/// <summary>
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

namespace ElevenEleven {
    [ExecuteInEditMode]
    public class SpriteOrderer : MonoBehaviour {

        const int MULTIPLIER = 100;

        [SerializeField]
        float yOffset;
        [SerializeField]
        int layerOffset = 0;
        [SerializeField]
        bool updateWhilePlaying = false;
        SpriteRenderer spriteRenderer;

        void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            SetSortingLayer();
        }

        void Update() {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
                SetSortingLayer();
            }
#endif

            if (updateWhilePlaying) {
                SetSortingLayer();
            }
        }

        void SetSortingLayer() {
            spriteRenderer.sortingOrder = (int)(-MULTIPLIER * (transform.position.y + yOffset)) + (int)transform.position.x + layerOffset;
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;

            var dividingPos = transform.position + new Vector3(0, yOffset);

            //Gizmos.DrawSphere(dividingPos, 0.25f);

            UnityEditor.Handles.color = Color.red;
            float val = UnityEditor.HandleUtility.GetHandleSize(dividingPos);
            UnityEditor.Handles.Label(dividingPos + new Vector3(-0.5f * val, val / 4), "Player Behind");
            UnityEditor.Handles.DrawLine(dividingPos - new Vector3(2 * val, 0), dividingPos + new Vector3(2 * val, 0));
            UnityEditor.Handles.Label(dividingPos + new Vector3(-0.5f * val, -val / 32), "Player In Front");
        }
#endif
    }
}