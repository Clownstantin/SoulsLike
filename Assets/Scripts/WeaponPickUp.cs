using System;
using UnityEngine;

namespace SoulsLike
{
	public class WeaponPickUp : Interactable
	{
		[Header("")]
		[SerializeField] private WeaponItem _weapon = default;

		public override void Interact(Action<WeaponItem> action) => action?.Invoke(_weapon);
	}
}
