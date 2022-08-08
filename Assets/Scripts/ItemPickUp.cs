using System;
using UnityEngine;

namespace SoulsLike
{
	public class ItemPickUp : Interactable
	{
		[Header("")]
		[SerializeField] private Item _item = default;

		public override void PickUp(Action<Item> action) => action?.Invoke(_item);
	}
}
