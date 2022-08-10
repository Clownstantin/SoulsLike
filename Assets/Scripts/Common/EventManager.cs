using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
	public class EventManager : MonoBehaviour
	{
		private Dictionary<EventID, Action<object>> _listeners = default;

		private void Awake() => _listeners = new Dictionary<EventID, Action<object>>();

		private void OnDestroy() => _listeners.Clear();

		public void AddListener(EventID eventID, Action<object> callback)
		{
			if(_listeners.ContainsKey(eventID)) _listeners[eventID] += callback;
			else
			{
				_listeners.Add(eventID, null);
				_listeners[eventID] += callback;
			}
		}

		public void TriggerEvent(EventID eventID, object param = null)
		{
			if(!_listeners.ContainsKey(eventID)) return;

			Action<object> callback = _listeners[eventID];

			if(callback != null) callback(param);
			else
			{
				Log.Send($"Callback doesn't exist {eventID}");
				_listeners.Remove(eventID);
			}
		}

		public void RemoveListener(EventID eventID, Action<object> callback)
		{
			if(_listeners.ContainsKey(eventID)) _listeners[eventID] -= callback;
			else Log.Send("Remove listener failed. Key not found.");
		}
	}
}
