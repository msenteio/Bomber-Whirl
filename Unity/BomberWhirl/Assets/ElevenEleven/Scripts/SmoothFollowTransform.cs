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

namespace ElevenEleven {
    public class SmoothFollowTransform : MonoBehaviour {

        [SerializeField]
        protected Component target;
        [SerializeField]
        float smoothAmt = 5.0f;
        [SerializeField]
        float maxDistBeforeReset = 10.0f;
        protected float MaxDistBeforeReset {
            get { return maxDistBeforeReset; }
            set { maxDistBeforeReset = value; }
        }

        [SerializeField]
        Vector3 offset = new Vector3(0, 0, 0);
        public Vector3 Offset {
            get { return offset; }
            set { offset = value; }
        }

        [SerializeField]
        bool follow = true;
        public bool Follow {
            get { return follow; }
            set { follow = value; }
        }

        private Vector2 velocity;

        protected virtual void OnPreRender() {

            if (Follow && target != null) {

                Vector3 position = transform.position;
                Vector2 endPosition = target.transform.position + offset;

                float dist = Vector2.Distance(position, endPosition);
                if (dist < maxDistBeforeReset) {
                    position = Vector2.Lerp((Vector2)position, endPosition, smoothAmt * Time.fixedDeltaTime).ToVector3(position.z);
                } else {
                    Vector2 separation = endPosition - (Vector2)position;
                    position = (endPosition - maxDistBeforeReset * separation.normalized).ToVector3(position.z);
                }

                transform.position = position;
            }
        }
    }
}