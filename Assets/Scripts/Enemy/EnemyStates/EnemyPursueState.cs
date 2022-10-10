using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.AI;

namespace SoulsLike.Enemy
{
	public sealed class EnemyPursueState : EnemyState, IEventSender
	{
		private readonly Transform _myTransform;
		private readonly Rigidbody _rigidbody;
		private readonly NavMeshAgent _navMesh;
		private readonly EnemyConfig _config;

		public EnemyPursueState(EnemyStateManager stateManager, EnemyStateFactory factory) : base(stateManager, factory)
		{
			_navMesh = stateManager.GetComponentInChildren<NavMeshAgent>();
			_myTransform = stateManager.transform;
			_rigidbody = stateManager.Rigidbody;
			_config = stateManager.EnemyConfig;

			_navMesh.enabled = false;
			_navMesh.stoppingDistance = _config.MaxAttackRange;
			_rigidbody.isKinematic = false;
		}

		public override void UpdateState(float delta)
		{
			if(!stateManager.CurrentTarget) return;
			Vector3 targetPos = stateManager.CurrentTarget.transform.position;
			float distanceFromTarget = Vector3.Distance(targetPos, _myTransform.position);
			Quaternion lookRot;

			if(stateManager.IsPerformingAction)
			{
				this.TriggerEvent(new EnemyStopEvent(stateManager.EnemyID, delta));
				_navMesh.enabled = false;

				Vector3 dir = (targetPos - _myTransform.position).normalized;
				dir.y = 0;
				dir = dir == Vector3.zero ? _myTransform.forward : dir;
				lookRot = Quaternion.LookRotation(dir);
			}
			else
			{
				_navMesh.enabled = true;
				lookRot = _navMesh.transform.rotation;
				_navMesh.SetDestination(targetPos);
				_rigidbody.velocity = _navMesh.velocity;
			}

			_myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, lookRot, _config.RotationSpeed * delta);

			Transform navMeshTransform = _navMesh.transform;
			navMeshTransform.localPosition = Vector3.zero;
			navMeshTransform.localRotation = Quaternion.identity;

			if(distanceFromTarget <= _config.MaxAttackRange) SwitchState(factory.CombatStance());
			else this.TriggerEvent(new EnemyMoveEvent(stateManager.EnemyID, delta));
		}
	}
}