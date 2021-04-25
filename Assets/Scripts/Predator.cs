using UnityEngine;

using Shapes;

namespace LD48Project {
	public sealed class Predator : MonoBehaviour {
		[Header("Head")]
		public Disc      HeadDisc;
		public Transform HeadTopTeethTransform;
		public Transform HeadBottomTeethTransform;
		public float     MaxAngle  = 120f;
		public float     MinAngle  = 4f;
		public float     TestAngle = 60f;

		float _curHeadAngle;
		float CurHeadAngle {
			get => _curHeadAngle;
			set {
				_curHeadAngle                          = Mathf.Clamp(value, MinAngle, MaxAngle);
				HeadDisc.AngRadiansStart               = Mathf.Deg2Rad * _curHeadAngle / 2f;
				HeadDisc.AngRadiansEnd                 = Mathf.Deg2Rad * (360f - _curHeadAngle / 2f);
				HeadTopTeethTransform.localRotation    = Quaternion.Euler(0f, 0f, _curHeadAngle / 2f);
				HeadBottomTeethTransform.localRotation = Quaternion.Euler(0f, 0f, -_curHeadAngle / 2f);
			}
		}

		void Update() {
			CurHeadAngle = TestAngle;
		}
	}
}
