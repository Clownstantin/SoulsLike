using SoulsLike.Enemy;

namespace SoulsLike
{
	public abstract class EnemyState : IEnemyState
	{
		protected readonly EnemyStateManager stateManager = default;
		protected readonly EnemyStateFactory factory = default;

		protected EnemyState(EnemyStateManager stateManager, EnemyStateFactory factory)
		{
			this.stateManager = stateManager;
			this.factory = factory;
		}

		public abstract void UpdateState(float delta);

		protected void SwitchState(IEnemyState newState) => stateManager.SetCurrentState(newState);
	}
}