using SoulsLike.Extentions;

namespace SoulsLike
{
	public class PlayerStats : Stats
	{
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

		public override void StaminaDrain(int drainValue)
		{
			unitData.currentStamina -= drainValue;
			this.TriggerEvent(new StaminaChanged(unitData.currentStamina));
		}
	}
}
