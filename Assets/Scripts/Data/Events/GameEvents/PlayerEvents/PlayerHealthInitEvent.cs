namespace SoulsLike
{
	public struct PlayerHealthInitEvent : IGameEvent
	{
		public readonly int health;

		public PlayerHealthInitEvent(int maxHealth) => health = maxHealth;
	}
}