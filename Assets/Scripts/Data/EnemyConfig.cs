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
		[SerializeField] private float _stopDistance = default;
		[SerializeField] private float _rotationSpeed = default;

		public int MaxDetectionTargets => _maxDetectionTargets;
		public float DetectionRadius => _detectionRadius;
		public LayerMask DetectionLayer => _detectionLayer;
		public float MaxDetectionAngle => _maxDetectionAngle;
		public float StopDistance => _stopDistance;
		public float RotationSpeed => _rotationSpeed;
	}
}