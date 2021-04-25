using UnityEngine;

namespace LD48Project {
	[RequireComponent(typeof(SpriteRenderer))]
	[ExecuteInEditMode]
	public sealed class HdrSpriteHelper : MonoBehaviour {
		static readonly int ColorId = Shader.PropertyToID("_Color");

		[ColorUsage(true, true)]
		public Color Color;

		SpriteRenderer _spriteRenderer;

		SpriteRenderer SpriteRenderer {
			get {
				if ( !_spriteRenderer || !Application.isPlaying ) {
					_spriteRenderer = GetComponent<SpriteRenderer>();
				}
				return _spriteRenderer;
			}
		}

		MaterialPropertyBlock _materialPropertyBlock;

		MaterialPropertyBlock MaterialPropertyBlock {
			get {
				if ( (_materialPropertyBlock == default) || !Application.isPlaying ) {
					_materialPropertyBlock = new MaterialPropertyBlock();
					SpriteRenderer.GetPropertyBlock(_materialPropertyBlock);
				}
				return _materialPropertyBlock;
			}
		}

		void Update() {
			MaterialPropertyBlock.SetColor(ColorId, Color);
			SpriteRenderer.SetPropertyBlock(MaterialPropertyBlock);
		}
	}
}
