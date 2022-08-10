using SoulsLike.Extentions;

namespace SoulsLike
{
	public class PlayerStats : Stats
	{
		private AnimatorHandler _animatorHandler = default;

		public void Init(AnimatorHandler animatorHandler)
		{
			_animatorHandler = animatorHandler;

			InitStats();

			this.TriggerEvent(EventID.OnHealthInit, unitData.maxHealth);
			this.TriggerEvent(EventID.OnStaminaInit, unitData.maxStamina);
		}

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);
			this.TriggerEvent(EventID.OnHealthChanged, unitData.currentHealth);

			_animatorHandler.PlayTargetAnimation(AnimationNameBase.DamageTaken, true);

			if(unitData.currentHealth <= 0)
			{
				unitData.currentHealth = 0;
				_animatorHandler.PlayTargetAnimation(AnimationNameBase.Death, true);

				//Handle Player Death
			}
		}

		public void StaminaDrain(int drainValue)
		{
			unitData.currentStamina -= drainValue;
			this.TriggerEvent(EventID.OnStaminaChanged, unitData.currentStamina);
		}
	}
}
