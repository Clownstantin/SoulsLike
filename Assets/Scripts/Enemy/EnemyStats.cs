using UnityEngine;

namespace SoulsLike
{
	public class EnemyStats : Stats
	{
		private Animator _animator = default;

		public void Init(Animator animator)
		{
			_animator = animator;
			InitStats();
		}

		public override void TakeDamage(int damage)
		{
			base.TakeDamage(damage);

			_animator.Play(AnimationNameBase.DamageTaken);

			if(unitData.currentHealth <= 0)
			{
				unitData.currentHealth = 0;
				_animator.Play(AnimationNameBase.Death);

				//Handle Enemy Death
			}
		}
	}
}
