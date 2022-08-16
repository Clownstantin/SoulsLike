using SoulsLike.Extentions;

namespace SoulsLike
{
	public class PlayerStats : Stats
	{
		public void Init()
		{
			InitStats();

			TriggerInitHealthEvent();
			TriggerInitStaminaEvent();
		}

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);
			TriggerHealthChangedEvent();

			if(unitData.currentHealth > 0) return;

			unitData.currentHealth = 0;

			PlayerDied playerDied = Instance<PlayerDied>.value;
			this.TriggerEvent(playerDied);

			//Handle Player Death
		}

		public override void StaminaDrain(int drainValue)
		{
			unitData.currentStamina -= drainValue;
			TriggerStaminaChangedEvent();
		}

		private void TriggerInitStaminaEvent()
		{
			StaminaInit staminaInit = Instance<StaminaInit>.value;
			staminaInit.Init(unitData.maxStamina);
			this.TriggerEvent(staminaInit);
		}

		private void TriggerInitHealthEvent()
		{
			HealthInit healthInit = Instance<HealthInit>.value;
			healthInit.Init(unitData.maxHealth);
			this.TriggerEvent(healthInit);
		}

		private void TriggerHealthChangedEvent()
		{
			HealthChanged healthChanged = Instance<HealthChanged>.value;
			healthChanged.Init(unitData.currentHealth);
			this.TriggerEvent(healthChanged);
		}

		private void TriggerStaminaChangedEvent()
		{
			StaminaChanged staminaChanged = Instance<StaminaChanged>.value;
			staminaChanged.Init(unitData.currentStamina);
			this.TriggerEvent(staminaChanged);
		}
	}
}
