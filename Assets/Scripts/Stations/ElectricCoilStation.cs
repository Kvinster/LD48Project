using UnityEngine;

namespace LD48Project.Stations {
	public sealed class ElectricCoilStation : BaseStation {
		[Space]
		public ElectricCoilController ElectricCoilController;

		public override void Activate() {
			base.Activate();
			ElectricCoilController.SetActive(true);
		}

		public override void Deactivate() {
			base.Deactivate();
			ElectricCoilController.SetActive(false);
		}
	}
}
