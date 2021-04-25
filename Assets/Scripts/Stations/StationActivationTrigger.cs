using System;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace LD48Project.Stations {
	public sealed class StationActivationTrigger : MonoBehaviour {
		public EventTrigger EventTrigger;
		public GameObject   HoverGraphicRoot;

		BaseStation _station;

		void Start() {
			TryInitDisabled();
		}

		public void Init(BaseStation station) {
			Assert.IsTrue(station);

			_station = station;

			EventTrigger.enabled = true;
			HoverGraphicRoot.SetActive(false);

			EventTrigger.triggers.Clear();
			AddTriggerEntry(EventTriggerType.PointerEnter, () => HoverGraphicRoot.SetActive(true));
			AddTriggerEntry(EventTriggerType.PointerExit, () => HoverGraphicRoot.SetActive(false));
			AddTriggerEntry(EventTriggerType.PointerClick, () => {
				if ( !Input.GetMouseButtonUp(0) ) {
					return;
				}
				StationsManager.Instance.TryActivateStation(_station);
			});
		}

		void AddTriggerEntry(EventTriggerType eventId, Action action) {
			var entry = new EventTrigger.Entry { eventID = eventId };
			entry.callback.AddListener(_ => action?.Invoke());
			EventTrigger.triggers.Add(entry);
		}

		void TryInitDisabled() {
			if ( !_station ) {
				EventTrigger.enabled = false;
				HoverGraphicRoot.SetActive(false);
			}
		}
	}
}
