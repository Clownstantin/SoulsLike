namespace SoulsLike
{
	public readonly struct EnemyStopEvent : IGameEvent
	{
		public readonly int enemyID;

		public EnemyStopEvent(int enemyID) => this.enemyID = enemyID;
	}
}