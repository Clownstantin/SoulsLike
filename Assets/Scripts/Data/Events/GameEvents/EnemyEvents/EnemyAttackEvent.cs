namespace SoulsLike
{
	public readonly struct EnemyAttackEvent : IGameEvent
	{
		public readonly int enemyID;
		public readonly EnemyAttackAction attackAction;

		public EnemyAttackEvent(int enemyID, EnemyAttackAction attackAction)
		{
			this.enemyID = enemyID;
			this.attackAction = attackAction;
		}
	}
}