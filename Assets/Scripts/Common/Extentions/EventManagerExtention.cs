﻿using System;
using UnityEngine;

namespace SoulsLike.Extentions
{
	public static class EventManagerExtention
	{
		public static void AddListener<T>(this Behaviour _, Action<T> callback) where T : IGameEvent =>
			GameManager.Instance.EventManager.AddListener(callback);

		public static void TriggerEvent(this Behaviour _, IGameEvent gameEvent) =>
			GameManager.Instance.EventManager.TriggerEvent(gameEvent);

		public static void RemoveListener<T>(this Behaviour _, Action<T> callback) where T : IGameEvent =>
			GameManager.Instance.EventManager.RemoveListener(callback);

		public static void AddListener<T>(this IView _, Action<T> callback) where T : IGameEvent =>
			GameManager.Instance.EventManager.AddListener(callback);

		public static void TriggerEvent(this IView _, IGameEvent gameEvent) =>
			GameManager.Instance.EventManager.TriggerEvent(gameEvent);

		public static void RemoveListener<T>(this IView _, Action<T> callback) where T : IGameEvent =>
			GameManager.Instance.EventManager.RemoveListener(callback);
	}
}