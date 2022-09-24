using System.Collections.Generic;

namespace SoulsLike
{
	public struct UpdateWeaponInventoryEvent : IGameEvent
	{
		public readonly List<WeaponItem> itemInventory;
		public readonly WeaponItem[] rightHandWeapons;
		public readonly WeaponItem[] leftHandWeapons;

		public UpdateWeaponInventoryEvent(List<WeaponItem> weaponInventory, WeaponItem[] rightWeapons, WeaponItem[] leftWeapons)
		{
			this.itemInventory = weaponInventory;
			rightHandWeapons = rightWeapons;
			leftHandWeapons = leftWeapons;
		}
	}
}