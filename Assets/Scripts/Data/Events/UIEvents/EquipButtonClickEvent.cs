using SoulsLike.UI;

namespace SoulsLike
{
	public struct EquipButtonClickEvent : IGameEvent
	{
		public readonly EquipmentSlotTypes slotType;

		public EquipButtonClickEvent(EquipmentSlotTypes slotType) => this.slotType = slotType;
	}
}