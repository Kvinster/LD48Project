using UnityEngine;

namespace LD48Project {
	public sealed class TentacleTest : MonoBehaviour {
		public Transform ForceApplyPos;
		public float     Force;
		public Transform Target;

		Rigidbody2D _rigidbody;

		void Start() {
			_rigidbody = GetComponent<Rigidbody2D>();
		}

		void Update() {
			if ( Input.GetKey(KeyCode.Space) ) {
				_rigidbody.AddForceAtPosition((Target.position - ForceApplyPos.position).normalized * Force,
					ForceApplyPos.position, ForceMode2D.Force);
			}
		}
	}
}
