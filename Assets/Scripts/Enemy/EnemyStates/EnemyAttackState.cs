using System;

namespace SoulsLike.EnemyStates
{
	public struct EnemyAttackState : IEnemyState
	{
		private EnemyStateManager _stateManager;

		public EnemyAttackState(EnemyStateManager stateManager) => _stateManager = stateManager;

		public void UpdateState(float delta) => throw new NotImplementedException();
	}
}