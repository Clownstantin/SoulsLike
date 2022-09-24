namespace SoulsLike
{
	public struct PickUpWeaponEvent : IGameEvent
	{
		public readonly WeaponItem weapon;

		public PickUpWeaponEvent(WeaponItem weapon) => this.weapon = weapon;
	}
}