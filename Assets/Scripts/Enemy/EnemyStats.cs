using UnityEngine;

namespace SoulsLike
{
	public class EnemyStats : Stats
	{
		private Animator _animator;

		public void Init(Animator animator)
		{
			_animator = animator;

			maxHealth = healthLevel * 10;
			currentHealth = maxHealth;
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
