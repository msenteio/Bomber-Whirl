namespace ElevenEleven { 
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShallowAudioSource {

        public bool expired {
            get { return audioSource == null; }
        }

        AudioSource m_audioSource;
        AudioSource audioSource {
            get { return m_audioSource; }
            set { m_audioSource = value; }
        }

        AudioClip clip {
            get {
                if (AssertExpired()) {
                    return audioSource.clip;
                } else {
                    return null;
                }
            }
            set {
                if (AssertExpired()) {
                    audioSource.clip = value;
                }
            }
        }

        internal ShallowAudioSource(AudioSource audioSource) {
            this.audioSource = audioSource;
        }

        internal void Expire() {
            audioSource = null;
        }

        bool AssertExpired() {
            UnityEngine.Assertions.Assert.raiseExceptions = true;
			UnityEngine.Assertions.Assert.IsFalse(expired, "This ShallowAudioSource is no longer usable. Make sure to dispose of it.");
            return !expired;
        }

        public Vector2 position {
            get {
                if (AssertExpired()) {
                    return audioSource.transform.position;
                } else {
                    return Vector2.zero;
                }
            }
            set {
                if (AssertExpired()) {
                    audioSource.transform.position = new Vector3(value.x, value.y, Camera.main.transform.position.z);
                }
            }
        }

        public float volume {
            get {
                if (AssertExpired()) {
                    return audioSource.volume;
                } else {
                    return 0;
                }
            }
            set {
                if (AssertExpired()) {
                    audioSource.volume = value;
                }
            }
        }

        public bool loop {
            get {
                if (AssertExpired()) {
                    return audioSource.loop;
                } else {
                    return false;
                }
            }
            internal set {
                if (AssertExpired()) {
                    audioSource.loop = value;
                }
            }
        }

        public float spatialBlend {
            get {
                if (AssertExpired()) {
                    return audioSource.spatialBlend;
                } else {
                    return 0;
                }
            }
            set {
                if (AssertExpired()) {
                    audioSource.spatialBlend = value;
                }
            }
        }

        public float pitch {
            get {
                if (AssertExpired()) {
                    return audioSource.pitch;
                } else {
                    return 0;
                }
            }
            set {
                if (AssertExpired()) {
                    audioSource.pitch = value;
                }
            }
        }

        public bool looping {
            get {
                if (AssertExpired()) {
                    return audioSource.loop;
                } else {
                    return false;
                }
            }
        }

        internal void Start(BeatValue beat = ShallowMusic.DEFAULT_BEAT) {
            if (AssertExpired()) {
                if (!audioSource.isPlaying) {
                    audioSource.ShallowPlay(beat);
                }
            }
        }

        public void Stop() {
            if (AssertExpired()) {
                audioSource.Stop();
                Expire();
            }
        }

        public void Stop(BeatValue beat) {
            if (AssertExpired()) {
                audioSource.ShallowStop(beat);
                Expire();
            }
        }
    }
}