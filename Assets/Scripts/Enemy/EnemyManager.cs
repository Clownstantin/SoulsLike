using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(IUnitStats))]
	public class EnemyManager : MonoBehaviour
	{
		private Animator _animator = default;
		private EnemyStats _enemyStats = default;

		private void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
			_enemyStats = GetComponent<EnemyStats>();
		}

		private void Start()
		{
			_enemyStats.Init(_animator);
		}
	}
}
