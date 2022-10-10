namespace SoulsLike
{
	public readonly struct EnemyHealthChanged : IGameEvent
	{
		public readonly int enemyID;

		public EnemyHealthChanged(int enemyID) => this.enemyID = enemyID;
	}
}