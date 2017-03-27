namespace ElevenEleven.Game {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.EventSystems;
	using TMPro;

	[System.Serializable]
	public class ImageAlpha {
		public Image image;
		public float brightness;
		public float alpha;
	}

	public class GameSelectButton : CenterScrollRectItem, ISelectHandler, IDeselectHandler {
		
		bool m_selected = false;
		public bool selected {
			get { return m_selected; }
			private set { m_selected = value; }
		}

		[SerializeField] TextMeshProUGUI m_titleText;
		TextMeshProUGUI titleText {
			get { return m_titleText; }
		}

		[SerializeField] ImageAlpha[] m_images;
		ImageAlpha[] images {
			get { return m_images; }
		}

		[SerializeField]
		ElevenConfig m_gameConfig;
		ElevenConfig gameConfig {
			get { return m_gameConfig; }
			set { m_gameConfig = value; }
		}

		[SerializeField]
		GamePreviewItem m_previewItem;
		GamePreviewItem previewItem {
			get { return m_previewItem; }
			set { m_previewItem = value; }
		}

		Button m_button;
		Button button {
			get {
				if (m_button == null) {
					m_button = GetComponent<Button>();
				}
				return m_button;
			}
		}
	
		internal void Initialize(ElevenConfig gameConfig, GamePreviewItem preview, Color color) {
			this.gameConfig = gameConfig;
			this.previewItem = preview;
			titleText.text = gameConfig.gameName;

			foreach (var image in images) {
				HSBColor hsbColor = new HSBColor(color);
				hsbColor.brightness = image.brightness;
				hsbColor.alpha = image.alpha;
				image.image.color = hsbColor.ToColor();
			}
				
			button.onClick.AddListener(PlayGame);
		}

		protected override void Update() {
			if (selected) {
				base.Update();
			}
		}

		public void PlayGame() {
			ShallowMusic.Play("UI", 1, BeatValue.Quarter);
			ShallowGames.NewGame(gameConfig);
		}

		public void OnSelect(BaseEventData eventData) {
			selected = true;	
			PlaySelectSound();
		}

		public void OnDeselect(BaseEventData eventData) {
			selected = false;	
		}

		void PlaySelectSound() {
			ShallowMusic.Play("UI", 0, BeatValue.Eighth, 0.5f);
		}
	}
}