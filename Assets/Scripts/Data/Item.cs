using UnityEngine;

namespace SoulsLike
{
	public abstract class Item : ScriptableObject
	{
		public Sprite itemIcon = default;
		public string itemName = default;
	}
}
