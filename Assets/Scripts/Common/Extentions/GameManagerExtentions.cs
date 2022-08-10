using System;
using UnityEngine;

namespace SoulsLike.Extentions
{
	public static class GameManagerExtentions
	{
		public static void RegisterUpdatableObject(this MonoBehaviour _, IUpdateable obj) =>
			GameManager.Instance.RegisterUpdatableObject(obj);

		public static void UnregisterUpdatableObject(this MonoBehaviour _, IUpdateable obj) =>
			GameManager.Instance.UnregisterUpdatableObject(obj);

		public static void AddListener(this MonoBehaviour _, EventID eventID, Action<object> callback) =>
			GameManager.Instance.EventManager.AddListener(eventID, callback);

		public static void TriggerEvent(this MonoBehaviour _, EventID eventID, object param) =>
			GameManager.Instance.EventManager.TriggerEvent(eventID, param);

		public static void TriggerEvent(this MonoBehaviour _, EventID eventID) =>
			GameManager.Instance.EventManager.TriggerEvent(eventID);

		public static void RemoveListener(this MonoBehaviour _, EventID eventID, Action<object> callback) =>
			GameManager.Instance.EventManager.RemoveListener(eventID, callback);
	}
}
