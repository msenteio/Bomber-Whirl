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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace ElevenEleven
{
	[System.Serializable]
	public class PoolItemEvent : UnityEvent<PoolItem> { }

	public class PoolItem : MonoBehaviour {

		internal PoolItemEvent itemReleased = new PoolItemEvent();

		public void Release() {
			itemReleased.Invoke (this);
			OnRelease ();
			gameObject.SetActive (false);
		}

		protected virtual void OnRelease() {

		}
	}

	public class Pool<T> : Singleton<Pool<T>> where T : PoolItem {

		[SerializeField] T prefab;
        [SerializeField] int startCount = 0;

        private readonly Stack<T> m_Stack = new Stack<T>();
//		private readonly UnityEvent<T> m_ActionOnGet = new UnityEvent<T>();
//		private readonly UnityEvent<T> m_ActionOnRelease = new UnityEvent<T>();

        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Stack.Count; } }

        protected override void Awake() {
			base.Awake();

			for (int i = 0; i < startCount; i++) {
                T element = Instantiate<T>(prefab);
                element.transform.SetParent(transform);
                element.itemReleased.AddListener(Release);
                element.gameObject.SetActive(true);
                m_Stack.Push(element);
            }
        }

        public T Get()
        {
            T element;
            if (m_Stack.Count == 0)
            {
				element = Instantiate<T>(prefab);
				element.transform.SetParent (transform);
				element.itemReleased.AddListener(Release);
                countAll++;
            }
            else
            {
                element = m_Stack.Pop();
				element.gameObject.SetActive (true);
            }

//			m_ActionOnGet.Invoke(element);
            return element;
        }

		public void Release(PoolItem go)
        {
			T element = go as T;

            if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element)) 
                Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
//			m_ActionOnRelease.Invoke(element);
            m_Stack.Push(element);
        }
    }
}