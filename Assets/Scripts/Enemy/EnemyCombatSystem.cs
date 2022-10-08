using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public class EnemyCombatSystem : MonoBehaviour, IEventListener, IEventSender
	{
		[SerializeField] private EnemyAttackAction[] _enemyAttacks = default;

		private Transform _myTransform = default;
		private UnitStats _currentTarget = default;
		private EnemyAttackAction _currentAttack = default;

		private int _enemyId = default;

		private float _stopDistance = default;
		private float _distanceToTarget = default;
		private float _recoveryTime = default;

		private void OnEnable() => this.AddListener<EnemyTargetFound>(OnTargetFound);

		private void OnDisable() => this.RemoveListener<EnemyTargetFound>(OnTargetFound);

		public void Init(float stopDistance)
		{
			_myTransform = transform;
			_stopDistance = stopDistance;
			_enemyId = _myTransform.GetInstanceID();
		}

		public void HandleEnemyAttack(float delta, ref bool isPerformingAction)
		{
			if(!_currentTarget) return;
			_distanceToTarget = Vector3.Distance(_currentTarget.transform.position, _myTransform.position);
			HandleRecoveryTime(delta, ref isPerformingAction);

			if(isPerformingAction || _distanceToTarget > _stopDistance) return;
			_currentAttack = GetRandomAttack();

			if(!_currentAttack) return;
			this.TriggerEvent(new EnemyAttack(_enemyId, _currentAttack));
			isPerformingAction = true;
			_recoveryTime = _currentAttack.RecoveryTime;
		}

		private EnemyAttackAction GetRandomAttack()
		{
			Vector3 targetDir = (_currentTarget.transform.position - _myTransform.position).normalized;
			float viewAngle = Vector3.Angle(targetDir, _myTransform.forward);

			int maxScore = GetMaxScore(_distanceToTarget, viewAngle);
			int randomScore = Random.Range(0, maxScore);
			int tempScore = 0;

			for(int i = 0; i < _enemyAttacks.Length; i++)
			{
				if(_distanceToTarget <= _enemyAttacks[i].MaxDistanceToAttack && _distanceToTarget >= _enemyAttacks[i].MinDistanceToAttack)
				{
					tempScore += _enemyAttacks[i].AttackScore;
					if(tempScore > randomScore)
						return _enemyAttacks[i];
				}
			}
			return null;
		}

		private void HandleRecoveryTime(float delta, ref bool isPerformingAction)
		{
			if(_recoveryTime > 0) _recoveryTime -= delta;
			if(isPerformingAction && _recoveryTime <= 0) isPerformingAction = false;
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

		private void OnTargetFound(EnemyTargetFound eventInfo)
		{
			if(eventInfo.enemyID != _enemyId) return;
			_currentTarget = eventInfo.currentTarget;
		}
	}
}