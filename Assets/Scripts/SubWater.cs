using UnityEngine;

namespace LD48Project {
	[ExecuteInEditMode]
	public sealed class SubWater : MonoBehaviour {
		[Range(0f, 1f)]
		public float Level;
		public float          MaxHeight = 3.5f;
		public SpriteRenderer SpriteRenderer;

		void Update() {
			if ( !SpriteRenderer ) {
				return;
			}
			SpriteRenderer.size = new Vector2(SpriteRenderer.size.x, MaxHeight * Level);
		}
	}
}
