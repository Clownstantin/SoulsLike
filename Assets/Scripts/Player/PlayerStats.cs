using SoulsLike.Extentions;

namespace SoulsLike
{
	public class PlayerStats : UnitStats
	{
		private void OnEnable() => this.AddListener<StaminaDrain>(OnStaminaDrain);

		private void OnDisable() => this.RemoveListener<StaminaDrain>(OnStaminaDrain);

		public void Init()
		{
			InitStats();

			this.TriggerEvent(new HealthInitEvent(_unitStatsData.maxHealth));
			this.TriggerEvent(new StaminaInitEvent(_unitStatsData.maxStamina));
		}

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);
			this.TriggerEvent(new HealthChanged(_unitStatsData.currentHealth));

			if(_unitStatsData.currentHealth > 0) return;

			_unitStatsData.currentHealth = 0;
			this.TriggerEvent(new PlayerDied());

			//Handle Player Death
		}

		private void OnStaminaDrain(StaminaDrain eventInfo)
		{
			_unitStatsData.currentStamina -= eventInfo.drainDamage;
			this.TriggerEvent(new StaminaChanged(_unitStatsData.currentStamina));
		}
	}
}