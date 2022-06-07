using UnityEngine;

namespace SoulsLike
{
	public class CameraHandler : MonoBehaviour
	{
		[SerializeField] private Transform _targetTransform;
		[SerializeField] private Transform _cameraTransform;
		[SerializeField] private Transform _cameraPivotTransform;
		[Header("Stats")]
		[SerializeField] private float _lookSpeed = 0.1f;
		[SerializeField] private float _followSpeed = 0.1f;
		[SerializeField] private float _pivotSpeed = 0.03f;
		[SerializeField] private float _pivotClampValue = 35f;
		[SerializeField] private float _cameraSphereRadius = 0.2f;
		[SerializeField] private float _cameraCollisionOffset = 0.2f;
		[SerializeField] private float _minCollisionOffset = 0.2f;

		private Transform _transform;
		private Vector3 _cameraPosition;
		private LayerMask _ignoreLayers;
		private Vector3 _cameraFollowVelocity = default;

		private float _targetPositionZ;
		private float _defaultPositionZ;
		private float _lookAngle;
		private float _pivotAngle;

		private static CameraHandler s_instance;

		public static CameraHandler Instance => s_instance;

		private void Awake()
		{
			Singleton();

			_transform = transform;
			_defaultPositionZ = _cameraTransform.localPosition.z;
			_ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
		}

		public void FollowTarget(float delta)
		{
			Vector3 targetPos = Vector3.SmoothDamp(_transform.position, _targetTransform.position, ref _cameraFollowVelocity, delta / _followSpeed);
			_transform.position = targetPos;

			HandleCameraCollision(delta);
		}

		public void HandleCameraRotation(float delta, float mouseX, float mouseY)
		{
			_lookAngle += mouseX * _lookSpeed / delta;
			_pivotAngle -= mouseY * _pivotSpeed / delta;
			_pivotAngle = Mathf.Clamp(_pivotAngle, -_pivotClampValue, _pivotClampValue);

			Vector3 rotation = Vector3.zero;
			rotation.y = _lookAngle;
			Quaternion targetRot = Quaternion.Euler(rotation);
			_transform.rotation = targetRot;

			rotation = Vector3.zero;
			rotation.x = _pivotAngle;
			targetRot = Quaternion.Euler(rotation);
			_cameraPivotTransform.localRotation = targetRot;
		}

		private void HandleCameraCollision(float delta)
		{
			_targetPositionZ = _defaultPositionZ;
			Vector3 dir = _cameraTransform.position - _cameraPivotTransform.position;
			dir.Normalize();

			if(Physics.SphereCast(_cameraPivotTransform.position, _cameraSphereRadius,
				dir, out RaycastHit hit, Mathf.Abs(_targetPositionZ), _ignoreLayers))
			{
				float distance = Vector3.Distance(_cameraPivotTransform.position, hit.point);
				_targetPositionZ = -(distance - _cameraCollisionOffset);
			}

			if(Mathf.Abs(_targetPositionZ) < _minCollisionOffset)
				_targetPositionZ = -_minCollisionOffset;

			_cameraPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, _targetPositionZ, delta / 0.2f);
			_cameraTransform.localPosition = _cameraPosition;
		}

		private void Singleton()
		{
			if(s_instance != null && s_instance != this)
				Destroy(gameObject);
			else
			{
				s_instance = this;
				DontDestroyOnLoad(gameObject);
			}
		}
	}
}
