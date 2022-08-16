using System;
using System.Collections.Generic;

namespace SoulsLike
{
	public class EventManager : IDisposable
	{
		private Dictionary<Type, Action<IGameEvent>> _events = default;
		private Dictionary<Delegate, Action<IGameEvent>> _eventLookups = default;

		public EventManager()
		{
			_events = new Dictionary<Type, Action<IGameEvent>>();
			_eventLookups = new Dictionary<Delegate, Action<IGameEvent>>();
		}

		public void AddListener<T>(Action<T> callback) where T : IGameEvent
		{
			Type type = typeof(T);

			if(_eventLookups.ContainsKey(callback)) return;
			_eventLookups[callback] = OverrideAction;

			if(_events.TryGetValue(type, out Action<IGameEvent> internalAction))
				_events[type] = internalAction += OverrideAction;
			else
				_events[type] = OverrideAction;

			void OverrideAction(IGameEvent e) => callback((T)e);
		}

		public void TriggerEvent(IGameEvent gameEvent)
		{
			if(_events.TryGetValue(gameEvent.GetType(), out Action<IGameEvent> action))
				action?.Invoke(gameEvent);
		}

		public void RemoveListener<T>(Action<T> callback) where T : IGameEvent
		{
			if(_eventLookups.TryGetValue(callback, out Action<IGameEvent> action))
			{
				if(_events.TryGetValue(typeof(T), out Action<IGameEvent> tempAction))
				{
					tempAction -= action;

					if(tempAction == null) _events.Remove(typeof(T));
					else _events[typeof(T)] = tempAction;
				}

				_eventLookups.Remove(callback);
			}
		}

		public void Dispose()
		{
			_events.Clear();
			_eventLookups.Clear();
		}
	}
}
