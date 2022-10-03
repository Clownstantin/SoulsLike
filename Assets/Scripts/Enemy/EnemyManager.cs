using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(EnemyLocomotion), typeof(EnemyStats))]
	public class EnemyManager : UnitManager
	{
		private EnemyAnimatorHandler _enemyAnimatorHandler = default;
		private EnemyLocomotion _enemyLocomotion = default;
		private EnemyStats _enemyStats = default;

		private bool _isPerformingAction = default;

		private void Awake()
		{
			_enemyStats = GetComponent<EnemyStats>();
			_enemyLocomotion = GetComponent<EnemyLocomotion>();
			_enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
		}

		protected override void OnStart()
		{
			_enemyStats.Init();
			_enemyAnimatorHandler.Init();
			_enemyLocomotion.Init();
		}

		public override void OnUpdate(float delta)
		{
			HandleCurrentAction();
		}

		private void HandleCurrentAction()
		{
			if(!_enemyLocomotion.HasTarget()) _enemyLocomotion.HandleDetection();
		}
	}
}