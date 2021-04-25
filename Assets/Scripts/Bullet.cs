using UnityEngine;

namespace LD48Project {
	public sealed class Bullet : MonoBehaviour {
		const float Lifetime = 5f;

		float _lifeTimer;

		void Update() {
			_lifeTimer += Time.deltaTime;
			if ( _lifeTimer >= Lifetime ) {
				Destroy(gameObject);
			}
		}
	}
}
