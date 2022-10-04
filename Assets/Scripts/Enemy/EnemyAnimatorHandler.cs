using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(Animator))]
	public class EnemyAnimatorHandler : AnimatorHandler
	{
		private void OnEnable() => Subscribe();

		private void OnDisable() => Unsubscribe();

		private void OnAnimatorMove()
		{
			float delta = Time.deltaTime;
			Vector3 deltaPosition = animator.deltaPosition;
			deltaPosition.y = 0;
			Vector3 velocity = deltaPosition / delta;
			this.TriggerEvent(new EnemyAnimationPlay(velocity));
		}

		private void OnHealthChanged(EnemyHealthChanged eventInfo)
		{
			if(eventInfo.enemyID != transform.parent.GetInstanceID()) return;
			PlayTargetAnimation(AnimationNameBase.DamageTaken, true);
		}

		private void OnEnemyMove(EnemyMoveEvent eventInfo) => animator.SetFloat(verticalHash, 1, 0.1f, eventInfo.delta);

		private void OnEnemyStop(EnemyStopEvent eventInfo) => animator.SetFloat(verticalHash, 0, 0.1f, eventInfo.delta);

		private void OnDie(EnemyDied eventInfo)
		{
			if(eventInfo.enemyID != transform.parent.GetInstanceID()) return;
			PlayTargetAnimation(AnimationNameBase.Death, true);
		}

		private void Subscribe()
		{
			this.AddListener<EnemyHealthChanged>(OnHealthChanged);
			this.AddListener<EnemyStopEvent>(OnEnemyStop);
			this.AddListener<EnemyMoveEvent>(OnEnemyMove);
			this.AddListener<EnemyDied>(OnDie);
		}

		private void Unsubscribe()
		{
			this.RemoveListener<EnemyHealthChanged>(OnHealthChanged);
			this.RemoveListener<EnemyStopEvent>(OnEnemyStop);
			this.RemoveListener<EnemyMoveEvent>(OnEnemyMove);
			this.RemoveListener<EnemyDied>(OnDie);
		}
	}
}