namespace ElevenEleven.Game {
	using ElevenEleven;
	using UnityEngine;
	using System.Collections;
    using InControl;
    
    public sealed class PlayerInput {

        int m_inputID;
        public int InputID {
            get { return m_inputID; }
            private set { m_inputID = value; }
        }

		Color m_color;
		public Color color {
			get { return m_color; }
			private set {
				m_color = value;
				device.SetLightColor(color);
			}
		}

		public void SetLightFlash(float flashOnDuration, float flashOffDuration) {
			device.SetLightFlash(flashOnDuration, flashOffDuration);
		}

		public void Vibrate(float intensity) {
			if (intensity == 0.0f) {
				StopVibration();
			} else {
				device.Vibrate(intensity);
			}
		}

		public void StopVibration() {
			device.StopVibration();
		}

        private PlayerActions actions;

        internal InputDevice device {
            get { return actions.Device; }
        }

        internal PlayerInput(int inputID, InputDevice device) {
            this.InputID = inputID;

            this.actions = new PlayerActions();
            this.actions.Device = device;

			color = PlayerManager.Instance.GetColor(inputID);
        }

        #region First Action
        public bool FirstActionIsPressed {
            get { return actions.PrimaryAction.IsPressed; }
        }

        public bool FirstActionWasPressed {
            get { return actions.PrimaryAction.WasPressed; }
        }

        public bool FirstActionWasReleased {
            get { return actions.PrimaryAction.WasReleased; }
        }
        #endregion

        #region Second Action
        public bool SecondActionIsPressed {
            get { return actions.SecondaryAction.IsPressed; }
        }

        public bool SecondActionWasPressed {
            get { return actions.SecondaryAction.WasPressed; }
        }

        public bool SecondActionWasReleased {
            get { return actions.SecondaryAction.WasReleased; }
        }
        #endregion

		#region Stick
		public Vector2 Stick {
			get { return actions.StickDirection.Value; }
		}

		public float StickAngle {
			get { return Mathf.Atan2(Stick.y, Stick.x); }
		}

        public bool LeftIsPressed {
            get { return actions.StickLeft.IsPressed; }
        }

        public bool LeftWasPressed {
            get { return actions.StickLeft.WasPressed; }
        }

        public bool LeftWasReleased {
            get { return actions.StickLeft.WasReleased; }
        }

        public bool RightIsPressed {
            get { return actions.StickRight.IsPressed; }
        }

        public bool RightWasPressed {
            get { return actions.StickRight.WasPressed; }
        }

        public bool RightWasReleased {
            get { return actions.StickRight.WasReleased; }
        }

        public bool UpIsPressed {
            get { return actions.StickUp.IsPressed; }
        }

        public bool UpWasPressed {
            get { return actions.StickUp.WasPressed; }
        }

        public bool UpWasReleased {
            get { return actions.StickUp.WasReleased; }
        }

        public bool DownIsPressed {
            get { return actions.StickDown.IsPressed; }
        }

        public bool DownWasPressed {
            get { return actions.StickDown.WasPressed; }
        }

        public bool DownWasReleased {
            get { return actions.StickDown.WasReleased; }
        }
        #endregion
    }

    class PlayerActions : PlayerActionSet {
        public PlayerAction StickUp;
        public PlayerAction StickDown;
        public PlayerAction StickLeft;
        public PlayerAction StickRight;
        public PlayerTwoAxisAction StickDirection;
        public PlayerAction PrimaryAction;
        public PlayerAction SecondaryAction;

        public PlayerActions() {
            StickUp = CreatePlayerAction("Stick Up");
            StickDown = CreatePlayerAction("Stick Down");
            StickLeft = CreatePlayerAction("Stick Left");
            StickRight = CreatePlayerAction("Stick Right");
            StickDirection = CreateTwoAxisPlayerAction(StickLeft, StickRight, StickDown, StickUp);

            PrimaryAction = CreatePlayerAction("Primary Action");
            SecondaryAction = CreatePlayerAction("Secondary Action");

            Initialize();
        }

        void Initialize() {
            StickLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
            StickLeft.AddDefaultBinding(InputControlType.DPadLeft);

            StickRight.AddDefaultBinding(InputControlType.LeftStickRight);
            StickRight.AddDefaultBinding(InputControlType.DPadRight);

            StickUp.AddDefaultBinding(InputControlType.LeftStickUp);
            StickUp.AddDefaultBinding(InputControlType.DPadUp);

            StickDown.AddDefaultBinding(InputControlType.LeftStickDown);
            StickDown.AddDefaultBinding(InputControlType.DPadDown);

            PrimaryAction.AddDefaultBinding(InputControlType.Action1);
            PrimaryAction.AddDefaultBinding(InputControlType.Action3);

            SecondaryAction.AddDefaultBinding(InputControlType.Action2);
            SecondaryAction.AddDefaultBinding(InputControlType.Action4);
            PrimaryAction.AddDefaultBinding(InputControlType.LeftBumper);
            PrimaryAction.AddDefaultBinding(InputControlType.LeftTrigger);

            SecondaryAction.AddDefaultBinding(InputControlType.RightBumper);
            SecondaryAction.AddDefaultBinding(InputControlType.RightTrigger);

            //PrimaryAction.AddDefaultBinding(InputControlType.Command);
            //PrimaryAction.AddDefaultBinding(InputControlType.Home);

            StickUp.AddDefaultBinding(Key.UpArrow);
            StickDown.AddDefaultBinding(Key.DownArrow);
            StickLeft.AddDefaultBinding(Key.LeftArrow);
            StickRight.AddDefaultBinding(Key.RightArrow);
            PrimaryAction.AddDefaultBinding(Key.Z);
            SecondaryAction.AddDefaultBinding(Key.X);
            

            StickUp.AddDefaultBinding(Key.W);
            StickDown.AddDefaultBinding(Key.S);
            StickLeft.AddDefaultBinding(Key.A);
            StickRight.AddDefaultBinding(Key.D);
            PrimaryAction.AddDefaultBinding(Key.N);
            SecondaryAction.AddDefaultBinding(Key.M);
        }
    }
}