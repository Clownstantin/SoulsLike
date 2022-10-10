using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike.Enemy
{
	[RequireComponent(typeof(Rigidbody))]
	public class EnemyStateManager : MonoBehaviour, IEventListener
	{
		[SerializeField] private EnemyConfig _enemyConfig = default;

		private EnemyStateFactory _stateFactory = default;
		private IEnemyState _currentState = default;

		private Rigidbody _rigidbody = default;
		private UnitStats _currentTarget = default;

		private int _enemyID = default;
		private bool _isPerformingAction = default;

		public EnemyConfig EnemyConfig => _enemyConfig;
		public UnitStats CurrentTarget => _currentTarget;
		public Rigidbody Rigidbody => _rigidbody;

		public bool IsPerformingAction => _isPerformingAction;
		public int EnemyID => _enemyID;

		private void OnEnable() => this.AddListener<EnemyAnimationPlay>(OnAnimationPlay);

		private void OnDisable() => this.RemoveListener<EnemyAnimationPlay>(OnAnimationPlay);

		public void Init()
		{
			_rigidbody = GetComponent<Rigidbody>();

			_enemyID = transform.GetInstanceID();
			_stateFactory = new EnemyStateFactory(this);
			_currentState = _stateFactory.Idle();
		}

		public void UpdateStates(float delta)
		{
			_currentState?.UpdateState(delta);
			Debug.Log($"Current state {_currentState?.GetType().Name}");
		}

		public void SetCurrentState(IEnemyState state) => _currentState = state;

		public void SetCurrentTarget(UnitStats target) => _currentTarget = target;

		public void SetIsPerformingAction(bool isPerformingAction) => _isPerformingAction = isPerformingAction;

		private void OnAnimationPlay(EnemyAnimationPlay eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			_rigidbody.drag = 0;
			_rigidbody.velocity = eventInfo.velocity;
		}
	}
}