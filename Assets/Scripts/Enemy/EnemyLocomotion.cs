using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.AI;

namespace SoulsLike
{
	[RequireComponent(typeof(Rigidbody))]
	public class EnemyLocomotion : MonoBehaviour
	{
		[SerializeField] private EnemyMovementData _enemyMovementData = default;

		private Transform _myTransform = default;
		private Rigidbody _rigidbody = default;
		private NavMeshAgent _navMesh = default;
		private UnitStats _currentTarget = default;

		private int _enemyID = default;

		public UnitStats CurrentTarget => _currentTarget;
		public float StopDistance => _enemyMovementData.stopDistance;

		private void OnEnable() => this.AddListener<EnemyAnimationPlay>(OnAnimationPlay);

		private void OnDisable() => this.RemoveListener<EnemyAnimationPlay>(OnAnimationPlay);

		public void Init()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_navMesh = GetComponentInChildren<NavMeshAgent>();

			_myTransform = transform;
			_navMesh.enabled = false;
			_rigidbody.isKinematic = false;

			_navMesh.stoppingDistance = _enemyMovementData.stopDistance;
			_enemyID = _myTransform.GetInstanceID();
		}

		public void HandleDetection()
		{
			if(_currentTarget) return;
			var collidersBuff = new Collider[_enemyMovementData.maxDetectionTargets];
			int buffSize = Physics.OverlapSphereNonAlloc(_myTransform.position, _enemyMovementData.detectionRadius, collidersBuff,
			                                             _enemyMovementData.detectionLayer);
			for(int i = 0; i < buffSize; i++)
			{
				if(!collidersBuff[i].TryGetComponent(out UnitStats unitStats)) continue;
				Vector3 targetDir = unitStats.transform.position - _myTransform.position;
				float viewAngle = Vector3.Angle(targetDir, _myTransform.forward);
				
				if(viewAngle > -_enemyMovementData.maxDetectionAngle && viewAngle < _enemyMovementData.maxDetectionAngle)
				{
					_currentTarget = unitStats;
					this.TriggerEvent(new EnemyTargetFound(_enemyID, _currentTarget));
					return;
				}
			}
		}

		public void HandleMoveToTarget(bool isPerformingAction, float delta)
		{
			if(!_currentTarget) return;
			Vector3 targetPos = _currentTarget.transform.position;
			float distanceFromTarget = Vector3.Distance(targetPos, _myTransform.position);
			
			if(isPerformingAction)
			{
				this.TriggerEvent(new EnemyStopEvent(_enemyID, delta));
				_navMesh.enabled = false;
			}
			else
			{
				_navMesh.enabled = true;
				_navMesh.SetDestination(targetPos);
				_rigidbody.velocity = _navMesh.velocity;

				if(distanceFromTarget > _enemyMovementData.stopDistance) this.TriggerEvent(new EnemyMoveEvent(_enemyID, delta));
				else this.TriggerEvent(new EnemyStopEvent(_enemyID, delta));
			}
			_navMesh.transform.localPosition = Vector3.zero;
			HandleRotation(delta, isPerformingAction);
		}

		private void HandleRotation(float delta, bool isPerformingAction)
		{
			float rotSpeed = _enemyMovementData.rotationSpeed;

			if(isPerformingAction)
			{
				Vector3 dir = (_currentTarget.transform.position - _myTransform.position).normalized;
				dir.y = 0;
				dir = dir == Vector3.zero ? _myTransform.forward : dir;

				Quaternion lookRotation = Quaternion.LookRotation(dir);
				_myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, lookRotation, rotSpeed * delta);
			}
			else _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, _navMesh.transform.rotation, rotSpeed * delta);
			
			_navMesh.transform.localRotation = Quaternion.identity;
		}

		private void OnAnimationPlay(EnemyAnimationPlay eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			_rigidbody.drag = 0;
			_rigidbody.velocity = eventInfo.velocity;
		}
	}
}