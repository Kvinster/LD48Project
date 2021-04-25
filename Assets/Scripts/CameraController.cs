using UnityEngine;

namespace LD48Project {
	[RequireComponent(typeof(Camera))]
	public sealed class CameraController : MonoBehaviour {
		[Header("Zoom")]
		public float ZoomSpeed;
		public float MaxCameraSize;
		public float MinCameraSize;
		[Header("Pan")]
		public float PanSpeed;

		Camera _camera;

		Vector2 _mousePosition;

		void Start() {
			_camera = GetComponent<Camera>();
		}

		void Update() {
			if ( Input.mouseScrollDelta != Vector2.zero ) {
				_camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - ZoomSpeed * Input.mouseScrollDelta.y,
					MinCameraSize, MaxCameraSize);
			}
			if ( Input.GetMouseButton(1) ) {
				transform.Translate(_camera.ScreenToWorldPoint(_mousePosition) - _camera.ScreenToWorldPoint(Input.mousePosition));
			}

			_mousePosition = Input.mousePosition;
		}
	}
}
