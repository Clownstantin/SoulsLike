using UnityEngine;

namespace SoulsLike
{
	public class CameraHandler : MonoBehaviour
	{
		[SerializeField] private Transform _cameraTransform = default;
		[SerializeField] private Transform _cameraPivotTransform = default;

		[Header("Movement")]
		[SerializeField] private float _lookSpeed = 0.1f;
		[SerializeField] private float _followSpeed = 0.1f;
		[SerializeField] private float _pivotSpeed = 0.03f;
		[SerializeField] private float _clampPivot = 35f;

		[Header("Collision Detection")]
		[SerializeField] private float _cameraSphereRadius = 0.2f;
		[SerializeField] private float _cameraCollisionOffset = 0.2f;
		[SerializeField] private float _minCollisionOffset = 0.2f;

		private static CameraHandler s_instance = default;

		private Transform _myTransform = default;
		private Vector3 _cameraPosition = default;
		private Vector3 _cameraFollowVelocity = default;
		private LayerMask _ignoreLayers = default;

		private float _defaultPositionZ = default;
		private float _targetPositionZ = default;
		private float _lookAngle = default;
		private float _pivotAngle = default;

		public static CameraHandler Instance => s_instance;

		#region Monobehavior
		private void Awake()
		{
			s_instance = this;
			_myTransform = transform;
			_defaultPositionZ = _cameraTransform.localPosition.z;
			_ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
		}
		#endregion

		public void FollowTarget(float delta, Vector3 targetPosition)
		{
			Vector3 targetPos = Vector3.SmoothDamp(_myTransform.position, targetPosition,
								ref _cameraFollowVelocity, delta * _followSpeed);
			_myTransform.position = targetPos;

			HandleCameraCollisions(delta);
		}

		public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
		{
			_lookAngle += mouseXInput * _lookSpeed / delta;
			_pivotAngle -= mouseYInput * _pivotSpeed / delta;
			_pivotAngle = Mathf.Clamp(_pivotAngle, -_clampPivot, _clampPivot);

			Vector3 rotation = Vector3.zero;
			rotation.y = _lookAngle;
			Quaternion targetRotation = Quaternion.Euler(rotation);
			_myTransform.rotation = targetRotation;

			rotation = Vector3.zero;
			rotation.x = _pivotAngle;
			targetRotation = Quaternion.Euler(rotation);
			_cameraPivotTransform.localRotation = targetRotation;
		}

		private void HandleCameraCollisions(float delta)
		{
			_targetPositionZ = _defaultPositionZ;

			Vector3 direction = _cameraTransform.position - _cameraPivotTransform.position;
			direction.Normalize();

			if(Physics.SphereCast(_cameraPivotTransform.position, _cameraSphereRadius, direction, out RaycastHit hit,
								  Mathf.Abs(_targetPositionZ), _ignoreLayers) && !hit.collider.isTrigger)
			{
				float distance = Vector3.Distance(_cameraPivotTransform.position, hit.point);
				_targetPositionZ = -(distance - _cameraCollisionOffset);

				if(Mathf.Abs(_targetPositionZ) < _minCollisionOffset) _targetPositionZ = -_minCollisionOffset;
			}

			_cameraPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, _targetPositionZ, delta / 0.1f);
			_cameraTransform.localPosition = _cameraPosition;
		}
	}
}
