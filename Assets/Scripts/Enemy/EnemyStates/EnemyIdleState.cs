using UnityEngine;

namespace SoulsLike.EnemyStates
{
	public struct EnemyIdleState : IEnemyState
	{
		private readonly EnemyStateManager _stateManager;
		private readonly EnemyConfig _config;
		private readonly Transform _myTransform;
		private readonly Collider[] _collidersBuff;

		private UnitStats _currentTarget;

		public EnemyIdleState(EnemyStateManager stateManager)
		{
			_currentTarget = null;
			_stateManager = stateManager;
			_myTransform = _stateManager.transform;
			_config = _stateManager.EnemyConfig;

			_collidersBuff = new Collider[_config.MaxDetectionTargets];
		}

		public void UpdateState(float delta)
		{
			if(_currentTarget) return;
			int buffSize = Physics.OverlapSphereNonAlloc(_myTransform.position, _config.DetectionRadius, _collidersBuff,
			                                             _config.DetectionLayer);
			for(int i = 0; i < buffSize; i++)
			{
				if(!_collidersBuff[i].TryGetComponent(out UnitStats unitStats)) continue;
				Vector3 targetDir = (unitStats.transform.position - _myTransform.position).normalized;
				float viewAngle = Vector3.Angle(targetDir, _myTransform.forward);

				if(viewAngle > -_config.MaxDetectionAngle && viewAngle < _config.MaxDetectionAngle)
				{
					_currentTarget = unitStats;
					_stateManager.SwitchState(new EnemyPursueState(_stateManager, _currentTarget));
					return;
				}
			}
		}
	}
}