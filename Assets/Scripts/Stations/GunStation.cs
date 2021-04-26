using UnityEngine;

namespace LD48Project.Stations {
	public sealed class GunStation : BaseStation {
		[Space]
		public GunController GunController;

		public override void Activate(bool startAnim = true) {
			base.Activate(startAnim);
			GunController.IsActive = true;
		}

		public override void Deactivate() {
			base.Deactivate();
			GunController.IsActive = false;
		}
	}
}
