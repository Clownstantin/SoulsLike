using UnityEngine;

namespace SoulsLike
{
	public class EnemyStats : Stats
	{
		private Animator _animator = default;

		public void Init(Animator animator)
		{
			_animator = animator;

			SetStat(out maxHealth, out currentHealth, healthLevel, healthMultiplier);
			SetStat(out maxStamina, out currentStamina, staminaLevel, staminaMultiplier);
		}

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);

			_animator.Play(AnimationNameBase.DamageTaken);

			if(currentHealth <= 0)
			{
				currentHealth = 0;
				_animator.Play(AnimationNameBase.Death);

				//Handle Enemy Death
			}
		}
	}
}
