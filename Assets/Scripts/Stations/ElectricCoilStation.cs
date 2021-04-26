using UnityEngine;

namespace LD48Project.Stations {
	public sealed class ElectricCoilStation : BaseStation {
		[Space]
		public ElectricCoilController ElectricCoilController;

		protected override bool PlayActiveAnim => (ElectricCoilController.CurCharges > 0);

		protected override void Start() {
			base.Start();
			ElectricCoilController.OnCurChargesChanged += OnCurChargesChanged;
		}

		public override void Activate(bool startAnim = true) {
			base.Activate(startAnim && (ElectricCoilController.CurCharges > 0));
			ElectricCoilController.SetActive(true);
		}

		public override void Deactivate() {
			base.Deactivate();
			ElectricCoilController.SetActive(false);
		}

		void OnCurChargesChanged(int curCharges) {
			if ( IsActive && (curCharges == 0) ) {
				StopActiveAnim(true);
			}
		}
	}
}
