using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike.Enemy
{
	[RequireComponent(typeof(Animator))]
	public class EnemyAnimatorHandler : AnimatorHandler, IEventListener, IEventSender
	{
		private int _enemyID = default;

		public override void Init()
		{
			base.Init();
			_enemyID = transform.parent.GetInstanceID();
		}

		private void OnEnable()
		{
			this.AddListener<EnemyHealthChanged>(OnHealthChanged);
			this.AddListener<EnemyStopEvent>(OnEnemyStop);
			this.AddListener<EnemyMoveEvent>(OnEnemyMove);
			this.AddListener<EnemyAttackEvent>(OnEnemyAttack);
			this.AddListener<EnemyDied>(OnDie);
		}

		private void OnDisable()
		{
			this.RemoveListener<EnemyHealthChanged>(OnHealthChanged);
			this.RemoveListener<EnemyStopEvent>(OnEnemyStop);
			this.RemoveListener<EnemyMoveEvent>(OnEnemyMove);
			this.RemoveListener<EnemyAttackEvent>(OnEnemyAttack);
			this.RemoveListener<EnemyDied>(OnDie);
		}

		private void OnAnimatorMove()
		{
			float delta = Time.deltaTime;
			Vector3 deltaPosition = animator.deltaPosition;
			deltaPosition.y = 0;
			Vector3 velocity = deltaPosition / delta;
			this.TriggerEvent(new EnemyAnimationPlay(_enemyID, velocity));
		}

		private void OnHealthChanged(EnemyHealthChanged eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			PlayTargetAnimation(AnimationNameBase.DamageTaken, true);
		}

		private void OnEnemyMove(EnemyMoveEvent eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			animator.SetFloat(verticalHash, 1, 0.1f, eventInfo.delta);
		}

		private void OnEnemyStop(EnemyStopEvent eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			animator.SetFloat(verticalHash, 0, 0.1f, eventInfo.delta);
			animator.SetFloat(horizontalHash, 0, 0.1f, eventInfo.delta);
		}

		private void OnEnemyAttack(EnemyAttackEvent eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			PlayTargetAnimation(eventInfo.attackAction.ActionAnimation, true);
		}

		private void OnDie(EnemyDied eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			PlayTargetAnimation(AnimationNameBase.Death, true);
		}
	}
}