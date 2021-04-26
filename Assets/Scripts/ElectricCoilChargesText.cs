using UnityEngine;

using TMPro;

namespace LD48Project {
	public sealed class ElectricCoilChargesText : MonoBehaviour {
		public TMP_Text               Text;
		public ElectricCoilController ElectricCoilController;

		void Start() {
			ElectricCoilController.OnCurChargesChanged += OnCurChargesChanged;
			OnCurChargesChanged(ElectricCoilController.CurCharges);
		}

		void OnCurChargesChanged(int curCharges) {
			Text.text = $"Charges: {curCharges}";
		}
	}
}
