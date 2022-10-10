namespace SoulsLike
{
	public readonly struct WeaponLoadEvent : IGameEvent
	{
		public readonly WeaponItem weapon;
		public readonly bool isLeftSlot;
		public readonly bool isBackSLot;

		public WeaponLoadEvent(WeaponItem weapon, bool isLeftSlot, bool isBackSLot = false)
		{
			this.weapon = weapon;
			this.isLeftSlot = isLeftSlot;
			this.isBackSLot = isBackSLot;
		}
	}
}