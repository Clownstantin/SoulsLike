using SoulsLike.Extentions;

namespace SoulsLike
{
	public class PlayerStats : Stats
	{
		public void Init()
		{
			InitStats();

			this.TriggerEvent(EventID.OnHealthInit, unitData.maxHealth);
			this.TriggerEvent(EventID.OnStaminaInit, unitData.maxStamina);
		}

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);
			this.TriggerEvent(EventID.OnHealthChanged, unitData.currentHealth);

			if(unitData.currentHealth <= 0)
			{
				unitData.currentHealth = 0;
				this.TriggerEvent(EventID.OnPlayerDeath);

				//Handle Player Death
			}
		}

		public override void StaminaDrain(int drainValue)
		{
			unitData.currentStamina -= drainValue;
			this.TriggerEvent(EventID.OnStaminaChanged, unitData.currentStamina);
		}
	}
}
