namespace SoulsLike
{
	public readonly struct EnemyDied : IGameEvent
	{
		public readonly int enemyID;

		public EnemyDied(int enemyID) => this.enemyID = enemyID;
	}
}