using SoulsLike.Extentions;

namespace SoulsLike
{
	public class PlayerStats : UnitStats, IEventListener, IEventSender
	{
		private void OnEnable() => this.AddListener<StaminaDrain>(OnStaminaDrain);

		private void OnDisable() => this.RemoveListener<StaminaDrain>(OnStaminaDrain);

		public void Init()
		{
			InitStats();

			this.TriggerEvent(new PlayerHealthInitEvent(unitStatsData.maxHealth));
			this.TriggerEvent(new StaminaInitEvent(unitStatsData.maxStamina));
		}

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);
			this.TriggerEvent(new PlayerHealthChanged(unitStatsData.currentHealth));

			if(unitStatsData.currentHealth > 0) return;

			unitStatsData.currentHealth = 0;
			this.TriggerEvent(new PlayerDied());

			//Handle Player Death
		}

		private void OnStaminaDrain(StaminaDrain eventInfo)
		{
			unitStatsData.currentStamina -= eventInfo.drainDamage;
			this.TriggerEvent(new StaminaChanged(unitStatsData.currentStamina));
		}
	}
}