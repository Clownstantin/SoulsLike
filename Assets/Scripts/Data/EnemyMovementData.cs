using System;
using UnityEngine;

namespace SoulsLike
{
	[Serializable]
	public struct EnemyMovementData
	{
		[Header("Detection Settings")]
		public int maxDetectionTargets;
		public float detectionRadius;
		public LayerMask detectionLayer;
		public float maxDetectionAngle;
		public float stopDistance;
	}
}