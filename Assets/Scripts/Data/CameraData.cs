using UnityEngine;

namespace SoulsLike
{
	[System.Serializable]
	public struct CameraData
	{
		public Transform cameraTransform;
		public Transform cameraPivotTransform;

		[Header("Movement")]
		public float lookSpeed;
		public float followSpeed;
		public float pivotSpeed;
		public float clampPivot;

		[Header("Collision Detection")]
		public float cameraSphereRadius;
		public float cameraCollisionOffset;
		public float minCollisionOffset;

		[Header("LockOn Settings")] 
		public LayerMask lockOnLayer;
		public int maxTargets;
		public float lockOnLerpSpeed;
		public float maxLockOnDistance;
		public float lockOnSphereRadius;
		public float clampViewAngle;
		public float lockedPivotPosition;
		public float unlockedPivotPosition;
	}
}
