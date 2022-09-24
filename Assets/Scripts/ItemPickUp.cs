using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public class ItemPickUp : Interactable
	{
		[Header("")]
		[SerializeField] private WeaponItem _item = default;

		public override void PickUp()
		{
			this.TriggerEvent(new PickUpWeaponEvent(_item));
			this.TriggerEvent(new ItemTextPopUp(_item, true));
			Destroy(gameObject);
		}
	}
}
