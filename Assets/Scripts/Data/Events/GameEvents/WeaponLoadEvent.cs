namespace SoulsLike
{
	public struct WeaponLoadEvent : IGameEvent
	{
		public readonly WeaponItem weapon;
		public readonly bool isLeft;

		public WeaponLoadEvent(WeaponItem weapon, bool isLeft)
		{
			this.weapon = weapon;
			this.isLeft = isLeft;
		}
	}
}