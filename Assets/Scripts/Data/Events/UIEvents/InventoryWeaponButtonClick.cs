﻿using SoulsLike.UI;

namespace SoulsLike
{
	public readonly struct InventoryWeaponButtonClick : IGameEvent
	{
		public readonly EquipmentSlotTypes slotType;
		public readonly WeaponItem weapon;

		public InventoryWeaponButtonClick(EquipmentSlotTypes slotType, WeaponItem weapon)
		{
			this.slotType = slotType;
			this.weapon = weapon;
		}
	}
}