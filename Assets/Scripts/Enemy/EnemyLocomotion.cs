using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.AI;

namespace SoulsLike
{
	[RequireComponent(typeof(Rigidbody))]
	public class EnemyLocomotion : MonoBehaviour
	{
		[SerializeField] private EnemyMovementData _enemyMovementData = default;

		private UnitStats _currentTarget = default;
		private NavMeshAgent _navMesh = default;

		private Transform _myTransform = default;
		private Rigidbody _rigidbody = default;

		private void OnEnable() => this.AddListener<EnemyAnimationPlay>(OnAnimationPlay);

		private void OnDisable() => this.RemoveListener<EnemyAnimationPlay>(OnAnimationPlay);

		public void Init()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_navMesh = GetComponentInChildren<NavMeshAgent>();

			_myTransform = transform;
			_navMesh.enabled = false;
			_navMesh.stoppingDistance = _enemyMovementData.stopDistance;
			_rigidbody.isKinematic = false;
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

		public void HandleMoveToTarget(bool isPerformingAction)
		{
			float delta = Time.deltaTime;
			float distanceFromTarget = Vector3.Distance(_currentTarget.transform.position, _myTransform.position);

			if(isPerformingAction)
			{
				this.TriggerEvent(new EnemyStopEvent(delta));
				_navMesh.enabled = false;
			}
			else
			{
				if(distanceFromTarget > _enemyMovementData.stopDistance)
					this.TriggerEvent(new EnemyMoveEvent(delta));
				else
					this.TriggerEvent(new EnemyStopEvent(delta));
			}
			_navMesh.transform.localPosition = Vector3.zero;
			HandleRotation(delta, isPerformingAction);
		}

		public bool HasTarget() => _currentTarget;

		private void HandleRotation(float delta, bool isPerformingAction)
		{
			float rotSpeed = _enemyMovementData.rotationSpeed;

			if(isPerformingAction)
			{
				Vector3 dir = _currentTarget.transform.position - _myTransform.position;
				dir.y = 0;
				dir.Normalize();

				if(dir == Vector3.zero) dir = _myTransform.forward;

				Quaternion lookRotation = Quaternion.LookRotation(dir);
				_myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, lookRotation, rotSpeed * delta);
			}
			else
			{
				_navMesh.enabled = true;
				_navMesh.SetDestination(_currentTarget.transform.position);
				_rigidbody.velocity = _navMesh.velocity;
				_myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, _navMesh.transform.rotation, rotSpeed * delta);
			}
			_navMesh.transform.localRotation = Quaternion.identity;
		}

		private void OnAnimationPlay(EnemyAnimationPlay eventInfo)
		{
			_rigidbody.drag = 0;
			_rigidbody.velocity = eventInfo.velocity;
		}

		private float GetViewAngle(Component target)
		{
			Vector3 targetDir = target.transform.position - _myTransform.position;
			return Vector3.Angle(targetDir, _myTransform.forward);
		}
	}
}