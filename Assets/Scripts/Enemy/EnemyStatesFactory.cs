using System.Collections.Generic;

namespace SoulsLike.Enemy
{
	public readonly struct EnemyStateFactory
	{
		private enum EnemyStates
		{
			Idle,
			Pursue,
			CombatStance,
			Attack,
			Length
		}

		private readonly Dictionary<EnemyStates, IEnemyState> _states;

		public EnemyStateFactory(EnemyStateManager stateManager)
		{
			_states = new Dictionary<EnemyStates, IEnemyState>((int)EnemyStates.Length);
			_states[EnemyStates.Idle] = new EnemyIdleState(stateManager, this);
			_states[EnemyStates.Pursue] = new EnemyPursueState(stateManager, this);
			_states[EnemyStates.CombatStance] = new EnemyCombatStanceState(stateManager, this);
			_states[EnemyStates.Attack] = new EnemyAttackState(stateManager, this);
		}

		public IEnemyState Idle() => _states[EnemyStates.Idle];

		public IEnemyState Pursue() => _states[EnemyStates.Pursue];

		public IEnemyState CombatStance() => _states[EnemyStates.CombatStance];

		public IEnemyState Attack() => _states[EnemyStates.Attack];
	}
}