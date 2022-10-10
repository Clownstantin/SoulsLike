namespace SoulsLike
{
	public readonly struct PickUpEvent : IGameEvent
	{
		public readonly WeaponItem weapon;

		public PickUpEvent(WeaponItem weapon) => this.weapon = weapon;
	}
}