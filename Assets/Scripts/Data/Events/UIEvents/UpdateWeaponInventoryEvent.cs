using System.Collections.Generic;

namespace SoulsLike
{
	public readonly struct UpdateWeaponInventoryEvent : IGameEvent
	{
		public readonly List<WeaponItem> itemInventory;
		public readonly WeaponItem[] rightHandWeapons;
		public readonly WeaponItem[] leftHandWeapons;

		public UpdateWeaponInventoryEvent(List<WeaponItem> weaponInventory, WeaponItem[] rightWeapons, WeaponItem[] leftWeapons)
		{
			itemInventory = weaponInventory;
			rightHandWeapons = rightWeapons;
			leftHandWeapons = leftWeapons;
		}
	}
}