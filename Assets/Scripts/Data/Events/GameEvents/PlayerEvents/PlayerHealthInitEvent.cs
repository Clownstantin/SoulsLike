namespace SoulsLike
{
	public readonly struct PlayerHealthInitEvent : IGameEvent
	{
		public readonly int health;

		public PlayerHealthInitEvent(int maxHealth) => health = maxHealth;
	}
}