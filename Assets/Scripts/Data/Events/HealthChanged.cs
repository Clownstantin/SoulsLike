namespace SoulsLike
{
	public struct HealthChanged : IGameEvent
	{
		public readonly int currentHealth;

		public HealthChanged(int health) => currentHealth = health;
	}
}