using UnityEngine;

namespace SoulsLike
{
	[System.Serializable]
	public struct MovementData
	{
		public Camera normalCamera;

		[Header("Movement Stats")]
		public float movementSpeed;
		public float walkingSpeed;
		public float sprintSpeed;
		public float rotationSpeed;
		public float fallingSpeed;

		[Header("Ground & Air Detection Stats")]
		public float groundDetectionRayStart;
		public float minDistanceToFall;
		public float groundDirectionRayDistance;
	}
}
