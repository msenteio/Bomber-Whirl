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
using UnityEngine.Events;
using System.Collections;

namespace ElevenEleven {
    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision> { }

    public abstract class OnCollisionEvent : MonoBehaviour {

        [SerializeField]
        UnityEvent collisionTriggered;
        public UnityEvent CollisionTriggered {
            get { return collisionTriggered; }
            private set { collisionTriggered = value; }
        }

        [SerializeField]
        CollisionEvent detailedCollisionTriggered;
        public CollisionEvent DetailedCollisionTriggered {
            get { return detailedCollisionTriggered; }
            private set { detailedCollisionTriggered = value; }
        }

        // We want the checkbox to enable/disable available
        protected virtual void Start() { }
    }
}