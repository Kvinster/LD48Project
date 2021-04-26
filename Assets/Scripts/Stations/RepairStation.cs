using UnityEngine;

namespace LD48Project.Stations {
	public sealed class RepairStation : BaseStation {
		[Header("Parameters")]
		public float RepairSpeed;
		[Header("Dependencies")]
		public Submarine Submarine;

		protected override void Update() {
			base.Update();

			if ( IsActive ) {
				Submarine.TryAddHp(RepairSpeed * Time.deltaTime);
			}
		}
	}
}
