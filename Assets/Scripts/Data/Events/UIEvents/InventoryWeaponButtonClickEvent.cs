using SoulsLike.UI;

namespace SoulsLike
{
	public struct InventoryWeaponButtonClickEvent : IGameEvent
	{
		public readonly EquipmentSlotTypes slotType;
		public readonly WeaponItem weapon;

		public InventoryWeaponButtonClickEvent(EquipmentSlotTypes slotType, WeaponItem weapon)
		{
			this.slotType = slotType;
			this.weapon = weapon;
		}
	}
}