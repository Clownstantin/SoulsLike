namespace SoulsLike
{
	public struct WeaponLoad : IGameEvent
	{
		public WeaponItem weapon;
		public bool isLeft;

		public void Init(WeaponItem weapon, bool isLeft)
		{
			this.weapon = weapon;
			this.isLeft = isLeft;
		}
	}
}