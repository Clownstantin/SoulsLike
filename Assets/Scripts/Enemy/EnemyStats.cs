using SoulsLike.Extentions;

namespace SoulsLike
{
	public class EnemyStats : UnitStats
	{
		public void Init() => InitStats();

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);
			int enemyID = transform.GetInstanceID();
			this.TriggerEvent(new EnemyHealthChanged(enemyID));

			if(unitStatsData.currentHealth > 0) return;
			unitStatsData.currentHealth = 0;
			this.TriggerEvent(new EnemyDied(enemyID));
			//Handle Enemy Death
		}
	}
}