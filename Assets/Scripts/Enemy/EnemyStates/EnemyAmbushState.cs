using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike.Enemy
{
	public sealed class EnemyAmbushState : EnemyState, IEventSender
	{
		private readonly Transform _myTransform;
		private readonly EnemyConfig _config;
		private readonly Collider[] _collidersBuff;

		private bool _isSleeping;

		public EnemyAmbushState(EnemyStateManager stateManager, EnemyStateFactory factory) : base(stateManager, factory)
		{
			_myTransform = stateManager.transform;
			_config = stateManager.EnemyConfig;
			_isSleeping = true;
			_collidersBuff = new Collider[_config.MaxDetectionTargets];
		}

		public override void UpdateState(float delta)
		{
			if(_isSleeping && !stateManager.IsInteracting)
				this.TriggerEvent(new EnemySleepEvent(stateManager.EnemyID));

			int buffSize = Physics.OverlapSphereNonAlloc(_myTransform.transform.position, _config.AmbushRadius,
			                                             _collidersBuff, _config.DetectionLayer);
			for(int i = 0; i < buffSize; i++)
			{
				if(!_collidersBuff[i].TryGetComponent(out UnitStats unit)) continue;
				Vector3 dirToTarget = (unit.transform.position - _myTransform.position).normalized;
				float viewAngle = Vector3.Angle(dirToTarget, _myTransform.forward);

				if(viewAngle > -_config.MaxDetectionAngle && viewAngle < _config.MaxDetectionAngle)
				{
					stateManager.SetCurrentTarget(unit);
					_isSleeping = false;
					this.TriggerEvent(new EnemyAwakeEvent(stateManager.EnemyID));
					break;
				}
			}

			if(!stateManager.CurrentTarget || stateManager.IsInteracting) return;
			SwitchState(factory.Pursue());
		}
	}
}