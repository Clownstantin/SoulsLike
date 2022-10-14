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
			float distanceToTarget = Vector3.Distance(_myTransform.position, targetPos);

			if(stateManager.IsPerformingAction) this.TriggerEvent(new EnemyStopEvent(stateManager.EnemyID));

#warning TODO Circle Around Player

			if(stateManager.RecoveryTime <= 0 && distanceToTarget <= _config.MaxAttackRange) SwitchState(factory.Attack());
			else if(distanceToTarget > _config.MaxAttackRange) SwitchState(factory.Pursue());
		}
	}
}