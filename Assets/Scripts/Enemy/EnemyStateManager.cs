using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.AI;

namespace SoulsLike.Enemy
{
	[RequireComponent(typeof(Rigidbody))]
	public class EnemyStateManager : MonoBehaviour, IEventListener, IEventSender
	{
		[SerializeField] private EnemyConfig _enemyConfig = default;

		private EnemyStateFactory _stateFactory = default;
		private IEnemyState _currentState = default;

		private Rigidbody _rigidbody = default;
		private NavMeshAgent _navMesh = default;
		private UnitStats _currentTarget = default;

		private int _enemyID = default;
		private bool _isPerformingAction = default;
		private bool _isInteracting = default;
		private float _recoveryTime;

		public EnemyConfig EnemyConfig => _enemyConfig;
		public UnitStats CurrentTarget => _currentTarget;
		public Rigidbody Rigidbody => _rigidbody;
		public NavMeshAgent NavMesh => _navMesh;

		public bool IsPerformingAction => _isPerformingAction;
		public bool IsInteracting => _isInteracting;
		public int EnemyID => _enemyID;
		public float RecoveryTime => _recoveryTime;

		private void OnEnable() => this.AddListener<EnemyAnimationPlay>(OnAnimationPlay);

		private void OnDisable() => this.RemoveListener<EnemyAnimationPlay>(OnAnimationPlay);

		public void Init()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_navMesh = GetComponentInChildren<NavMeshAgent>();

			_enemyID = transform.GetInstanceID();
			_stateFactory = new EnemyStateFactory(this);
			_currentState = _stateFactory.Ambush();
		}

		public void UpdateStates(float delta)
		{
			if(_recoveryTime > 0) _recoveryTime -= delta;
			if(_isPerformingAction && _recoveryTime <= 0) _isPerformingAction = false;

			_currentState?.UpdateState(delta);
			Debug.Log($"Current state {_currentState?.GetType().Name}");
		}

		public void SetCurrentState(IEnemyState state) => _currentState = state;

		public void SetCurrentTarget(UnitStats target) => _currentTarget = target;

		public void SetIsPerformingAction(bool isPerformingAction) => _isPerformingAction = isPerformingAction;

		public void SetIsInteracting(bool isInteracting) => _isInteracting = isInteracting;

		public void SetRecoveryTime(float recoveryTime) => _recoveryTime = recoveryTime;

		private void OnAnimationPlay(EnemyAnimationPlay eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			_rigidbody.drag = 0;
			_rigidbody.velocity = eventInfo.velocity;
		}
	}
}