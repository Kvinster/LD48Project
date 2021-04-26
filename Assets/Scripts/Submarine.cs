using UnityEngine;
using UnityEngine.Assertions;

using System;

using DG.Tweening;

namespace LD48Project {
	public sealed class Submarine : MonoBehaviour {
		public static Submarine Instance { get; private set; }

		[Header("Parameters")]
		public float StartHp = 100;
		public float DeathAnimDuration = 1f;
		[Header("Dependencies")]
		public DeathScreen      DeathScreen;
		public SubWater         SubWater;
		public ShapeProgressBar HealthProgressBar;

		float _curHp;

		Sequence _deathAnim;

		public float CurHp {
			get => _curHp;
			private set {
				_curHp = value;

				HealthProgressBar.Progress = CurHp / StartHp;

				OnCurHpChanged?.Invoke(_curHp);
			}
		}

		public bool IsAlive { get; private set; } = true;

		public event Action<float> OnCurHpChanged;

		void Awake() {
			Assert.IsFalse(Instance);
			Instance = this;
		}

		void OnDestroy() {
			if ( Instance == this ) {
				Instance = null;
			}
		}

		void Start() {
			CurHp = StartHp;
		}

		public void TryAddHp(float hp) {
			if ( !IsAlive ) {
				return;
			}

			CurHp = Mathf.Min(CurHp + hp, StartHp);
		}

		public void TakeDamage(float damage) {
			if ( !IsAlive ) {
				return;
			}

			CurHp = Mathf.Max(CurHp - damage, 0);

			if ( Mathf.Approximately(CurHp, 0f) ) {
				IsAlive = false;
				PlayDeathAnim();
			}
		}

		void PlayDeathAnim() {
			Assert.IsNull(_deathAnim);
			_deathAnim = DOTween.Sequence()
				.Append(DOTween.To(() => SubWater.Level, x => SubWater.Level = x, 1f, DeathAnimDuration)
					.SetEase(Ease.Linear));
			_deathAnim.onComplete += DeathScreen.Show;
		}
	}
}
