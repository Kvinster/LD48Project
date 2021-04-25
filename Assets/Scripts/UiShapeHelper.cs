using UnityEngine;
using UnityEngine.Assertions;

using Shapes;

namespace LD48Project {
	[ExecuteInEditMode]
	[RequireComponent(typeof(ShapeRenderer))]
	[RequireComponent(typeof(RectTransform))]
	public sealed class UiShapeHelper : MonoBehaviour {
		public bool AdjustSizeToTransform = true;

		void Start() {
			UpdateLayer();
		}

		void Update() {
			if ( !Application.isPlaying ) {
				UpdateLayer();

				if ( AdjustSizeToTransform ) {
					var rt        = GetComponent<RectTransform>();
					var rectangle = GetComponent<Rectangle>();
					if ( rectangle ) {
						rectangle.Width  = rt.sizeDelta.x;
						rectangle.Height = rt.sizeDelta.y;
					}
				}
			}
		}

		void UpdateLayer() {
			var sr = GetComponent<ShapeRenderer>();
			Assert.IsTrue(sr);
			sr.SortingLayerID = SortingLayer.NameToID("UI");
		}
	}
}
