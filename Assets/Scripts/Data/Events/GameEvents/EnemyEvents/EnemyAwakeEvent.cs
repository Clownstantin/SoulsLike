namespace SoulsLike
{
	public readonly struct EnemyAwakeEvent : IGameEvent
	{
		public readonly int enemyID;

		public EnemyAwakeEvent(int enemyID) => this.enemyID = enemyID;
	}
}