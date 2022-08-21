using UnityEngine;

namespace SoulsLike.Extentions
{
	public static class GameManagerExtention
	{
		public static void RegisterUpdatableObject(this Behaviour _, UpdateableComponent obj) =>
			GameManager.Instance.RegisterUpdatableObject(obj);

		public static void UnregisterUpdatableObject(this Behaviour _, UpdateableComponent obj) =>
			GameManager.Instance.UnregisterUpdatableObject(obj);
	}
}
