using UnityEngine;
using UnityEngine.Assertions;

using System;

namespace LD48Project {
	public sealed class Submarine : MonoBehaviour {
		public static Submarine Instance { get; private set; }

		[Header("Parameters")]
		public float StartHp = 100;
		[Header("Dependencies")]
		public ShapeProgressBar HealthProgressBar;

		float _curHp;

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

		void Start() {
			CurHp = StartHp;
		}

		public void TakeDamage(float damage) {
			if ( !IsAlive ) {
				return;
			}

			CurHp = Mathf.Max(CurHp - damage, 0);

			if ( Mathf.Approximately(CurHp, 0f) ) {
				IsAlive = false;
				Debug.Log("bleugh");
				// TODO: die
			}
		}
	}
}
