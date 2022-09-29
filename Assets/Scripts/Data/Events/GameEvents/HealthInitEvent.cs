namespace SoulsLike
{
	public struct HealthInitEvent : IGameEvent
	{
		public readonly int health;

		public HealthInitEvent(int maxHealth) => health = maxHealth;
	}
}
