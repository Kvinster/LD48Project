using UnityEngine;
using UnityEngine.EventSystems;

using System;
using System.Collections.Generic;

using Shapes;

namespace LD48Project {
	public sealed class ShapeButton : MonoBehaviour {
		[Header("Parameters")]
		public Color NormalColor   = Color.white;
		public Color HoverColor    = Color.white;
		public Color PressedColor  = Color.white;
		public Color DisabledColor = Color.white;
		[Header("Dependencies")]
		public Rectangle    Rectangle;
		public Collider2D   Collider;
		public EventTrigger EventTrigger;

		bool _enabled = true;

		readonly List<Action> _clickActions = new List<Action>();

		public bool Enabled {
			get => _enabled;
			set {
				_enabled         = value;
				Collider.enabled = _enabled;
				Rectangle.Color  = _enabled ? NormalColor : DisabledColor;
			}
		}

		void Reset() {
			Rectangle    = GetComponentInChildren<Rectangle>();
			Collider     = GetComponentInChildren<Collider2D>();
			EventTrigger = GetComponentInChildren<EventTrigger>();
		}

		void Start() {
			AddTriggerEntry(EventTriggerType.PointerEnter, () => Rectangle.Color = HoverColor);
			AddTriggerEntry(EventTriggerType.PointerExit, () => Rectangle.Color  = NormalColor);
			AddTriggerEntry(EventTriggerType.PointerDown, () => Rectangle.Color  = PressedColor);
			AddTriggerEntry(EventTriggerType.PointerUp, () => Rectangle.Color    = NormalColor);
			AddTriggerEntry(EventTriggerType.PointerClick, OnClick);

			Enabled = _enabled;
		}

		public void AddClickAction(Action action) {
			if ( action == null ) {
				return;
			}
			if ( _clickActions.Contains(action) ) {
				Debug.LogWarningFormat("{0}.{1}: action is already registered", nameof(ShapeButton),
					nameof(AddClickAction));
				return;
			}
			_clickActions.Add(action);
		}

		public void RemoveClickAction(Action action) {
			_clickActions.Remove(action);
		}

		void OnClick() {
			if ( !Enabled ) {
				return;
			}
			foreach ( var action in _clickActions ) {
				action.Invoke();
			}
		}

		void AddTriggerEntry(EventTriggerType eventId, Action action, bool ignoreEnabled = false) {
			var entry = new EventTrigger.Entry { eventID = eventId };
			entry.callback.AddListener(_ => {
				if ( !ignoreEnabled && !Enabled ) {
					return;
				}
				action?.Invoke();
			});
			EventTrigger.triggers.Add(entry);
		}
	}
}
