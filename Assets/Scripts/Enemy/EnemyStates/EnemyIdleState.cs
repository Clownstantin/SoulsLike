using UnityEngine;

namespace SoulsLike.Enemy
{
	public sealed class EnemyIdleState : EnemyState
	{
		private readonly EnemyConfig _config;
		private readonly Transform _myTransform;
		private readonly Collider[] _collidersBuff;

		public EnemyIdleState(EnemyStateManager stateManager, EnemyStateFactory factory) : base(stateManager, factory)
		{
			_myTransform = stateManager.transform;
			_config = stateManager.EnemyConfig;
			_collidersBuff = new Collider[_config.MaxDetectionTargets];
		}

		public override void UpdateState(float delta)
		{
			if(stateManager.CurrentTarget) return;
			int buffSize = Physics.OverlapSphereNonAlloc(_myTransform.position, _config.DetectionRadius, _collidersBuff, _config.DetectionLayer);
			
			for(int i = 0; i < buffSize; i++)
			{
				if(!_collidersBuff[i].TryGetComponent(out UnitStats unitStats)) continue;
				Vector3 targetDir = (unitStats.transform.position - _myTransform.position).normalized;
				float viewAngle = Vector3.Angle(targetDir, _myTransform.forward);

				if(viewAngle > -_config.MaxDetectionAngle && viewAngle < _config.MaxDetectionAngle)
				{
					stateManager.SetCurrentTarget(unitStats);
					SwitchState(factory.Pursue());
					return;
				}
			}
		}
	}
}