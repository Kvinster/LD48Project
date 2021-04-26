using UnityEngine;
using UnityEngine.Assertions;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using LD48Project.Stations;

using DG.Tweening;

namespace LD48Project {
	public sealed class StationsManager : MonoBehaviour {
		sealed class Path {
			public readonly BaseStation       StartStation;
			public readonly BaseStation       EndStation;
			public readonly List<BaseStation> Nodes;

			float _length = float.MinValue;

			public float Length {
				get {
					if ( Mathf.Approximately(_length, float.MinValue) ) {
						var length = 0f;
						for ( var i = 1; i < Nodes.Count; ++i ) {
							length += Vector2.Distance(Nodes[i - 1].transform.position, Nodes[i].transform.position);
						}
						_length = length;
					}
					return _length;
				}
			}

			public Path Reversed {
				get {
					var nodes = new List<BaseStation>(Nodes.Count);
					for ( var i = 0; i < Nodes.Count; ++i ) {
						nodes.Add(Nodes[Nodes.Count - 1 - i]);
					}
					return new Path(nodes);
				}
			}

			public Path(List<BaseStation> nodes) {
				Nodes        = nodes;
				StartStation = Nodes[0];
				EndStation   = Nodes[Nodes.Count - 1];
			}
		}

		public static StationsManager Instance { get; private set; }

		public float             PlayerMovementSpeed = 1f;
		public Submarine         Submarine;
		public Transform         PlayerTransform;
		public BaseStation       StartStation;
		public List<BaseStation> Stations = new List<BaseStation>();

		readonly List<Path> _paths = new List<Path>();

		Sequence _activationAnim;

		public BaseStation ActiveStation { get; private set; }

		void Awake() {
			Assert.IsFalse(Instance);
			Instance = this;

			PreparePaths();
		}

		IEnumerator Start() {
			yield return null;
			ActiveStation = StartStation;
			ActiveStation.Activate();
		}

		public void TryActivateStation(BaseStation station) {
			if ( !Submarine.IsAlive || (ActiveStation == station) || (_activationAnim != null) ) {
				return;
			}
			_activationAnim?.Kill();
			var path          = GetPath(ActiveStation, station);
			var pathRaw       = path.Nodes.Select(x => x.transform.position).ToArray();
			var activeStation = ActiveStation;
			_activationAnim = DOTween.Sequence()
				.AppendCallback(() => activeStation.Deactivate())
				.Append(PlayerTransform.DOPath(pathRaw, path.Length / PlayerMovementSpeed).SetEase(Ease.Linear));
			_activationAnim.onComplete += () => {
				station.Activate();
				ActiveStation = station;
				_activationAnim = null;
			};
		}

		Path GetPath(BaseStation startStation, BaseStation endStation) {
			foreach ( var path in _paths ) {
				if ( (path.StartStation == startStation) && (path.EndStation == endStation) ) {
					return path;
				}
				if ( (path.StartStation == endStation) && (path.EndStation == startStation) ) {
					return path.Reversed;
				}
			}
			Debug.LogErrorFormat("{0}.{1}: no path found between '{2}' and '{3}'", nameof(StationsManager),
				nameof(GetPath), startStation.gameObject.name, endStation.gameObject.name);
			return null;
		}

		void PreparePaths() {
			foreach ( var aStation in Stations ) {
				foreach ( var bStation in Stations ) {
					if ( aStation == bStation ) {
						continue;
					}
					var path = CalcPath(aStation, bStation);
					if ( path != null ) {
						_paths.Add(path);
					}
				}
			}
		}

		// please god let no one see this code
		Path CalcPath(BaseStation startStation, BaseStation endStation) {
			Assert.IsNotNull(startStation);
			Assert.IsNotNull(endStation);
			Assert.AreNotEqual(startStation, endStation);
			Path result = null;

			void TryFindPathRecursive(BaseStation curNode, List<BaseStation> curPath) {
				curPath.Add(curNode);
				if ( curNode == endStation ) {
					result = new Path(curPath);
					return;
				}
				foreach ( var neighbour in curNode.Neighbours ) {
					if ( curPath.Contains(neighbour) ) {
						continue;
					}
					TryFindPathRecursive(neighbour, new List<BaseStation>(curPath));
					if ( result != null ) {
						return;
					}
				}
			}

			TryFindPathRecursive(startStation, new List<BaseStation>());
			Assert.IsNotNull(result);
			return result;
		}
	}
}
