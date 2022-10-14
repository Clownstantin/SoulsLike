namespace SoulsLike
{
	public readonly struct EnemySleepEvent : IGameEvent
	{
		public readonly int enemyID;

		public EnemySleepEvent(int enemyID) => this.enemyID = enemyID;
	}
}