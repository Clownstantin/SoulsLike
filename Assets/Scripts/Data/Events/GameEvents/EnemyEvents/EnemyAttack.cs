namespace SoulsLike
{
	public struct EnemyAttack : IGameEvent
	{
		public readonly int enemyID;
		public readonly EnemyAttackAction attackAction;

		public EnemyAttack(int enemyID, EnemyAttackAction attackAction)
		{
			this.enemyID = enemyID;
			this.attackAction = attackAction;
		}
	}
}