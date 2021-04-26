using UnityEngine;

namespace LD48Project {
	public sealed class Bullet : MonoBehaviour {
		public const float Damage = 1f;

		const float Lifetime = 5f;

		float _lifeTimer;

		void Update() {
			_lifeTimer += Time.deltaTime;
			if ( _lifeTimer >= Lifetime ) {
				Destroy(gameObject);
			}
		}

		void OnCollisionEnter2D(Collision2D other) {
			Destroy(gameObject);
		}
	}
}
