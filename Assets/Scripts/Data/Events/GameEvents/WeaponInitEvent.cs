namespace SoulsLike
{
	public struct WeaponInitEvent : IGameEvent
	{
		public readonly WeaponItem rightWeapon;
		public readonly WeaponItem leftWeapon;

		public WeaponInitEvent(WeaponItem rightWeapon, WeaponItem leftWeapon)
		{
			this.rightWeapon = rightWeapon;
			this.leftWeapon = leftWeapon;
		}
	}
}