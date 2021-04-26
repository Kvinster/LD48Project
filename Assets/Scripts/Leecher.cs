using UnityEngine;
using UnityEngine.Assertions;

using System.Collections.Generic;
using System.Linq;

using DG.Tweening;
using Shapes;

namespace LD48Project {
	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class Leecher : MonoBehaviour {
		const float DeathTime                 = 5f;
		const float CompleteDeathAnimDuration = 2f;
		const float DeathPushForce            = 0.05f;

		public static readonly HashSet<Leecher> Instances = new HashSet<Leecher>();

		[Header("Parameters")]
		public float StartHp;
		public Color RawColor;
		public float MinIntensity = 0f;
		public float MaxIntensity = 2f;
		[Header("Dependencies")]
		public Rectangle             Rectangle;
		public List<LeecherTentacle> Tentacles = new List<LeecherTentacle>();

		Rigidbody2D _rigidbody;

		float _curHp;

		float _deathTimer;

		Tween _completeDeathAnim;

		public bool IsAlive { get; private set; }

		bool IsAttached {
			get {
				return Tentacles.Any(x => x.IsAttached);
			}
		}

		float CurHp {
			get => _curHp;
			set => _curHp = value;
		}

		void Reset() {
			GetComponentsInChildren(Tentacles);
		}

		void OnDestroy() {
			Instances.Remove(this);
		}

		void Awake() {
			Instances.Add(this);
		}

		void Start() {
			_rigidbody = GetComponent<Rigidbody2D>();

			CurHp = StartHp;

			foreach ( var tentacle in Tentacles ) {
				tentacle.Init(this);
			}
		}

		void Update() {
			if ( !IsAlive ) {
				_deathTimer = Mathf.Max(0f, _deathTimer - Time.deltaTime);
				if ( Mathf.Approximately(_deathTimer, 0f) ) {
					IsAlive = true;
					CurHp   = StartHp;
				}
			} else if ( IsAttached ) {
				SetIntensity(Mathf.PingPong(Time.time, MaxIntensity - MinIntensity));
			} else {
				SetIntensity(0f);
			}
		}

		public void TryTakeDischarge() {
			if ( !IsAttached ) {
				return;
			}

			Tentacles.ForEach(x => x.Deattach());

			IsAlive     = false;
			_deathTimer = float.MaxValue;

			_rigidbody.AddForce(
				(transform.position - Submarine.Instance.transform.position).normalized * DeathPushForce,
				ForceMode2D.Impulse);

			PlayCompleteDeathAnim();
		}

		void PlayCompleteDeathAnim() {
			Assert.IsNull(_completeDeathAnim);

			var intensity = 0f;
			_completeDeathAnim = DOTween.Sequence().Append(DOTween.To(() => intensity, x => {
				intensity = x;
				SetIntensity(x);
			}, 2f, CompleteDeathAnimDuration).SetEase(Ease.InBounce));
			_completeDeathAnim.onComplete += () => Destroy(gameObject);
		}

		void TakeDamage(float damage) {
			if ( !IsAlive ) {
				return;
			}
			CurHp = Mathf.Max(CurHp - damage, 0f);
			if ( Mathf.Approximately(CurHp, 0f) ) {
				IsAlive    = false;
				_deathTimer = DeathTime;

				Tentacles.ForEach(x => x.Deattach());
			}
		}

		void OnCollisionEnter2D(Collision2D other) {
			if ( other.gameObject.GetComponent<Bullet>() ) {
				TakeDamage(Bullet.Damage);
			}
		}

		void SetIntensity(float intensity) {
			var factor   = Mathf.Pow(2,intensity);
			var newColor = new Color(RawColor.r * factor, RawColor.g * factor, RawColor.b * factor);
			Rectangle.Color = newColor;
		}
	}
}
