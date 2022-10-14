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
			this.AddListener<EnemySleepEvent>(OnEnemySleep);
			this.AddListener<EnemyAwakeEvent>(OnEnemyAwake);
			this.AddListener<EnemyDied>(OnDie);
		}

		private void OnDisable()
		{
			this.RemoveListener<EnemyHealthChanged>(OnHealthChanged);
			this.RemoveListener<EnemyStopEvent>(OnEnemyStop);
			this.RemoveListener<EnemyMoveEvent>(OnEnemyMove);
			this.RemoveListener<EnemyAttackEvent>(OnEnemyAttack);
			this.RemoveListener<EnemySleepEvent>(OnEnemySleep);
			this.RemoveListener<EnemyAwakeEvent>(OnEnemyAwake);
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

		public bool GetIsInteracting() => animator.GetBool(isInteractingHash);

		private void OnHealthChanged(EnemyHealthChanged eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			PlayTargetAnimation(AnimationNameBase.DamageTaken, true);
		}

		private void OnEnemyMove(EnemyMoveEvent eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			animator.SetFloat(verticalHash, 1);
		}

		private void OnEnemyStop(EnemyStopEvent eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			animator.SetFloat(verticalHash, 0);
		}

		private void OnEnemyAttack(EnemyAttackEvent eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			PlayTargetAnimation(eventInfo.attackAction.ActionAnimation, true);
		}

		private void OnEnemySleep(EnemySleepEvent eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			PlayTargetAnimation(AnimationNameBase.Sleep, true);
		}

		private void OnEnemyAwake(EnemyAwakeEvent eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			PlayTargetAnimation(AnimationNameBase.GetUp, true);
		}

		private void OnDie(EnemyDied eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			PlayTargetAnimation(AnimationNameBase.Death, true);
		}
	}
}