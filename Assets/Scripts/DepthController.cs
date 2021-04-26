using UnityEngine;

using System.Globalization;

using TMPro;

namespace LD48Project {
	public sealed class DepthController : MonoBehaviour {
		[Header("Parameters")]
		public float DescendSpeed;
		[Header("Dependencies")]
		public TMP_Text DepthText;

		public float CurDepth         { get; private set; } = 0f;
		public float DescendSpeedMult { get; set; }         = 1f;

		void Update() {
			if ( !Mathf.Approximately(DescendSpeed, 0f) ) {
				CurDepth += DescendSpeed * DescendSpeedMult * Time.deltaTime;
				UpdateText();
			}
		}

		void UpdateText() {
			DepthText.text = $"Depth: {CurDepth.ToString("F2", CultureInfo.InvariantCulture)}m";
		}
	}
}
