using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike.Enemy
{
	public sealed class EnemyCombatStanceState : EnemyState, IEventSender
	{
		private readonly Transform _myTransform;
		private readonly EnemyConfig _config;

		public EnemyCombatStanceState(EnemyStateManager stateManager, EnemyStateFactory factory) : base(stateManager, factory)
		{
			_myTransform = stateManager.transform;
			_config = stateManager.EnemyConfig;
		}

		public override void UpdateState(float delta)
		{
			Vector3 targetPos = stateManager.CurrentTarget.transform.position;
			Vector3 myPos = _myTransform.position;
			float distanceToTarget = Vector3.Distance(myPos, targetPos);

			Vector3 dir = (targetPos - myPos).normalized;
			dir.y = 0;
			dir = dir == Vector3.zero ? _myTransform.forward : dir;
			_myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, Quaternion.LookRotation(dir), _config.RotationSpeed * delta);

			if(stateManager.IsPerformingAction) this.TriggerEvent(new EnemyStopEvent(stateManager.EnemyID));

#warning TODO Circle Around Player

			if(stateManager.RecoveryTime <= 0 && distanceToTarget <= _config.MaxAttackRange) SwitchState(factory.Attack());
			else if(stateManager.RecoveryTime <= 0 && distanceToTarget > _config.MaxAttackRange) SwitchState(factory.Pursue());
		}
	}
}