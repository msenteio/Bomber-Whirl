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
/// 
using UnityEngine;
using System.Collections;

namespace ElevenEleven {
    public class WASDPhysics2DMovement : MonoBehaviour {

        [SerializeField]
        float speed = 1.0f;

        [SerializeField]
        bool faceMove = false;
        [SerializeField]
        float turnSpeed = 10.0f;
        float targetAngle = 0.0f;

        Rigidbody2D m_rigidbody2D;
        new protected Rigidbody2D rigidbody2D {
            get {
                if (m_rigidbody2D == null) {
                    m_rigidbody2D = GetComponent<Rigidbody2D>();
                }
                return m_rigidbody2D;
            }
        }

        protected virtual void OnDisable() {
            rigidbody2D.velocity = Vector2.zero;
        }

        protected virtual void FixedUpdate() {
            Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (axis.magnitude < 1.0f) {
                axis = axis.normalized * axis.magnitude;
            } else {
                axis.Normalize();
            }
            rigidbody2D.velocity = speed * axis;


            if (faceMove) {
                if (rigidbody2D.velocity != Vector2.zero) {
                    targetAngle = Mathf.Rad2Deg * Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x);
                }

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.AngleAxis(targetAngle, Vector3.forward),
                    turnSpeed * Time.fixedDeltaTime);
            }
        }
    }
}