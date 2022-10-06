using UnityEngine;

namespace SoulsLike
{
	[CreateAssetMenu(menuName = "Enemy Actions/Attack Action", fileName = "new AttackAction", order = 51)]
	public class EnemyAttackAction : EnemyAction
	{
		[SerializeField] private int _attackScore = default;
		[SerializeField] private float _recoveryTime = default;
		[SerializeField] private float _maxAttackAngle = default;
		[SerializeField] private float _maxDistanceToAttack = default;
		[SerializeField] private float _minDistanceToAttack = default;

		public int AttackScore => _attackScore;
		public float RecoveryTime => _recoveryTime;
		public float MaxAttackAngle => _maxAttackAngle;
		public float MaxDistanceToAttack => _maxDistanceToAttack;
		public float MinDistanceToAttack => _minDistanceToAttack;
	}
}