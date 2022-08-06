namespace SoulsLike
{
	public class PlayerStats : Stats
	{
		private UIManager _uiManager = default;
		private AnimatorHandler _animatorHandler = default;

		public void Init(UIManager uIManager, AnimatorHandler animatorHandler)
		{
			_animatorHandler = animatorHandler;
			_uiManager = uIManager;

			SetStat(out maxHealth, out currentHealth, healthLevel, healthMultiplier);
			SetStat(out maxStamina, out currentStamina, staminaLevel, staminaMultiplier);

			_uiManager.SetMaxHealth(maxHealth);
			_uiManager.SetMaxStamina(maxStamina);
		}

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);
			_uiManager.SetCurrentHealth(currentHealth);

			_animatorHandler.PlayTargetAnimation(AnimationNameBase.DamageTaken, true);

			if(currentHealth <= 0)
			{
				currentHealth = 0;
				_animatorHandler.PlayTargetAnimation(AnimationNameBase.Death, true);

				//Handle Player Death
			}
		}

		public void StaminaDrain(int drainValue)
		{
			currentStamina -= drainValue;
			_uiManager.SetCurrentStamina(currentStamina);
		}
	}
}
