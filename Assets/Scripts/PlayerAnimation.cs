using UnityEngine;

using DG.Tweening;
using Shapes;

namespace LD48Project {
	[RequireComponent(typeof(Disc))]
	public sealed class PlayerAnimation : MonoBehaviour {
		public Color RawColor         = Color.yellow;
		public float MaxIdleIntensity = 5f;
		public float MinIdleIntensity = 1f;
		public float IdleAnimDuration = 0.5f;

		Disc _disc;

		Sequence _idleAnim;

		void Start() {
			_disc = GetComponent<Disc>();

			SetIntensity(MinIdleIntensity);
			var intensity = MinIdleIntensity;
			_idleAnim = DOTween.Sequence().Append(DOTween.To(() => intensity, x => {
				intensity = x;
				SetIntensity(x);
			}, MaxIdleIntensity, IdleAnimDuration / 2f)).SetLoops(-1, LoopType.Yoyo);
		}

		void SetIntensity(float intensity) {
			var factor   = Mathf.Pow(2,intensity);
			var newColor = new Color(RawColor.r * factor, RawColor.g * factor, RawColor.b * factor);
			_disc.Color = newColor;
		}
	}
}
