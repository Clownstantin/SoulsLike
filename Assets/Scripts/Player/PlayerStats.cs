using SoulsLike.Extentions;

namespace SoulsLike.Player
{
	public class PlayerStats : UnitStats, IEventListener, IEventSender
	{
		private void OnEnable() => this.AddListener<StaminaDrain>(OnStaminaDrain);

		private void OnDisable() => this.RemoveListener<StaminaDrain>(OnStaminaDrain);

		public override void Init()
		{
			base.Init();

			this.TriggerEvent(new PlayerHealthInitEvent(unitStatsData.maxHealth));
			this.TriggerEvent(new StaminaInitEvent(unitStatsData.maxStamina));
		}

		public override void TakeDamage(int damage)
		{
			if(isDead) return;
			base.TakeDamage(damage);
			this.TriggerEvent(new PlayerHealthChanged(unitStatsData.currentHealth));

			if(unitStatsData.currentHealth > 0) return;

			unitStatsData.currentHealth = 0;
			isDead = true;
			this.TriggerEvent(new PlayerDied());
		}

		private void OnStaminaDrain(StaminaDrain eventInfo)
		{
			unitStatsData.currentStamina -= eventInfo.drainDamage;
			this.TriggerEvent(new StaminaChanged(unitStatsData.currentStamina));
		}
	}
}