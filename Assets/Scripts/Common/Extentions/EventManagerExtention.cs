using System;

namespace SoulsLike.Extentions
{
	public static class EventManagerExtention
	{
		public static void AddListener<T>(this IEventListener _, Action<T> callback) where T : IGameEvent =>
			GameManager.Instance.EventManager.AddListener(callback);

		public static void TriggerEvent(this IEventSender _, IGameEvent gameEvent) =>
			GameManager.Instance.EventManager.TriggerEvent(gameEvent);

		public static void RemoveListener<T>(this IEventListener _, Action<T> callback) where T : IGameEvent =>
			GameManager.Instance.EventManager.RemoveListener(callback);
	}
}