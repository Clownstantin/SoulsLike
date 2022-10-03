using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(Animator))]
	public class EnemyAnimatorHandler : AnimatorHandler
	{
		private void OnEnable()
		{
			this.AddListener<EnemyHealthChanged>(OnHealthChanged);
			this.AddListener<EnemyStopEvent>(OnEnemyStop);
			this.AddListener<EnemyDied>(OnDie);
		}

		private void OnDisable()
		{
			this.RemoveListener<EnemyHealthChanged>(OnHealthChanged);
			this.RemoveListener<EnemyStopEvent>(OnEnemyStop);
			this.RemoveListener<EnemyDied>(OnDie);
		}

		private void OnHealthChanged(EnemyHealthChanged eventInfo)
		{
			if(eventInfo.enemyID != transform.parent.GetInstanceID()) return;
			PlayTargetAnimation(AnimationNameBase.DamageTaken, true);
		}

		private void OnEnemyStop(EnemyStopEvent eventInfo) => animator.SetFloat(AnimatorParameterBase.Vertical, 0, 0.1f, eventInfo.delta);

		private void OnDie(EnemyDied eventInfo)
		{
			if(eventInfo.enemyID != transform.parent.GetInstanceID()) return;
			PlayTargetAnimation(AnimationNameBase.Death, true);
		}
	}
}