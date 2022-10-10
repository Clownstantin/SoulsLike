namespace SoulsLike
{
	public readonly struct EnemyStopEvent : IGameEvent
	{
		public readonly int enemyID;
		public readonly float delta;

		public EnemyStopEvent(int enemyID, float delta)
		{
			this.enemyID = enemyID;
			this.delta = delta;
		}
	}
}