using UnityEngine;

namespace SoulsLike
{
	public class PlayerStats : MonoBehaviour
	{
		private AnimatorHandler _animatorHandler;

		public int healthLevel = 10;
		public int maxHealth;
		public int currentHealth;

		public HealthBar healthBar;

		public void Init(AnimatorHandler animatorHandler)
		{
			_animatorHandler = animatorHandler;

			maxHealth = SetMaxHealthFromHealthLevel();
			currentHealth = maxHealth;
			healthBar.SetMaxHealth(maxHealth);
		}

		public void TakeDamage(int damage)
		{
			currentHealth -= damage;
			healthBar.SetCurrentHealth(currentHealth);

			_animatorHandler.PlayTargetAnimation(AnimationNameBase.DamageTaken, true);

			if(currentHealth <= 0)
			{
				currentHealth = 0;
				_animatorHandler.PlayTargetAnimation(AnimationNameBase.Death, true);

				//Handle Player Death
			}
		}

		private int SetMaxHealthFromHealthLevel() => healthLevel * 10;
	}
}
