using SoulsLike.EnemyStates;
using UnityEngine;
using UnityEngine.AI;

namespace SoulsLike
{
	[RequireComponent(typeof(Rigidbody))]
	public class EnemyStateManager : MonoBehaviour
	{
		[SerializeField] private EnemyConfig _enemyConfig = default;

		private Rigidbody _rigidbody = default;
		private NavMeshAgent _navMeshAgent = default;

		private IEnemyState _currentState = default;
		private int _enemyID = default;

		public bool isPerformingAction = default;

		public EnemyConfig EnemyConfig => _enemyConfig;
		public Rigidbody Rigidbody => _rigidbody;
		public NavMeshAgent NavMeshAgent => _navMeshAgent;

		public int EnemyID => _enemyID;

		public void Init()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_navMeshAgent = GetComponentInChildren<NavMeshAgent>();

			_enemyID = transform.GetInstanceID();
			_currentState = new EnemyIdleState(this);
		}

		public void UpdateState(float delta) => _currentState?.UpdateState(delta);

		public void SwitchState(IEnemyState state) => _currentState = state;
	}
}