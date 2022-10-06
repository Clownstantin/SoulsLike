namespace SoulsLike
{
	public struct EnemyTargetFound : IGameEvent
	{
		public readonly int enemyID;
		public readonly UnitStats currentTarget;

		public EnemyTargetFound(int enemyID, UnitStats currentTarget)
		{
			this.enemyID = enemyID;
			this.currentTarget = currentTarget;
		}
	}
}