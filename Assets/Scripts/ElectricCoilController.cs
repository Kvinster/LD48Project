using UnityEngine;

using DG.Tweening;

using UnityEngine.Assertions;

namespace LD48Project {
	public sealed class ElectricCoilController : MonoBehaviour {
		[Header("Parameters")]
		public float ReloadTime           = 5f;
		public float GraphicsShowTime     = 0.1f;
		public float ActiveReloadTimeMult = 2f;
		[Header("Dependencies")]
		public GameObject GraphicsRoot;

		bool _isActive;

		bool  _isReady = true;
		float _reloadTimer;

		Tween _dischargeAnim;

		void Update() {
			if ( !_isReady ) {
				_reloadTimer = Mathf.Max(_reloadTimer - Time.deltaTime * (_isActive ? ActiveReloadTimeMult : 1f), 0f);
				if ( Mathf.Approximately(_reloadTimer, 0f) ) {
					_isReady = true;
				}
			}

			if ( !_isActive ) {
				return;
			}
			if ( _isReady && Input.GetKeyDown(KeyCode.Space) ) {
				Discharge();
			}
		}

		public void SetActive(bool isActive) {
			_isActive = isActive;

			// TODO: show help + discharge button
		}

		void Discharge() {
			Assert.IsTrue(_isReady);
			_dischargeAnim?.Kill();
			_dischargeAnim = DOTween.Sequence()
				.AppendCallback(() => GraphicsRoot.SetActive(true))
				.AppendInterval(GraphicsShowTime)
				.AppendCallback(() => GraphicsRoot.SetActive(false));

			// TODO: deal damage

			_isReady     = false;
			_reloadTimer = ReloadTime;
		}
	}
}
