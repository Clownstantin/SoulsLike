using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(EnemyCombatSystem), typeof(EnemyLocomotion), typeof(EnemyStats))]
	public class EnemyManager : UnitManager
	{
		private EnemyStats _enemyStats = default;
		private EnemyLocomotion _enemyLocomotion = default;
		// private EnemyCombatSystem _enemyCombatSystem = default;
		private EnemyAnimatorHandler _enemyAnimatorHandler = default;
		private EnemyStateManager _enemyStateManager = default;

		private void Awake()
		{
			_enemyStats = GetComponent<EnemyStats>();
			_enemyLocomotion = GetComponent<EnemyLocomotion>();
			//_enemyCombatSystem = GetComponent<EnemyCombatSystem>();
			_enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
			_enemyStateManager = GetComponent<EnemyStateManager>();
		}

		protected override void OnStart()
		{
			_enemyStats.Init();
			_enemyLocomotion.Init();
			//_enemyCombatSystem.Init(_enemyLocomotion.StopDistance);
			_enemyAnimatorHandler.Init();
			_enemyStateManager.Init();
		}

		public override void OnUpdate(float delta) { }

		public override void OnFixedUpdate(float delta) => _enemyStateManager.UpdateState(delta);
	}
}