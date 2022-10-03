namespace SoulsLike
{
	public struct EnemyDied : IGameEvent
	{
		public readonly int enemyID;

		public EnemyDied(int enemyID) => this.enemyID = enemyID;
	}
}