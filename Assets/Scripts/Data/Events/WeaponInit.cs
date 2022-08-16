namespace SoulsLike
{
	public struct WeaponInit : IGameEvent
	{
		public WeaponItem rightWeapon;
		public WeaponItem leftWeapon;

		public void Init(WeaponItem rightWeapon, WeaponItem leftWeapon)
		{
			this.rightWeapon = rightWeapon;
			this.leftWeapon = leftWeapon;
		}
	}
}