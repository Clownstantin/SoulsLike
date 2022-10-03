using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.AI;

namespace SoulsLike
{
	public class EnemyLocomotion : MonoBehaviour
	{
		[SerializeField] private EnemyMovementData _enemyMovementData = default;

		private UnitStats _currentTarget = default;
		private NavMeshAgent _navMesh = default;
		private Transform _myTransform = default;

		public void Init()
		{
			_myTransform = transform;
			_navMesh = GetComponentInChildren<NavMeshAgent>();
		}

		public void HandleDetection()
		{
			var collidersBuff = new Collider[_enemyMovementData.maxDetectionTargets];
			int buffSize = Physics.OverlapSphereNonAlloc(_myTransform.position, _enemyMovementData.detectionRadius, collidersBuff,
			                                             _enemyMovementData.detectionLayer);
			for(int i = 0; i < buffSize; i++)
			{
				if(!collidersBuff[i].TryGetComponent(out UnitStats unitStats)) continue;

				float viewAngle = GetViewAngle(unitStats);
				if(viewAngle > -_enemyMovementData.maxDetectionAngle && viewAngle < _enemyMovementData.maxDetectionAngle)
					_currentTarget = unitStats;
			}
		}

		public void HandleMoveToTarget(float delta, bool isPerformingAction)
		{
			float viewAngle = GetViewAngle(_currentTarget);
			float distanceFromTarget = Vector3.Distance(_currentTarget.transform.position, _myTransform.position);

			if(isPerformingAction)
			{
				this.TriggerEvent(new EnemyStopEvent(delta));
				_navMesh.enabled = false;
			}
			else
			{
				if(distanceFromTarget > _enemyMovementData.stopDistance) this.TriggerEvent(new EnemyMoveEvent(delta));
			}
		}

		public bool HasTarget() => _currentTarget;

		private float GetViewAngle(Component target)
		{
			Vector3 targetDir = target.transform.position - _myTransform.position;
			return Vector3.Angle(targetDir, _myTransform.forward);
		}
	}
}