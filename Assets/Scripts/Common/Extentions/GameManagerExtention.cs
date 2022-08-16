using System;
using UnityEngine;

namespace SoulsLike.Extentions
{
	public static class GameManagerExtention
	{
		public static void RegisterUpdatableObject(this Behaviour _, IUpdateable obj) =>
			GameManager.Instance.RegisterUpdatableObject(obj);

		public static void UnregisterUpdatableObject(this Behaviour _, IUpdateable obj) =>
			GameManager.Instance.UnregisterUpdatableObject(obj);

		public static void AddListener(this Behaviour _, EventID eventID, Action<object> callback) =>
			GameManager.Instance.EventManager.AddListener(eventID, callback);

		public static void TriggerEvent(this Behaviour _, EventID eventID, object param) =>
			GameManager.Instance.EventManager.TriggerEvent(eventID, param);

		public static void TriggerEvent(this Behaviour _, EventID eventID) =>
			GameManager.Instance.EventManager.TriggerEvent(eventID);

		public static void RemoveListener(this Behaviour _, EventID eventID, Action<object> callback) =>
			GameManager.Instance.EventManager.RemoveListener(eventID, callback);
	}
}
