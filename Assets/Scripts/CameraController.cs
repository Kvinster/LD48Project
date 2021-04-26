using UnityEngine;

using System.Collections;

namespace LD48Project {
	[RequireComponent(typeof(Camera))]
	public sealed class CameraController : MonoBehaviour {
		[Header("Zoom")]
		public float ZoomSpeed;
		public float MaxCameraSize;
		public float MinCameraSize;
		[Header("Pan")]
		public float MaxDistance = 10f;

		Camera _camera;

		Vector2   _mousePosition;
		Transform _subTransform;

		IEnumerator Start() {
			yield return null; // so that the sub can init
			_camera       = GetComponent<Camera>();
			_subTransform = Submarine.Instance.transform;
		}

		void Update() {
			if ( Input.mouseScrollDelta != Vector2.zero ) {
				_camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - ZoomSpeed * Input.mouseScrollDelta.y,
					MinCameraSize, MaxCameraSize);
			}
			if ( Input.GetMouseButton(1) ) {
				var pos = transform.position;
				var newPos = pos + _camera.ScreenToWorldPoint(_mousePosition) -
				             _camera.ScreenToWorldPoint(Input.mousePosition);
				if ( (!(Mathf.Abs(newPos.x - _subTransform.position.x) > MaxDistance)) &&
				     (!(Mathf.Abs(newPos.y - _subTransform.position.y) > MaxDistance)) ) {
					transform.position = newPos;
				}
			}

			_mousePosition = Input.mousePosition;
		}
	}
}
