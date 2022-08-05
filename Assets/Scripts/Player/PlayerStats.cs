namespace SoulsLike
{
	public class PlayerStats : Stats
	{
		private AnimatorHandler _animatorHandler;

		public HealthBarUI healthBar;

		public void Init(AnimatorHandler animatorHandler)
		{
			_animatorHandler = animatorHandler;

			maxHealth = healthLevel * 10;
			currentHealth = maxHealth;
			healthBar.SetMaxHealth(maxHealth);
		}

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);
			healthBar.SetCurrentHealth(currentHealth);

			_animatorHandler.PlayTargetAnimation(AnimationNameBase.DamageTaken, true);

			if(currentHealth <= 0)
			{
				currentHealth = 0;
				_animatorHandler.PlayTargetAnimation(AnimationNameBase.Death, true);

				//Handle Player Death
			}
		}
	}
}
