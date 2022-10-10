namespace SoulsLike
{
	public readonly struct PlayerHealthChanged : IGameEvent
	{
		public readonly int currentHealth;

		public PlayerHealthChanged(int health) => currentHealth = health;
	}
}