using UnityEngine;

namespace SoulsLike
{
	public class CameraHandler : MonoBehaviour
	{
		private Transform _myTransform;
		private Vector3 _cameraPosition;
		private Vector3 _cameraFollowVelocity = Vector3.zero;
		private LayerMask _ignoreLayers;

		private float _defaultPositionZ;
		private float _targetPositionZ;
		private float _lookAngle;
		private float _pivotAngle;

		public static CameraHandler instance;

		public Transform targetTransform;
		public Transform cameraTransform;
		public Transform cameraPivotTransform;

		public float lookSpeed = 0.1f;
		public float followSpeed = 0.1f;
		public float pivotSpeed = 0.03f;
		public float minPivot = -35f;
		public float maxPivot = 35f;

		public float cameraSphereRadius = 0.2f;
		public float cameraCollisionOffset = 0.2f;
		public float minCollisionOffset = 0.2f;

		#region Monobehavior
		private void Awake()
		{
			instance = this;
			_myTransform = transform;
			_defaultPositionZ = cameraTransform.localPosition.z;
			_ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
		}
		#endregion

		public void FollowTarget(float delta)
		{
			Vector3 targetPos = Vector3.SmoothDamp(_myTransform.position, targetTransform.position,
								ref _cameraFollowVelocity, delta / followSpeed);
			_myTransform.position = targetPos;

			HandleCameraCollisions(delta);
		}

		public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
		{
			_lookAngle += mouseXInput * lookSpeed / delta;
			_pivotAngle -= mouseYInput * pivotSpeed / delta;
			_pivotAngle = Mathf.Clamp(_pivotAngle, minPivot, maxPivot);

			Vector3 rotation = Vector3.zero;
			rotation.y = _lookAngle;
			Quaternion targetRotation = Quaternion.Euler(rotation);
			_myTransform.rotation = targetRotation;

			rotation = Vector3.zero;
			rotation.x = _pivotAngle;
			targetRotation = Quaternion.Euler(rotation);
			cameraPivotTransform.localRotation = targetRotation;
		}

		private void HandleCameraCollisions(float delta)
		{
			_targetPositionZ = _defaultPositionZ;

			Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
			direction.Normalize();

			if(Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction,
								  out RaycastHit hit, Mathf.Abs(_targetPositionZ), _ignoreLayers))
			{
				float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
				_targetPositionZ = -(distance - cameraCollisionOffset);

				if(Mathf.Abs(_targetPositionZ) < minCollisionOffset) _targetPositionZ = -minCollisionOffset;

				_cameraPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, _targetPositionZ, delta / 0.1f);
				cameraTransform.localPosition = _cameraPosition;
			}
		}
	}
}
