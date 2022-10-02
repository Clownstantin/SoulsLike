using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(EnemyStats))]
	public class EnemyManager : UnitManager
	{
		private Animator _animator = default;

		private EnemyLocomotion _enemyLocomotion = default;
		private EnemyStats _enemyStats = default;

		private bool _isPerformingAction = default;

		private void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
			_enemyStats = GetComponent<EnemyStats>();
			_enemyLocomotion = GetComponent<EnemyLocomotion>();
		}

		public override void OnUpdate(float delta) => HandleCurrentAction();

		private void Start() => _enemyStats.Init(_animator);

		private void HandleCurrentAction()
		{
			if(!_enemyLocomotion.HasTarget()) _enemyLocomotion.HandleDetection();
		}
	}
}