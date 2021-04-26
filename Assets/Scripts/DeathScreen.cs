using UnityEngine;
using UnityEngine.SceneManagement;

using System.Globalization;

using TMPro;

namespace LD48Project {
	public sealed class DeathScreen : MonoBehaviour {
		public GameObject      GameRoot;
		public GameObject      DepthMeterRoot;
		public GameObject      SubHpRoot;
		public TMP_Text        Text;
		public DepthController DepthController;
		public ShapeButton     TryAgainButton;

		void OnEnable() {
			DepthMeterRoot.SetActive(false);
			SubHpRoot.SetActive(false);
			Text.text =
				$"YOU DIED\nYOUR RESULT: {DepthController.CurDepth.ToString("F2", CultureInfo.InvariantCulture)}m";
			TryAgainButton.AddClickAction(OnTryAgainClick);
		}

		public void Show() {
			Time.timeScale = 0f;
			GameRoot.SetActive(false);
			gameObject.SetActive(true);
		}

		void OnTryAgainClick() {
			Time.timeScale = 1f;
			SceneManager.LoadScene(sceneBuildIndex: 0);
		}
	}
}
