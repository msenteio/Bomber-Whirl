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
using System.Collections.Generic;

namespace ElevenEleven {
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    public class ItemsInBounds : MonoBehaviour {

        [SerializeField]
        List<GameObject> m_containedItems = new List<GameObject>();
        public List<GameObject> containedItems {
            get { return m_containedItems; }
        }

        [SerializeField]
        UnityEvent m_itemsUpdated;
        public UnityEvent itemsUpdated {
            get { return m_itemsUpdated; }
        }

        [SerializeField]
        GameObjectEvent m_itemsAdded;
        public GameObjectEvent itemsAdded {
            get { return m_itemsAdded; }
        }

        [SerializeField]
        GameObjectEvent m_itemsRemoved;
        public GameObjectEvent itemsRemoved {
            get { return m_itemsRemoved; }
        }

        //	[SerializeField] bool allowOnlyPlayer = false;

        void OnTriggerEnter2D(Collider2D other) {
            //		bool allow = (allowOnlyPlayer ? other.gameObject.GetComponent<Player>() != null : true);
            if (/*gameObject.layer == other.gameObject.layer && */!containedItems.Contains(other.gameObject)) {
                containedItems.Add(other.gameObject);
                itemsUpdated.Invoke();
                itemsAdded.Invoke(other.gameObject);
            }
        }

        void OnTriggerExit2D(Collider2D other) {
            if (containedItems.Contains(other.gameObject)) {
                containedItems.Remove(other.gameObject);
                itemsUpdated.Invoke();
                itemsRemoved.Invoke(other.gameObject);
            }
        }
    }
}