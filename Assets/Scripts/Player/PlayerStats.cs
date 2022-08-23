using SoulsLike.Extentions;

namespace SoulsLike
{
	public class PlayerStats : Stats
	{
		private void OnEnable() => this.AddListener<StaminaDrain>(OnStaminaDrain);

		private void OnDisable() => this.RemoveListener<StaminaDrain>(OnStaminaDrain);

		public void Init()
		{
			InitStats();

			this.TriggerEvent(new HealthInit(unitData.maxHealth));
			this.TriggerEvent(new StaminaInit(unitData.maxStamina));
		}

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);
			this.TriggerEvent(new HealthChanged(unitData.currentHealth));

			if(unitData.currentHealth > 0) return;

			unitData.currentHealth = 0;
			this.TriggerEvent(new PlayerDied());

			//Handle Player Death
		}

		private void OnStaminaDrain(StaminaDrain eventInfo)
		{
			unitData.currentStamina -= eventInfo.drainDamage;
			this.TriggerEvent(new StaminaChanged(unitData.currentStamina));
		}
	}
}
