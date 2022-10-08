using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.AI;

namespace SoulsLike.EnemyStates
{
	public struct EnemyPursueState : IEnemyState, IEventSender
	{
		private EnemyStateManager _stateManager;
		private EnemyConfig _config;
		private UnitStats _currentTarget;

		private Transform _myTransform;
		private Rigidbody _rigidbody;
		private NavMeshAgent _navMesh;

		private readonly int _enemyID;

		public EnemyPursueState(EnemyStateManager stateManager, UnitStats currentTarget)
		{
			_stateManager = stateManager;
			_currentTarget = currentTarget;

			_config = _stateManager.EnemyConfig;
			_myTransform = _stateManager.transform;
			_enemyID = _stateManager.EnemyID;
			_rigidbody = _stateManager.Rigidbody;
			_navMesh = _stateManager.NavMeshAgent;

			_navMesh.enabled = false;
			_navMesh.stoppingDistance = _config.StopDistance;
			_rigidbody.isKinematic = false;
		}

		public void UpdateState(float delta)
		{
			if(!_currentTarget) return;
			Vector3 targetPos = _currentTarget.transform.position;
			float distanceFromTarget = Vector3.Distance(targetPos, _myTransform.position);

			if(_stateManager.isPerformingAction)
			{
				this.TriggerEvent(new EnemyStopEvent(_enemyID, delta));
				_navMesh.enabled = false;
			}
			else
			{
				_navMesh.enabled = true;
				_navMesh.SetDestination(targetPos);
				_rigidbody.velocity = _navMesh.velocity;

				if(distanceFromTarget > _config.StopDistance) this.TriggerEvent(new EnemyMoveEvent(_enemyID, delta));
				else
				{
					this.TriggerEvent(new EnemyStopEvent(_enemyID, delta));
					//_stateManager.SwitchState(new EnemyAttackState());
				}
			}
			_navMesh.transform.localPosition = Vector3.zero;
			HandleRotation(delta, _stateManager.isPerformingAction);
		}

		private void HandleRotation(float delta, bool isPerformingAction)
		{
			float rotSpeed = _config.RotationSpeed;

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
	}
}