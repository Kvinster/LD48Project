using UnityEngine;

namespace LD48Project.Stations {
	public sealed class DrivingStation : BaseStation {
		[Header("Parameters")]
		public float ActiveDescendSpeedMult = 1.5f;
		[Header("Dependencies")]
		public DepthController DepthController;

		public override void Activate(bool startAnim = true) {
			base.Activate(startAnim);
			DepthController.DescendSpeedMult = ActiveDescendSpeedMult;
		}

		public override void Deactivate() {
			base.Deactivate();
			DepthController.DescendSpeedMult = 1f;
		}
	}
}
