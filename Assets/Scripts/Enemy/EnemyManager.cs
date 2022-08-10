using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(EnemyStats))]
	public class EnemyManager : MonoBehaviour
	{
		private Animator _animator;
		private EnemyStats _enemyStats;

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
