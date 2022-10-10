using UnityEngine;

namespace SoulsLike.Enemy
{
	[RequireComponent(typeof(EnemyStats))]
	public class EnemyManager : UnitManager
	{
		private EnemyStats _enemyStats = default;
		private EnemyStateManager _enemyStateManager = default;
		private EnemyAnimatorHandler _enemyAnimatorHandler = default;

		private void Awake()
		{
			_enemyStats = GetComponent<EnemyStats>();
			_enemyStateManager = GetComponent<EnemyStateManager>();
			_enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
		}

		protected override void OnStart()
		{
			_enemyStats.Init();
			_enemyStateManager.Init();
			_enemyAnimatorHandler.Init();
		}

		public override void OnUpdate(float delta) { }

		public override void OnFixedUpdate(float delta) => _enemyStateManager.UpdateStates(delta);
	}
}