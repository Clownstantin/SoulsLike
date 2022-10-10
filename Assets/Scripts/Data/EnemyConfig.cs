using UnityEngine;

namespace SoulsLike
{
	[CreateAssetMenu(menuName = "Data/EnemyConfig", fileName = "new EnemyConfig", order = 51)]
	public class EnemyConfig : ScriptableObject
	{
		[Header("Detection Settings")]
		[SerializeField] private int _maxDetectionTargets = default;
		[SerializeField] private float _detectionRadius = default;
		[SerializeField] private LayerMask _detectionLayer = default;
		[SerializeField] private float _maxDetectionAngle = default;
		[Header("Movement Settings")]
		[SerializeField] private float _rotationSpeed = default;
		[Header("Combat Settings")]
		[SerializeField] private EnemyAttackAction[] _attackActions = default;
		[SerializeField] private float _maxAttackRange = default;

		public int MaxDetectionTargets => _maxDetectionTargets;
		public float DetectionRadius => _detectionRadius;
		public LayerMask DetectionLayer => _detectionLayer;
		public float MaxDetectionAngle => _maxDetectionAngle;

		public float RotationSpeed => _rotationSpeed;

		public EnemyAttackAction[] AttackActions => _attackActions;
		public float MaxAttackRange => _maxAttackRange;
	}
}