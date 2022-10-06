using UnityEngine;

namespace SoulsLike
{
	public abstract class Item : ScriptableObject
	{
		[SerializeField] private Sprite _itemIcon = default;
		[SerializeField] private string _itemName = default;

		public Sprite ItemIcon => _itemIcon;
		public string ItemName => _itemName;
	}
}
