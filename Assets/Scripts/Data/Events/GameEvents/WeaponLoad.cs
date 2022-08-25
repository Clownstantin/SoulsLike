namespace SoulsLike
{
	public struct WeaponLoad : IGameEvent
	{
		public readonly WeaponItem weapon;
		public readonly bool isLeft;

		public WeaponLoad(WeaponItem weapon, bool isLeft)
		{
			this.weapon = weapon;
			this.isLeft = isLeft;
		}
	}
}