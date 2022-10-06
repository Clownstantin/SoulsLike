namespace SoulsLike
{
	public struct EnemyMoveEvent : IGameEvent
	{
		public readonly int enemyID;
		public readonly float delta;

		public EnemyMoveEvent(int enemyID, float delta)
		{
			this.enemyID = enemyID;
			this.delta = delta;
		}
	}
}