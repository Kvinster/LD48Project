using UnityEngine;

using System.Collections;

using Random = UnityEngine.Random;

namespace LD48Project {
	public sealed class LeecherTentacle : MonoBehaviour {
		const float Force           = 0.003f;
		const float MaxWorkTime     = 0.8f;
		const float IdleTime        = 0.2f;
		const float RegenerateTime  = 5f;
		const float DamagePerSecond = 0.25f;

		public Transform ForceApplyPos;

		Leecher _owner;

		Rigidbody2D _rigidbody;

		FixedJoint2D _joint;

		bool  _isIdle;
		float _idleTimer;

		bool  _attached;
		float _regenTimer;

		Transform _target;

		public bool IsAttached => _joint;

		bool IsRegenerating => !Mathf.Approximately(_regenTimer, 0f);

		IEnumerator Start() {
			_rigidbody = GetComponent<Rigidbody2D>();

			yield return null;

			_target = Submarine.Instance.transform;
		}

		void Update() {
			if ( _isIdle ) {
				_idleTimer = Mathf.Max(0f, _idleTimer - Time.deltaTime);
				if ( Mathf.Approximately(_idleTimer, 0f) ) {
					_isIdle = false;
					_idleTimer  = MaxWorkTime;
				}
			}
			if ( IsRegenerating ) {
				_regenTimer = Mathf.Max(0f, _regenTimer - Time.deltaTime);
			} else if ( _attached && !_joint ) { // joint broke
				DestroyJoint();
			}
			if ( !_owner.IsAlive ) {
				return;
			}
			if ( IsAttached ) {
				Submarine.Instance.TakeDamage(DamagePerSecond * Time.deltaTime);
			} else {
				TryWork();
			}
		}

		public void Init(Leecher owner) {
			_owner = owner;
		}

		public void Deattach() {
			if ( !IsAttached ) {
				return;
			}
			DestroyJoint();
		}

		void DestroyJoint() {
			if ( _joint ) {
				Destroy(_joint);
			}
			_attached   = false;
			_regenTimer = RegenerateTime;
		}

		void TryWork() {
			if ( !_target || _isIdle || IsAttached || IsRegenerating ) {
				return;
			}

			var hit = Physics2D.Raycast(ForceApplyPos.position,
				(_target.position - ForceApplyPos.position).normalized, float.MaxValue, 1 << 8);
			if ( !hit.collider ) {
				return;
			}
			var direction = (hit.point - (Vector2)ForceApplyPos.position).normalized;
			var angle     = Vector2.SignedAngle(Vector2.up, direction);
			var spread    = 60;
			angle     += Random.Range(-spread, spread);
			direction =  Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.up;
			direction =  direction.normalized;
			_rigidbody.AddForceAtPosition(direction * Force,
				ForceApplyPos.position, ForceMode2D.Force);

			_idleTimer = Mathf.Max(0f, _idleTimer - Time.deltaTime);
			if ( Mathf.Approximately(0f, _idleTimer) ) {
				_isIdle = true;
				_idleTimer  = IdleTime;
			}
		}

		void OnCollisionEnter2D(Collision2D other) {
			if ( !_owner.IsAlive || _attached ) {
				return;
			}
			if ( other.gameObject.CompareTag("Player") ) {
				var playerRb = other.rigidbody;
				Attach(playerRb);
			}
		}

		void Attach(Rigidbody2D rigidbody) {
			if ( _joint ) {
				return;
			}
			_joint               = gameObject.AddComponent<FixedJoint2D>();
			_joint.connectedBody = rigidbody;
			_joint.breakForce    = 0.12f;

			_attached = true;
		}
	}
}
