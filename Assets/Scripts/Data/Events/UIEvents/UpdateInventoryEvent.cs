using System.Collections.Generic;

namespace SoulsLike
{
	public struct UpdateInventoryEvent : IGameEvent
	{
		public readonly List<Item> itemInventory;
		public readonly WeaponItem[] rightHandWeapons;
		public readonly WeaponItem[] leftHandWeapons;

		public UpdateInventoryEvent(List<Item> itemInventory, WeaponItem[] rightWeapons, WeaponItem[] leftWeapons)
		{
			this.itemInventory = itemInventory;
			rightHandWeapons = rightWeapons;
			leftHandWeapons = leftWeapons;
		}
	}
}