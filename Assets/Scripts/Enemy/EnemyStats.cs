using SoulsLike.Extentions;

namespace SoulsLike.Enemy
{
	public class EnemyStats : UnitStats, IEventSender
	{
		private int _enemyID = default;

		public override void Init()
		{
			base.Init();
			_enemyID = transform.GetInstanceID();
		}

		public override void TakeDamage(int damage)
		{
			if(isDead) return;
			base.TakeDamage(damage);
			this.TriggerEvent(new EnemyHealthChanged(_enemyID));

			if(unitStatsData.currentHealth > 0) return;
			unitStatsData.currentHealth = 0;
			isDead = true;
			this.TriggerEvent(new EnemyDied(_enemyID));
		}
	}
}