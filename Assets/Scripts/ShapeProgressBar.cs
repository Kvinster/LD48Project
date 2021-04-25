using UnityEngine;

using Shapes;

namespace LD48Project {
	[ExecuteInEditMode]
	public sealed class ShapeProgressBar : MonoBehaviour {
		public float     MaxWidth;
		public Rectangle Foreground;
		[Space]
		[Range(0f, 1f)]
		public float EditorProgress;

		float _progress;
		public float Progress {
			get => _progress;
			set {
				_progress        = Mathf.Clamp01(value);
				Foreground.Width = MaxWidth * _progress;
			}
		}

		void Update() {
			if ( !Application.isPlaying && Foreground && (MaxWidth > 0f) ) {
				Progress = EditorProgress;
			}
		}
	}
}
