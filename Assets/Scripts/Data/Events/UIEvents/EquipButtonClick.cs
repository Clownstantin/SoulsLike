using SoulsLike.UI;

namespace SoulsLike
{
	public readonly struct EquipButtonClick : IGameEvent
	{
		public readonly EquipmentSlotTypes slotType;

		public EquipButtonClick(EquipmentSlotTypes slotType) => this.slotType = slotType;
	}
}