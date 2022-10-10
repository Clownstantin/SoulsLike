namespace SoulsLike
{
	public readonly struct StaminaDrain : IGameEvent
	{
		public readonly int drainDamage;

		public StaminaDrain(int drainDamage) => this.drainDamage = drainDamage;
	}
}