using UnityEngine;

using System;

using DG.Tweening;

namespace LD48Project {
	public sealed class ElectricCoilController : MonoBehaviour {
		[Header("Parameters")]
		public float ReloadTime           = 5f;
		public float GraphicsShowTime     = 0.1f;
		public float ActiveReloadTimeMult = 2f;
		public float UiAnimDuration       = 0.5f;
		public int   StartCharges         = 5;
		[Header("Dependencies")]
		public GameObject GraphicsRoot;
		[Space]
		public GameObject       UiRoot;
		public GameObject       ReloadProgressBarRoot;
		public ShapeProgressBar ReloadProgressBar;
		public ShapeButton      Button;
		public GameObject       ReadyTextRoot;
		public Transform        ButtonRoot;
		public Transform        ButtonAppearPos;
		public Transform        ButtonDisappearPos;

		bool _isActive;

		bool  _isReady = true;
		float _reloadTimer;

		Tween _dischargeAnim;
		Tween _uiAnim;

		int _curCharges;

		public int CurCharges {
			get => _curCharges;
			private set {
				_curCharges = value;
				OnCurChargesChanged?.Invoke(CurCharges);
			}
		}

		bool IsReady {
			get => _isReady;
			set {
				_isReady       = value;
				Button.Enabled = IsReady;
				ReadyTextRoot.SetActive(IsReady);
				ReloadProgressBarRoot.SetActive(!IsReady);
			}
		}

		public Action<int> OnCurChargesChanged;

		void Start() {
			CurCharges = StartCharges;

			Button.AddClickAction(TryDischarge);
			UiRoot.SetActive(false);
			ButtonRoot.localPosition = ButtonDisappearPos.localPosition;
		}

		void Update() {
			if ( !IsReady ) {
				_reloadTimer = Mathf.Max(_reloadTimer - Time.deltaTime * (_isActive ? ActiveReloadTimeMult : 1f), 0f);
				ReloadProgressBar.Progress = 1f - _reloadTimer / ReloadTime;
				if ( Mathf.Approximately(_reloadTimer, 0f) ) {
					IsReady = true;
				}
			}

			if ( !_isActive || !Submarine.Instance.IsAlive ) {
				return;
			}
			if ( IsReady && (CurCharges > 0) && Input.GetKeyDown(KeyCode.Space) ) {
				TryDischarge();
			}
		}

		public void SetActive(bool isActive) {
			_isActive = isActive;

			Button.Enabled = IsReady;
			if ( _isActive ) {
				if ( CurCharges > 0 ) {
					ShowUi();
				}
			} else {
				HideUi();
			}
		}

		void ShowUi() {
			UiRoot.SetActive(true);
			_uiAnim?.Kill();
			_uiAnim = DOTween.Sequence()
				.Append(ButtonRoot.DOLocalMove(ButtonAppearPos.localPosition,
						UiAnimDuration * Vector2.Distance(ButtonRoot.localPosition, ButtonAppearPos.localPosition) /
						Vector2.Distance(ButtonAppearPos.localPosition, ButtonDisappearPos.localPosition))
					.SetEase(Ease.InSine));
			_uiAnim.onComplete += () => { _uiAnim = null; };
		}

		void HideUi() {
			_uiAnim?.Kill();
			_uiAnim = DOTween.Sequence()
				.Append(ButtonRoot.DOLocalMove(ButtonDisappearPos.localPosition,
						UiAnimDuration * Vector2.Distance(ButtonRoot.localPosition, ButtonDisappearPos.localPosition) /
						Vector2.Distance(ButtonAppearPos.localPosition, ButtonDisappearPos.localPosition))
					.SetEase(Ease.InSine));
			_uiAnim.onComplete += () => {
				UiRoot.SetActive(false);
				_uiAnim = null;
			};
		}

		void TryDischarge() {
			if ( !IsReady ) {
				return;
			}
			_dischargeAnim?.Kill();
			_dischargeAnim = DOTween.Sequence()
				.AppendCallback(() => GraphicsRoot.SetActive(true))
				.AppendInterval(GraphicsShowTime)
				.AppendCallback(() => GraphicsRoot.SetActive(false));

			foreach ( var leecher in Leecher.Instances ) {
				leecher.TryTakeDischarge();
			}

			--CurCharges;

			if ( CurCharges == 0 ) {
				_reloadTimer = float.MaxValue;
				HideUi();
			} else {
				_reloadTimer = ReloadTime;
			}

			IsReady = false;
		}
	}
}
