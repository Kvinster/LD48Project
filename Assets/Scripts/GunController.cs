using UnityEngine;
using UnityEngine.Assertions;

namespace LD48Project {
	public sealed class GunController : MonoBehaviour {
		[Header("Parameters")]
		public float ReloadTime;
		public float BulletSpeed;
		public float RotationSpeed;
		public float MinAngle;
		public float MaxAngle;
		[Header("Dependencies")]
		public Transform  GunRotationRoot;
		public Transform  BulletOrigin;
		public GameObject BulletPrefab;

		public bool IsActive { get; set; }

		bool  _isReady;
		float _reloadTimer;

		void Update() {
			if ( !_isReady ) {
				_reloadTimer = Mathf.Max(_reloadTimer - Time.deltaTime, 0f);
				if ( Mathf.Approximately(_reloadTimer, 0f) ) {
					_isReady = true;
				}
			}

			if ( !IsActive ) {
				return;
			}
			if ( Input.GetKey(KeyCode.A) ) {
				AddAngle(RotationSpeed * Time.deltaTime);
			} else if ( Input.GetKey(KeyCode.D) ) {
				AddAngle(-RotationSpeed * Time.deltaTime);
			}
			if ( _isReady && Input.GetKey(KeyCode.Space) ) {
				Shoot();
			}
		}

		void AddAngle(float angle) {
			var newAngle = GunRotationRoot.localRotation.eulerAngles.z + angle;
			while ( newAngle > 180f ) {
				newAngle -= 360f;
			}
			while ( newAngle < -180f ) {
				newAngle += 360f;
			}
			newAngle = Mathf.Clamp(newAngle, MinAngle, MaxAngle);
			GunRotationRoot.localRotation = Quaternion.Euler(0f, 0f, newAngle);
		}

		void Shoot() {
			Assert.IsTrue(_isReady);
			var bulletGo = Instantiate(BulletPrefab, BulletOrigin.position, GunRotationRoot.rotation, null);
			var bulletRb = bulletGo.GetComponent<Rigidbody2D>();
			Assert.IsTrue(bulletRb);
			bulletRb.AddForce(GunRotationRoot.TransformDirection(Vector2.up) * (bulletRb.mass * BulletSpeed),
				ForceMode2D.Impulse);

			_isReady     = false;
			_reloadTimer = ReloadTime;
		}
	}
}
