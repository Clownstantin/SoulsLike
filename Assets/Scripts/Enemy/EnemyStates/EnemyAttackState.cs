using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike.Enemy
{
	public sealed class EnemyAttackState : EnemyState, IEventSender
	{
		private readonly Transform _myTransform;
		private readonly EnemyAttackAction[] _enemyAttacks;

		private EnemyAttackAction _currentAttack;

		public EnemyAttackState(EnemyStateManager stateManager, EnemyStateFactory factory) : base(stateManager, factory)
		{
			_myTransform = stateManager.transform;
			_enemyAttacks = stateManager.EnemyConfig.AttackActions;
		}

		public override void UpdateState(float delta)
		{
			if(stateManager.IsPerformingAction) return;
			Vector3 targetPos = stateManager.CurrentTarget.transform.position;
			Vector3 myPos = _myTransform.position;
			Vector3 dirToTarget = (targetPos - myPos).normalized;
			float distanceToTarget = Vector3.Distance(targetPos, myPos);
			float viewAngle = Vector3.Angle(dirToTarget, _myTransform.forward);

			int maxScore = GetMaxScore(distanceToTarget, viewAngle);
			_currentAttack = GetRandomAttack(maxScore, distanceToTarget);

			if(!_currentAttack) return;
			this.TriggerEvent(new EnemyAttackEvent(stateManager.EnemyID, _currentAttack));
			stateManager.SetIsPerformingAction(true);
			stateManager.SetRecoveryTime(_currentAttack.RecoveryTime);

			SwitchState(factory.CombatStance());
		}

		private EnemyAttackAction GetRandomAttack(int maxScore, float distanceToTarget)
		{
			int randomScore = Random.Range(0, maxScore);
			int tempScore = 0;

			for(int i = 0; i < _enemyAttacks.Length; i++)
			{
				if(distanceToTarget <= _enemyAttacks[i].MaxDistanceToAttack && distanceToTarget >= _enemyAttacks[i].MinDistanceToAttack)
				{
					tempScore += _enemyAttacks[i].AttackScore;
					if(tempScore > randomScore)
						return _enemyAttacks[i];
				}
			}
			return null;
		}

		private int GetMaxScore(float distanceToTarget, float viewAngle)
		{
			int score = 0;
			for(int i = 0; i < _enemyAttacks.Length; i++)
			{
				if(distanceToTarget <= _enemyAttacks[i].MaxDistanceToAttack && distanceToTarget >= _enemyAttacks[i].MinDistanceToAttack &&
				   viewAngle <= _enemyAttacks[i].MaxAttackAngle && viewAngle >= -_enemyAttacks[i].MaxAttackAngle)
					score += _enemyAttacks[i].AttackScore;
			}
			return score;
		}
	}
}