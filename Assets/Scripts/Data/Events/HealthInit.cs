namespace SoulsLike
{
	public struct HealthInit : IGameEvent
	{
		public int health;

		public void Init(int maxHealth) => health = maxHealth;
	}
}
