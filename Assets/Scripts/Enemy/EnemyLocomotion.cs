using UnityEngine;

namespace SoulsLike
{
	public class EnemyLocomotion : MonoBehaviour
	{
		[SerializeField] private EnemyMovementData _enemyMovementData = default;

		private UnitStats _currentTarget = default;
		private Transform _myTransform = default;

		private void Awake() => _myTransform = transform;

		public void HandleDetection()
		{
			var collidersBuff = new Collider[_enemyMovementData.maxDetectionTargets];
			int buffSize = Physics.OverlapSphereNonAlloc(_myTransform.position, _enemyMovementData.detectionRadius, collidersBuff,
			                                             _enemyMovementData.detectionLayer);
			for(int i = 0; i < buffSize; i++)
			{
				if(!collidersBuff[i].TryGetComponent(out UnitStats unitStats)) continue;

				Vector3 targetDir = unitStats.transform.position - _myTransform.position;
				float viewAngle = Vector3.Angle(targetDir, _myTransform.forward);

				if(viewAngle > -_enemyMovementData.maxDetectionAngle && viewAngle < _enemyMovementData.maxDetectionAngle) _currentTarget = unitStats;
			}
		}

		public bool HasTarget() => _currentTarget;
	}
}