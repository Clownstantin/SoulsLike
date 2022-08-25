namespace SoulsLike
{
	public struct WeaponInit : IGameEvent
	{
		public readonly WeaponItem rightWeapon;
		public readonly WeaponItem leftWeapon;

		public WeaponInit(WeaponItem rightWeapon, WeaponItem leftWeapon)
		{
			this.rightWeapon = rightWeapon;
			this.leftWeapon = leftWeapon;
		}
	}
}