using UnityEngine;

namespace SoulsLike.Enemy
{
	public sealed class EnemyCombatStanceState : EnemyState
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
			if(!stateManager.CurrentTarget) return;
			Vector3 targetPos = stateManager.CurrentTarget.transform.position;
			float distanceToTarget = Vector3.Distance(_myTransform.position, targetPos);

#warning TODO Circle Around Player

			if(distanceToTarget <= _config.MaxAttackRange) SwitchState(factory.Attack());
			else if(distanceToTarget > _config.MaxAttackRange) SwitchState(factory.Pursue());
		}
	}
}