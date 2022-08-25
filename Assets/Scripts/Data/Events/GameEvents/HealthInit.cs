namespace SoulsLike
{
	public struct HealthInit : IGameEvent
	{
		public readonly int health;

		public HealthInit(int maxHealth) => health = maxHealth;
	}
}
