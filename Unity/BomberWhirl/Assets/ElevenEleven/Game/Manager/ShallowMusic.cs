namespace ElevenEleven {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using ElevenEleven.Game;

//    public enum SFXClips : int {
//		UI = 0,
//		Hit = 1,
//		HitLong = 2,
//		Drop = 3,
//		DropLong = 4,
//        Shoot = 5,
//        Whoosh = 6,
//    }

    public enum Notes : int {
        C = 0,
        CSharp = 1,
        DFlat = 1,
        D = 2,
        DSharp = 3,
        EFlat = 3,
        E = 4,
        F = 5,
        FSharp = 6,
        GFlat = 6,
        G = 7,
        GSharp = 8,
        AFlat = 8,
        A = 9,
        ASharp = 10,
        BFlat = 10,
        B = 11,
        HighC = 12,
    }

	[System.Serializable]
	internal class SFXClipsResource {
		public string name;
		public string resourceLocation;
	}

    [System.Serializable]
    internal abstract class AudioClipDetail {
        public List<AudioClip> clips;

        public AudioClip randomClip {
            get {
                return clips[Random.Range(0, clips.Count - 1)];
            }
        }
    }

    [System.Serializable]
    internal class SFXClipDetail : AudioClipDetail {
		public string clipName;
    }

    [System.Serializable]
    internal class NoteClipDetail : AudioClipDetail {

    }

    [System.Serializable]
    internal class MusicClipDetail {
        public int bpm;
        public AudioClip clip;
    }

	public class ShallowMusic : PrivateSingleton<ShallowMusic> {

		public const BeatValue DEFAULT_BEAT = BeatValue.Sixteenth;

		[SerializeField] List<MusicClipDetail> m_musicSoundTrack = new List<MusicClipDetail>();
		List<MusicClipDetail> musicSoundTrack {
			get { return m_musicSoundTrack; }
        }

		[SerializeField] List<SFXClipsResource> m_clipResources = new List<SFXClipsResource>();
		List<SFXClipsResource> clipResources {
			get {
				return m_clipResources;
			}
		}

//        [SerializeField]
        List<SFXClipDetail> m_sfxClipDetailList = new List<SFXClipDetail>();
        List<SFXClipDetail> sfxClipDetailList {
            get { return m_sfxClipDetailList; }
        }

        [SerializeField]
        List<NoteClipDetail> m_noteClipDetailList = new List<NoteClipDetail>();
        List<NoteClipDetail> noteClipDetailList {
            get { return m_noteClipDetailList; }
        }
        
		Dictionary<string, SFXClipDetail> m_sfxClips = new Dictionary<string, SFXClipDetail>();
		Dictionary<string, SFXClipDetail> sfxClips {
            get { return m_sfxClips; }
        }
        
        Dictionary<int, NoteClipDetail> m_notes = new Dictionary<int, NoteClipDetail>();
        Dictionary<int, NoteClipDetail> notes {
            get { return m_notes; }
        }

        List<AudioSource> m_activeSources = new List<AudioSource>();
		List<AudioSource> activeSources {
			get { return m_activeSources; }
		}

		List<AudioSource> m_inactiveSources = new List<AudioSource>();
		List<AudioSource> inactiveSources {
			get { return m_inactiveSources; }
		}

        Dictionary<AudioSource, ShallowAudioSource> m_shallowAudioSources = new Dictionary<AudioSource, ShallowAudioSource>();
        Dictionary<AudioSource, ShallowAudioSource> shallowAudioSources {
            get {
                return m_shallowAudioSources;
            }
        }

        protected override void Awake() {
			base.Awake();

			LoadResources();

            foreach (var item in sfxClipDetailList) {
				sfxClips.Add(item.clipName, item);
            }

            for (int i = 0; i < noteClipDetailList.Count; i++) {
                notes.Add(i, noteClipDetailList[i]);
            }
        }

		void LoadResources() {
			foreach (var clipResource in clipResources) {
				SFXClipDetail detail = new SFXClipDetail();
				detail.clipName = clipResource.name;

				List<AudioClip> clips = new List<AudioClip>(Resources.LoadAll<AudioClip>(clipResource.resourceLocation));
				clips.Sort(delegate	(AudioClip a, AudioClip b) {
					return string.CompareOrdinal(a.name, b.name);
				});

				detail.clips = clips;

				sfxClipDetailList.Add(detail);
			}
		}

		void Start() {
			StartCoroutine(PlayMusicSoundTrack());
#if UNITY_EDITOR
//			StartCoroutine(Tester());
#endif
		}

#if UNITY_EDITOR
//		IEnumerator Tester() {
//            //Tester2();
//
//            Notes[] notes = new Notes[] { Notes.C, Notes.E, Notes.G, Notes.HighC, Notes.G, Notes.E };
//            int index = 0;
//            
//			while (true) {
//				index++;
//
//
//				ShallowMusic.Play("Speech/Player", BeatValue.Quarter);
//				yield return new WaitForBeat(BeatValue.Quarter);
//
//				ShallowMusic.Play("Speech/1", BeatValue.Eighth);
//				yield return new WaitForBeat(BeatValue.Eighth);
//
//				ShallowMusic.Play("Speech/Joined", BeatValue.Eighth);
//				yield return new WaitForBeat(BeatValue.Quarter);
//				//				yield return new WaitForBeat(BeatValue.Quarter);
//
//
//				ShallowMusic.Play("Speech/Player", BeatValue.Quarter);
//				yield return new WaitForBeat(BeatValue.Quarter);
//
//				ShallowMusic.Play("Speech/1", BeatValue.Eighth);
//				yield return new WaitForBeat(BeatValue.Eighth);
//
//				ShallowMusic.Play("Speech/Left", BeatValue.Eighth);
//				yield return new WaitForBeat(BeatValue.Quarter);
//				//				yield return new WaitForBeat(BeatValue.Quarter);
//
//
//            }
//		}
//
//        int count;
//        void Tester2() {
//            Debug.Log(count++ % 4 + 1);
//            Clock.Instance.SyncFunction(Tester2, BeatValue.Measure);
//        }
#endif

		IEnumerator PlayMusicSoundTrack() {

			List<MusicClipDetail> music = new List<MusicClipDetail>(musicSoundTrack);
			Clock clock = gameObject.GetComponent<Clock>();
			AudioSource musicSource = GetAudioSource(false, 0.0f);
			musicSource.volume = 0.5f;

			AudioClip prevMusicClip = null;
			while (true) {
				music.Shuffle();

				// We never let the same song be played twice
				if (prevMusicClip == music[0].clip) {
					MusicClipDetail tmp = music[0];
					music[0] = music[1];
					music[1] = tmp;
				}

				foreach (var song in music) {
					clock.SetBPM(song.bpm);
					musicSource.clip = song.clip;
					
					musicSource.ShallowPlay(BeatValue.Measure);

					yield return new WaitForBeat(BeatValue.Measure);
					yield return new WaitForBeat(BeatValue.Measure);

					while (musicSource.isPlaying) {
						yield return null;
					}
				}

				prevMusicClip = music[music.Count - 1].clip;
			}
		}

		void LateUpdate() {
			for (int i = activeSources.Count - 1; i >= 0; i--) {
                if (!activeSources[i].isPlaying) {
                    AudioSource source = activeSources[i];
                    inactiveSources.Add(source);
                    activeSources.RemoveAt(i);

                    if (shallowAudioSources.ContainsKey(source)) {
                        if (!shallowAudioSources[source].expired) {
                            shallowAudioSources[source].Expire();
                        }
                        shallowAudioSources.Remove(source);
                    }
                }
			}
		}

		AudioSource GetAudioSource(bool addToActiveSources = true, float spatialBlend = 1.0f) {
			AudioSource source;

			if (inactiveSources.Count == 0) {
				GameObject go = new GameObject(addToActiveSources ? "Audio Source " + activeSources.Count : "Audio Source");
				source = go.AddComponent<AudioSource>();
//				source.gameObject.AddComponent<AudioReverbFilter>().reverbPreset = AudioReverbPreset.Psychotic;

                source.playOnAwake = false;
				source.spatialBlend = spatialBlend;

				source.transform.SetParent(transform);
			} else {
				source = inactiveSources[inactiveSources.Count - 1];
				inactiveSources.RemoveAt(inactiveSources.Count - 1);
			}

			if (addToActiveSources) {
				activeSources.Add(source);
			}

			return source;
		}
        
        ShallowAudioSource GetShallowAudioSource(AudioClip clip) {
            AudioSource source = GetAudioSource(true, 1.0f);
            source.clip = clip;
            ShallowAudioSource shallowSource = new ShallowAudioSource(source);

            if (!shallowAudioSources.ContainsKey(source)) {
                shallowAudioSources.Add(source, shallowSource);
            } else {
                shallowAudioSources[source] = shallowSource;
            }

            return shallowSource;
		}

		ShallowAudioSource MemberPlay(string clip, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
			return MemberPlay(clip, Vector2.zero, beat, volume);
		}

		ShallowAudioSource MemberPlay(string clip, int index, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
			return MemberPlay(clip, index, Vector2.zero, beat, volume);
		}

		ShallowAudioSource MemberPlay(string clip, Vector2 location, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
			return MemberPlay(sfxClips[clip].randomClip, location, false, beat, volume);
		}

		ShallowAudioSource MemberPlay(string clip, int index, Vector2 location, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
			return MemberPlay(sfxClips[clip].clips[index], location, false, beat, volume);
		}

        ShallowAudioSource MemberPlay(Notes note, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
            return MemberPlay(note, Vector2.zero, beat, volume);
        }

        ShallowAudioSource MemberPlay(Notes note, Vector2 location, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
            return MemberPlay(notes[(int)note].randomClip, location, false, beat, volume);
        }

        ShallowAudioSource MemberPlay(AudioClip clip, bool loop = false, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
            return MemberPlay(clip, Vector2.zero, loop, beat, volume);
		}

        ShallowAudioSource MemberPlay(AudioClip clip, Vector2 location, bool loop = false, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
			ShallowAudioSource source = GetShallowAudioSource(clip);
            source.loop = loop;
			source.position = location;
			source.volume = volume;
            source.Start(beat);

            return source;
		}

		void MemberPlay(AudioSource source, BeatValue beat = DEFAULT_BEAT) {
			source.PlayScheduled(Clock.Instance.AtNext(beat));
		}

        void MemberStop(AudioSource source, BeatValue beat = DEFAULT_BEAT) {
            source.SetScheduledEndTime(Clock.Instance.AtNext(beat));
		}

		public static ShallowAudioSource Play(string clip, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
			return Instance.MemberPlay(clip, beat, volume);
		}

		public static ShallowAudioSource Play(string clip, int index, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
			return Instance.MemberPlay(clip, index, beat, volume);
		}

		public static ShallowAudioSource Play(string clip, Vector2 location, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
			return Instance.MemberPlay(clip, location, beat, volume);
		}

		public static ShallowAudioSource Play(string clip, int index, Vector2 location, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
			return Instance.MemberPlay(clip, index, location, beat, volume);
		}

        public static ShallowAudioSource Play(Notes note, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
            return Instance.MemberPlay(note, beat, volume);
        }

        public static ShallowAudioSource Play(Notes note, Vector2 location, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
            return Instance.MemberPlay(note, location, beat, volume);
        }

        public static ShallowAudioSource Play(AudioClip clip, bool loop, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
            return Instance.MemberPlay(clip, loop, beat, volume);
		}

		public static ShallowAudioSource Play(AudioClip clip, Vector2 location, bool loop, BeatValue beat = DEFAULT_BEAT, float volume = 1.0f) {
			return Instance.MemberPlay(clip, location, loop, beat, volume);
        }

        public static void Play(AudioSource source, BeatValue beat = DEFAULT_BEAT) {
            Instance.MemberPlay(source, beat);
        }

        public static void Stop(AudioSource source, BeatValue beat = DEFAULT_BEAT) {
            Instance.MemberStop(source, beat);
        }
    }

	public static class ShallowMusicExtension {
        public static void ShallowPlay(this AudioSource source, BeatValue beat = ShallowMusic.DEFAULT_BEAT) {
            ShallowMusic.Play(source, beat);
        }

        public static void ShallowStop(this AudioSource source, BeatValue beat = ShallowMusic.DEFAULT_BEAT) {
            ShallowMusic.Stop(source, beat);
        }
    }	
}

