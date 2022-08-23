namespace SoulsLike
{
	public struct StaminaDrain : IGameEvent
	{
		public readonly int drainDamage;

		public StaminaDrain(int drainDamage) => this.drainDamage = drainDamage;
	}
}