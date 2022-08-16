namespace SoulsLike
{
	public struct HealthChanged : IGameEvent
	{
		public int currentHealth;

		public  void Init(int health) => currentHealth = health;
	}
}