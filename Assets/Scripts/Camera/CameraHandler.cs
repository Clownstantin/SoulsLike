using System.Collections.Generic;
using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public class CameraHandler : MonoBehaviour
	{
		[SerializeField] private CameraData _cameraData = default;

		private List<UnitManager> _availableTargets = default;
		private Transform _currentLockOnTarget = default;
		private Transform _leftLockOnTarget = default;
		private Transform _rightLockOnTarget = default;

		private Transform _playerTransform = default;
		private Transform _myTransform = default;

		private Vector3 _cameraPosition = default;
		private Vector3 _cameraFollowVelocity = default;
		private LayerMask _ignoreLayers = default;

		private float _defaultPositionZ = default;
		private float _targetPositionZ = default;
		private float _lookAngle = default;
		private float _pivotAngle = default;

		private bool _isLockedOnTarget = default;

		#region Monobehavior
		private void Awake()
		{
			_myTransform = transform;
			_availableTargets = new List<UnitManager>();
			_defaultPositionZ = _cameraData.cameraTransform.localPosition.z;
			_ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
		}

		private void OnEnable()
		{
			this.AddListener<LockOnTargetEvent>(OnLockOnTarget);
			this.AddListener<SwitchOnTargetEvent>(OnSwitchTarget);
		}

		private void OnDisable()
		{
			this.RemoveListener<LockOnTargetEvent>(OnLockOnTarget);
			this.RemoveListener<SwitchOnTargetEvent>(OnSwitchTarget);
		}
		#endregion

		public void Init(Transform playerTransform) => _playerTransform = playerTransform;

		public void FollowTarget(float delta)
		{
			Vector3 targetPos = Vector3.SmoothDamp(_myTransform.position, _playerTransform.position,
								ref _cameraFollowVelocity, delta * _cameraData.followSpeed);
			_myTransform.position = targetPos;

			HandleCameraCollisions(delta);
		}

		public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
		{
			Transform cameraPivotTransform = _cameraData.cameraPivotTransform;

			if(!_isLockedOnTarget)
			{
				_lookAngle += mouseXInput * _cameraData.lookSpeed * delta;
				_pivotAngle -= mouseYInput * _cameraData.pivotSpeed * delta;
				_pivotAngle = Mathf.Clamp(_pivotAngle, -_cameraData.clampPivot, _cameraData.clampPivot);

				Vector3 rotation = Vector3.zero;
				rotation.y = _lookAngle;
				Quaternion targetRotation = Quaternion.Euler(rotation);
				_myTransform.rotation = targetRotation;

				rotation = Vector3.zero;
				rotation.x = _pivotAngle;
				targetRotation = Quaternion.Euler(rotation);
				cameraPivotTransform.localRotation = targetRotation;
			}
			else
			{
				float velocity = 0;

				Vector3 dir = (_currentLockOnTarget.position - _myTransform.position).normalized;
				dir.y = 0;
				_myTransform.rotation = Quaternion.Lerp(_myTransform.rotation, Quaternion.LookRotation(dir), delta * _cameraData.lockOnLerpSpeed);

				dir = (_currentLockOnTarget.position - cameraPivotTransform.position).normalized;
				Vector3 eulerAngle = Quaternion.LookRotation(dir).eulerAngles;
				eulerAngle.y = 0;
				cameraPivotTransform.localEulerAngles = eulerAngle;
			}
		}

		private void OnLockOnTarget(LockOnTargetEvent _)
		{
			_isLockedOnTarget = !_isLockedOnTarget;
			if(_isLockedOnTarget) FindAvailableTargets();
			else ClearAvailableTargets();

			if(_availableTargets.Count == 0)
			{
				_isLockedOnTarget = false;
				return;
			}

			float shortestDistance = Mathf.Infinity;

			for(int i = 0; i < _availableTargets.Count; i++)
			{
				float distanceFromTarget = Vector3.Distance(_playerTransform.position, _availableTargets[i].transform.position);

				if(distanceFromTarget < shortestDistance)
				{
					shortestDistance = distanceFromTarget;
					_currentLockOnTarget = _availableTargets[i].LockOnTransform;
				}
			}
		}

		private void OnSwitchTarget(SwitchOnTargetEvent eventInfo)
		{
			if(!_isLockedOnTarget) return;

			float shortestLeftTargetDistance = Mathf.Infinity;
			float shortestRightTargetDistance = Mathf.Infinity;

			for(int i = 0; i < _availableTargets.Count; i++)
			{
				Vector3 relativeTargetPos = _currentLockOnTarget.InverseTransformPoint(_availableTargets[i].transform.position);
				float distanceFromLeftTarget = _currentLockOnTarget.transform.position.x - _availableTargets[i].transform.position.x;
				float distanceFromRightTarget = _currentLockOnTarget.transform.position.x + _availableTargets[i].transform.position.x;

				if(relativeTargetPos.x > 0 && distanceFromLeftTarget < shortestLeftTargetDistance)
				{
					shortestLeftTargetDistance = distanceFromLeftTarget;
					_leftLockOnTarget = _availableTargets[i].LockOnTransform;
				}

				if(relativeTargetPos.x < 0 && distanceFromRightTarget < shortestRightTargetDistance)
				{
					shortestRightTargetDistance = distanceFromRightTarget;
					_rightLockOnTarget = _availableTargets[i].LockOnTransform;
				}
			}

			Log.Send($"Targets to switch {_availableTargets.Count}");

			if(eventInfo.isLeftTarget && _leftLockOnTarget) _currentLockOnTarget = _leftLockOnTarget;
			else if(eventInfo.isRightTarget && _rightLockOnTarget) _currentLockOnTarget = _rightLockOnTarget;
		}

		private void FindAvailableTargets()
		{
			Collider[] colliders = Physics.OverlapSphere(_playerTransform.position, _cameraData.lockOnSphereRadius);

			for(int i = 0; i < colliders.Length; i++)
			{
				if(colliders[i].TryGetComponent(out UnitManager unit))
				{
					Vector3 lockTargetDir = (unit.transform.position - _playerTransform.position).normalized;
					float distanceFromTarget = Vector3.Distance(_playerTransform.position, unit.transform.position);
					float viewAngle = Vector3.Angle(lockTargetDir, _cameraData.cameraTransform.forward);

					if(unit.transform.root != _playerTransform.root && viewAngle > -_cameraData.clampAngle &&
						viewAngle < _cameraData.clampAngle && distanceFromTarget <= _cameraData.maxLockOnDistance)
						_availableTargets.Add(unit);
				}
			}
		}

		private void ClearAvailableTargets()
		{
			_availableTargets.Clear();
			_currentLockOnTarget = null;
			_rightLockOnTarget = null;
			_leftLockOnTarget = null;
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
