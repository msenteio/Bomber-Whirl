namespace ElevenEleven.Game {
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections;
	using System.Collections.Generic;
	using InControl;

    [System.Serializable]
    public class PlayerInputEvent : UnityEvent<PlayerInput> { }

    public class PlayerManager : Singleton<PlayerManager> {
        
		static Dictionary<InputDevice, int> deviceOrder = new Dictionary<InputDevice, int>();

        static Dictionary<int, PlayerInput> m_players = new Dictionary<int, PlayerInput>();
        internal static Dictionary<int, PlayerInput> players {
            get { return m_players; }
        }

        static HashSet<int> m_bots = new HashSet<int>();
        static HashSet<int> bots {
            get { return m_bots; }
        }

        static PlayerInput m_defaultHuman;
        public static PlayerInput DefaultHuman {
            get {
				if (m_defaultHuman == null && InputManager.ActiveDevice != InputDevice.Null) {
                    m_defaultHuman = new PlayerInput(0, InputManager.ActiveDevice);
                }
                return m_defaultHuman;
            }
        }

        public static PlayerInput GetHuman(int index) {
            return players[index];
        }

		public static bool TryGetHuman(int index, out PlayerInput input) {
			if (players.ContainsKey(index)) {
				input = players[index];
				return true;
			} 
//			else if (index == 0 && DefaultHuman != null) {
//				input = DefaultHuman;
//				return true;
//			}
			else {
				input = null;
				return false;
			}
        }

        public static PlayerInput CreateHuman(int index) {
			if (index < InputManager.Devices.Count) {
            	return new PlayerInput(index, InputManager.Devices[index]);
			} else {
				Debug.LogWarningFormat("Index [%0] is not available.", index);
				return null;
			}
        }

		public static bool IsBot(int index) {
            return bots.Contains(index);
		}

        public static bool IsHuman(int index) {
            return players.ContainsKey(index);
        }

		public static PlayerInputEvent inputAdded = new PlayerInputEvent();
		public static PlayerInputEvent inputRemoved = new PlayerInputEvent();

		public static int PlayerCount {
			get { return players.Count + bots.Count; }
		}

        [SerializeField] Color[] m_playerColors;
        Color[] playerColors {
            get {
                return m_playerColors;
            }
        }

        public Color GetColor(int index) {
            return playerColors[index];
        }

		internal static void Clear() {
			players.Clear();
            bots.Clear();
        }

        internal static bool Contains(InputDevice device) {
            int id;
            return Contains(device, out id);
        }

        internal static bool Contains(int playerID) {
            return players.ContainsKey(playerID);
        }

        internal static bool Contains(InputDevice device, out int playerID) {
            foreach (var item in players.Values) {
                if (item.device == device) {
                    playerID = item.InputID;
                    return true;
                }
            }
            playerID = -1;
            return false;
        }

        internal static void AddHuman(InputDevice device) {
			int desiredID = -1;
	
			if (deviceOrder.ContainsKey(device) && !players.ContainsKey(deviceOrder[device])) {
				desiredID = deviceOrder[device];
			}

			if (desiredID < 0) {
	            for (int i = 0; i < 4; i++) {
	                if (!players.ContainsKey(i)) {
						desiredID = i;
	                    break;
	                }
	            }
			}

			PlayerInput input = new PlayerInput(desiredID, device);
			players.Add(desiredID, input);
			if (deviceOrder.ContainsKey(device)) {
				deviceOrder[device] = desiredID;
			} else {
				deviceOrder.Add(device, desiredID);
			}

			inputAdded.Invoke(input);
        }

        internal static void RemoveHuman(InputDevice device) {
            int playerID;
            if (Contains(device, out playerID)) {
                PlayerInput input = players[playerID];
                players.Remove(playerID);
                inputRemoved.Invoke(input);
            }
        }

		internal static void AddBot() {

		}

        //void Update() {
        //    InputManager.Devices
        //}


	}
}