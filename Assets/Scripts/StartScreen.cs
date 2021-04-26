using UnityEngine;
using UnityEngine.UI;

namespace LD48Project {
	public sealed class StartScreen : MonoBehaviour {
		public GameObject  GameRoot;
		public GameObject  DepthMeterRoot;
		public GameObject  SubHpRoot;
		public GameObject  ButtonsContainer;
		public ShapeButton StartButton;
		public ShapeButton HowToPlayButton;
		public GameObject  HowToPlayRoot;
		public Button      HideHowToPlayButton;

		void Start() {
			GameRoot.SetActive(false);
			DepthMeterRoot.SetActive(false);
			SubHpRoot.SetActive(false);

			Time.timeScale = 0f;

			StartButton.AddClickAction(OnStartClick);
			HowToPlayButton.AddClickAction(OnHowToPlayClick);
			HideHowToPlayButton.onClick.AddListener(OnHideHowToPlayClick);
		}

		void OnDestroy() {
			if ( StartButton ) {
				StartButton.RemoveClickAction(OnStartClick);
			}
			if ( HowToPlayButton ) {
				HowToPlayButton.RemoveClickAction(OnHowToPlayClick);
			}
		}

		void OnStartClick() {
			Time.timeScale = 1f;
			gameObject.SetActive(false);

			GameRoot.SetActive(true);
			DepthMeterRoot.SetActive(true);
			SubHpRoot.SetActive(true);
		}

		void OnHowToPlayClick() {
			ButtonsContainer.SetActive(false);
			HowToPlayRoot.SetActive(true);
		}

		void OnHideHowToPlayClick() {
			ButtonsContainer.SetActive(true);
			HowToPlayRoot.SetActive(false);
		}
	}
}
