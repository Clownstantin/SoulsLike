using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike.Enemy
{
	public sealed class EnemyAttackState : EnemyState, IEventSender
	{
		private readonly Transform _myTransform;
		private readonly EnemyAttackAction[] _enemyAttacks;

		private EnemyAttackAction _currentAttack;
		private float _recoveryTime;

		public EnemyAttackState(EnemyStateManager stateManager, EnemyStateFactory factory) : base(stateManager, factory)
		{
			_myTransform = stateManager.transform;
			_enemyAttacks = stateManager.EnemyConfig.AttackActions;
		}

		public override void UpdateState(float delta)
		{
			if(stateManager.IsPerformingAction && _recoveryTime <= 0)
			{
				stateManager.SetIsPerformingAction(false);
				SwitchState(factory.CombatStance());
			}

			if(_recoveryTime > 0) _recoveryTime -= delta;

			float distanceToTarget = Vector3.Distance(stateManager.CurrentTarget.transform.position, _myTransform.position);
			this.TriggerEvent(new EnemyStopEvent(stateManager.EnemyID, delta));

			_currentAttack = GetRandomAttack(distanceToTarget);

			if(!_currentAttack || _recoveryTime > 0) return;
			this.TriggerEvent(new EnemyAttackEvent(stateManager.EnemyID, _currentAttack));
			stateManager.SetIsPerformingAction(true);
			_recoveryTime = _currentAttack.RecoveryTime;
		}

		private EnemyAttackAction GetRandomAttack(float distanceToTarget)
		{
			Vector3 targetDir = (stateManager.CurrentTarget.transform.position - _myTransform.position).normalized;
			float viewAngle = Vector3.Angle(targetDir, _myTransform.forward);

			int maxScore = GetMaxScore(distanceToTarget, viewAngle);
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