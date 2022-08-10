using UnityEngine;

namespace SoulsLike
{
	public class CameraHandler : MonoBehaviour
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
		}

		[SerializeField] private CameraData _cameraData = default;

		private Transform _myTransform = default;
		private Vector3 _cameraPosition = default;
		private Vector3 _cameraFollowVelocity = default;
		private LayerMask _ignoreLayers = default;

		private float _defaultPositionZ = default;
		private float _targetPositionZ = default;
		private float _lookAngle = default;
		private float _pivotAngle = default;

		#region Monobehavior
		private void Awake()
		{
			_myTransform = transform;
			_defaultPositionZ = _cameraData.cameraTransform.localPosition.z;
			_ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
		}
		#endregion

		public void FollowTarget(float delta, Vector3 targetPosition)
		{
			Vector3 targetPos = Vector3.SmoothDamp(_myTransform.position, targetPosition,
								ref _cameraFollowVelocity, delta * _cameraData.followSpeed);
			_myTransform.position = targetPos;

			HandleCameraCollisions(delta);
		}

		public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
		{
			_lookAngle += mouseXInput * _cameraData.lookSpeed / delta;
			_pivotAngle -= mouseYInput * _cameraData.pivotSpeed / delta;
			_pivotAngle = Mathf.Clamp(_pivotAngle, -_cameraData.clampPivot, _cameraData.clampPivot);

			Vector3 rotation = Vector3.zero;
			rotation.y = _lookAngle;
			Quaternion targetRotation = Quaternion.Euler(rotation);
			_myTransform.rotation = targetRotation;

			rotation = Vector3.zero;
			rotation.x = _pivotAngle;
			targetRotation = Quaternion.Euler(rotation);
			_cameraData.cameraPivotTransform.localRotation = targetRotation;
		}

		private void HandleCameraCollisions(float delta)
		{
			_targetPositionZ = _defaultPositionZ;

			Vector3 cameraPos = _cameraData.cameraTransform.position;
			Vector3 pivotPos = _cameraData.cameraPivotTransform.position;

			Vector3 direction = cameraPos - pivotPos;
			direction.Normalize();

			if(Physics.SphereCast(pivotPos, _cameraData.cameraSphereRadius, direction, out RaycastHit hit,
								  Mathf.Abs(_targetPositionZ), _ignoreLayers) && !hit.collider.isTrigger)
			{
				float distance = Vector3.Distance(pivotPos, hit.point);
				_targetPositionZ = -(distance - _cameraData.cameraCollisionOffset);

				if(Mathf.Abs(_targetPositionZ) < _cameraData.minCollisionOffset) _targetPositionZ = -_cameraData.minCollisionOffset;
			}

			_cameraPosition.z = Mathf.Lerp(_cameraData.cameraTransform.localPosition.z, _targetPositionZ, delta / 0.1f);
			_cameraData.cameraTransform.localPosition = _cameraPosition;
		}
	}
}
