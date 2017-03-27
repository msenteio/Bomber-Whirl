namespace ElevenEleven.Game {
    using UnityEngine;
    using System.Collections;

    public enum LoadType {
		None = -1,
        LoadDefaults = 0,
        FreshStart = 1
    }

    public class LoadDefaults : MonoBehaviour {

        [SerializeField] LoadType loadType = LoadType.FreshStart;

        void Awake() {
            if (loadType == LoadType.FreshStart) {
                Main.FreshStart();
			} else if (loadType == LoadType.LoadDefaults) {
                Main.LoadDefaults();
            }
        }
    }
}