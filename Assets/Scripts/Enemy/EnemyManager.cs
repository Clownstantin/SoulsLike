using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(EnemyCombatSystem), typeof(EnemyLocomotion), typeof(EnemyStats))]
	public class EnemyManager : UnitManager
	{
		private EnemyStats _enemyStats = default;
		private EnemyLocomotion _enemyLocomotion = default;
		private EnemyCombatSystem _enemyCombatSystem = default;
		private EnemyAnimatorHandler _enemyAnimatorHandler = default;

		private bool _isPerformingAction = default;

		private void Awake()
		{
			_enemyStats = GetComponent<EnemyStats>();
			_enemyLocomotion = GetComponent<EnemyLocomotion>();
			_enemyCombatSystem = GetComponent<EnemyCombatSystem>();
			_enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
		}

		protected override void OnStart()
		{
			_enemyStats.Init();
			_enemyLocomotion.Init();
			_enemyCombatSystem.Init(_enemyLocomotion.StopDistance);
			_enemyAnimatorHandler.Init();
		}

		public override void OnUpdate(float delta) { }

		public override void OnFixedUpdate(float delta) => HandleCurrentAction(delta);

		private void HandleCurrentAction(float delta)
		{
			_enemyLocomotion.HandleDetection();
			_enemyLocomotion.HandleMoveToTarget(_isPerformingAction, delta);
			_enemyCombatSystem.HandleEnemyAttack(delta, ref _isPerformingAction);
		}
	}
}