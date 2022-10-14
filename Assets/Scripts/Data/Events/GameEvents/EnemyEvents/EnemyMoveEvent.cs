namespace SoulsLike
{
	public readonly struct EnemyMoveEvent : IGameEvent
	{
		public readonly int enemyID;

		public EnemyMoveEvent(int enemyID, float delta) => this.enemyID = enemyID;
	}
}