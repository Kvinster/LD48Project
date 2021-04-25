using UnityEngine;
using UnityEngine.Assertions;

using System.Collections.Generic;

using DG.Tweening;
using Shapes;

namespace LD48Project.Stations {
	public abstract class BaseStation : MonoBehaviour {
		const float ActiveRotationSpeed = 10f;
		const float TransitionDuration  = 2f;

		public Disc                     Graphic;
		public StationActivationTrigger Trigger;
		[Space]
		public List<BaseStation> Neighbours = new List<BaseStation>();

		protected bool IsActive;

		Sequence _transitionAnim;

		protected virtual void Reset() {
			Graphic = GetComponent<Disc>();
			Trigger = GetComponentInChildren<StationActivationTrigger>();
		}

		protected virtual void OnValidate() {
			if ( Neighbours.Count == 0 ) {
				Debug.LogErrorFormat(this, "Station '{0}' has no neighbours", gameObject.name);
			}
		}

		protected virtual void Start() {
			if ( Trigger ) {
				Trigger.Init(this);
			}
		}

		protected virtual void Update() {
			if ( Graphic && IsActive && (_transitionAnim == null) ) {
				Graphic.DashOffset += ActiveRotationSpeed * Time.deltaTime;
				Graphic.DashOffset %= 1;
			}
		}

		public virtual void Activate() {
			Assert.IsFalse(IsActive);

			IsActive = true;

			StartActiveAnim();
		}

		public virtual void Deactivate() {
			Assert.IsTrue(IsActive, gameObject.name);

			IsActive = false;

			StopActiveAnim();
		}

		void StartActiveAnim() {
			if ( !Graphic ) {
				return;
			}
			_transitionAnim?.Kill();
			var curOffset = Graphic.DashOffset;
			_transitionAnim = DOTween.Sequence().Append(DOTween.To(() => Graphic.DashOffset,
				x => Graphic.DashOffset = x, curOffset + ActiveRotationSpeed, TransitionDuration).SetEase(Ease.InSine));
			_transitionAnim.onComplete += () => { _transitionAnim = null; };
		}

		void StopActiveAnim() {
			_transitionAnim?.Kill();
			if ( !Graphic ) {
				_transitionAnim = null;
				return;
			}
			Graphic.DashOffset %= 1;
			var curOffset = Graphic.DashOffset;
			var endOffset = Graphic.DashSize + 1f / Graphic.DashSize;
			_transitionAnim = DOTween.Sequence().Append(DOTween.To(() => Graphic.DashOffset,
				x => Graphic.DashOffset = x, endOffset, TransitionDuration * (1f - curOffset / endOffset)));
			_transitionAnim.onComplete += () => { _transitionAnim = null; };
		}
	}
}
