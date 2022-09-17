using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(EnemyStats))]
	public class EnemyManager : UnitManager
	{
		private Animator _animator = default;
		private EnemyStats _enemyStats = default;

		private void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
			_enemyStats = GetComponent<EnemyStats>();
		}

		public override void OnUpdate(float delta)
		{
		}

		private void Start() => _enemyStats.Init(_animator);
	}
}
