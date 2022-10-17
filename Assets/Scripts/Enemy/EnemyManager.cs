using UnityEngine;

namespace SoulsLike.Enemy
{
	[RequireComponent(typeof(EnemyStats))]
	public class EnemyManager : UnitManager
	{
		private EnemyStats _enemyStats = default;
		private EnemyStateManager _stateManager = default;
		private EnemyWeaponSlotManager _weaponSlotManager = default;
		private EnemyAnimatorHandler _animatorHandler = default;

		private void Awake()
		{
			_enemyStats = GetComponent<EnemyStats>();
			_stateManager = GetComponent<EnemyStateManager>();
			_animatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
			_weaponSlotManager = GetComponentInChildren<EnemyWeaponSlotManager>();
		}

		protected override void OnStart()
		{
			_enemyStats.Init();
			_stateManager.Init();
			_animatorHandler.Init();
			_weaponSlotManager.Init();
		}

		public override void OnUpdate(float delta)
		{
			bool isInteracting = _animatorHandler.GetIsInteracting();
			_stateManager.SetIsInteracting(isInteracting);
		}

		public override void OnFixedUpdate(float delta) => _stateManager.UpdateStates(delta);
	}
}