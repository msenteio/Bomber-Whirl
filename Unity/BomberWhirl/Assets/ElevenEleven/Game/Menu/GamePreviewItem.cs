namespace ElevenEleven.Game {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class GamePreviewItem : CenterScrollRectItem {

        [SerializeField]
        RawImage m_rawImage;
        RawImage rawImage {
            get { return m_rawImage; }
        }

		[SerializeField] Image[] borderImages;

        [SerializeField]
        AspectRatioFitter m_aspectRatio;
        AspectRatioFitter aspectRatio {
            get { return m_aspectRatio; }
        }

        [SerializeField]
        ElevenConfig m_gameConfig;
        ElevenConfig gameConfig {
            get { return m_gameConfig; }
            set { m_gameConfig = value; }
        }

		[SerializeField]
		GameSelectButton m_gameSelection;
		GameSelectButton gameSelection {
			get { return m_gameSelection; }
			set { m_gameSelection = value; }
		}

		internal void Initialize(ElevenConfig gameConfig, GameSelectButton gameSelection, Color color) {
            this.gameConfig = gameConfig;
			this.gameSelection = gameSelection;
			rawImage.texture = gameConfig.previewImage;
			aspectRatio.aspectRatio = (float)gameConfig.previewImage.width / gameConfig.previewImage.height;
       
			foreach (var borderImage in borderImages) {
				borderImage.color = color.NewAlpha(borderImage.color.a);
			}
		}

		protected override void Update() {
			float brightness;
			if (gameSelection.selected) {
				base.Update();
				brightness = 1.0f;
			} else {
				brightness = 128.0f / 255.0f;
			}	

			HSBColor hsbColor = new HSBColor(rawImage.color);
			hsbColor.brightness = Mathf.Lerp(hsbColor.brightness, brightness, 5 * Time.deltaTime);
			rawImage.color = hsbColor.ToColor();
		}
    }
}