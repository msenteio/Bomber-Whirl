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
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace ElevenEleven {
    public class EventHelper : MonoBehaviour {

        [SerializeField]
        protected bool active = true;
        [SerializeField]
        AudioSource audioSource;
        protected new AudioSource audio {
            get {
                if (audioSource == null) {
                    audioSource = base.GetComponent<AudioSource>();
                    if (audioSource == null) {
                        audioSource = gameObject.AddComponent<AudioSource>();
                    }
                }
                return audioSource;
            }
        }

        [SerializeField]
        UnityEvent genericAction;

		public void PlaySound(string soundName) {
			ShallowMusic.Play(soundName, BeatValue.Eighth);
		}

        public void SpawnObject(GameObject go) {
            if (active) {
                Instantiate(go);
            }
        }

        public void SpawnObjectAtLocalPosition(GameObject go) {
            if (active) {
                Instantiate(go, transform.position, Quaternion.identity);
            }
        }

        public void PlayAudioClip(AudioClip clip) {
            if (active) {
                audio.PlayOneShot(clip);
            }
        }

        //	public void PlayFMODClip(FMODAsset clip) {
        //		AudioController.PlayOneShot(clip, transform.position);
        //	}

        public void CallGenericAction() {
            if (active) {
                genericAction.Invoke();
            }
        }
    }
}