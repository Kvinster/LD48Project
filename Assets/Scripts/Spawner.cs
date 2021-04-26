using UnityEngine;

namespace LD48Project {
	public sealed class Spawner : MonoBehaviour {
		[Header("Parameters")]
		public float StartSpawnPeriod;
		public float SpawnPeriodDecStep;
		public int   StartSpawnBurstSize;
		public float SpawnBurstIncSpeed;
		[Space]
		public float SpawnDistance;
		[Header("Dependencies")]
		public Transform  SpawnCenter;
		public Transform  SpawnParent;
		public GameObject Prefab;

		float _spawnPeriod;
		float _spawnBurstSize;

		float _timer;

		bool IsActive => Submarine.Instance && Submarine.Instance.IsAlive;

		void Start() {
			_spawnPeriod    = StartSpawnPeriod;
			_spawnBurstSize = StartSpawnBurstSize;

			_timer = _spawnPeriod;
		}

		void Update() {
			if ( !IsActive ) {
				return;
			}

			_timer -= Time.deltaTime;
			if ( _timer <= 0f ) {
				Spawn();

				_spawnPeriod    -= SpawnPeriodDecStep * Random.Range(0.9f, 1.1f);
				_spawnBurstSize *= SpawnBurstIncSpeed * Random.Range(0.9f, 1.1f);

				_timer = _spawnPeriod;
			}
		}

		void Spawn() {
			for ( var i = 0; i < Mathf.FloorToInt(_spawnBurstSize); ++i ) {
				var pos = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward) * Vector2.up *
				          SpawnDistance + SpawnCenter.position;
				Instantiate(Prefab, pos, Quaternion.identity, SpawnParent);
			}
		}
	}
}
