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
		private Vector3 _cameraVelocity = default;
		private LayerMask _ignoreLayers = default;

		private float _defaultPositionZ = default;
		private float _targetPositionZ = default;
		private float _lookAngle = default;
		private float _pivotAngle = default;

		private bool _isLockedOnTarget = default;

		public bool IsLockedOnTarget => _isLockedOnTarget;
		public Transform CameraTransform => _cameraData.cameraTransform;
		public Transform CurrentLockOnTarget => _currentLockOnTarget;

		#region Monobehavior
		private void Awake()
		{
			_myTransform = transform;
			_availableTargets = new List<UnitManager>(_cameraData.maxTargets);
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
			                                       ref _cameraVelocity, delta * _cameraData.followSpeed);
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
				Vector3 lockOnPos = _currentLockOnTarget.position;
				Vector3 dir = (lockOnPos - _myTransform.position).normalized;
				dir.y = 0;
				_myTransform.rotation = Quaternion.Lerp(_myTransform.rotation, Quaternion.LookRotation(dir), delta * _cameraData.lockOnLerpSpeed);

				dir = (lockOnPos - cameraPivotTransform.position).normalized;
				Vector3 eulerAngle = Quaternion.LookRotation(dir).eulerAngles;
				eulerAngle.y = 0;
				cameraPivotTransform.localEulerAngles = eulerAngle;
			}
		}

		public void HandleCameraHeight(float delta)
		{
			Vector3 velocity = Vector3.zero;
			var newLockedPos = new Vector3(0, _cameraData.lockedPivotPosition);
			var newUnlockedPos = new Vector3(0, _cameraData.unlockedPivotPosition);
			Vector3 camLocalPos = _cameraData.cameraPivotTransform.localPosition;

			_cameraData.cameraPivotTransform.localPosition = Vector3.SmoothDamp(camLocalPos, _isLockedOnTarget ? newLockedPos : newUnlockedPos, ref velocity, delta);
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

			foreach(UnitManager target in _availableTargets)
			{
				float distanceFromTarget = Vector3.Distance(_playerTransform.position, target.transform.position);

				if(!(distanceFromTarget < shortestDistance)) continue;
				shortestDistance = distanceFromTarget;
				_currentLockOnTarget = target.LockOnTransform;
			}
		}

		private void OnSwitchTarget(SwitchOnTargetEvent eventInfo)
		{
			if(!_isLockedOnTarget) return;

			float shortestLeftTargetDistance = Mathf.Infinity;
			float shortestRightTargetDistance = Mathf.Infinity;

			foreach(UnitManager target in _availableTargets)
			{
				Vector3 targetPos = target.transform.position;
				Vector3 lockOnPos = _currentLockOnTarget.transform.position;
				Vector3 relativeTargetPos = _currentLockOnTarget.InverseTransformPoint(targetPos);
				float distanceFromLeftTarget = lockOnPos.x - targetPos.x;
				float distanceFromRightTarget = lockOnPos.x + targetPos.x;

				switch(relativeTargetPos.x)
				{
					case > 0 when distanceFromLeftTarget < shortestLeftTargetDistance:
						shortestLeftTargetDistance = distanceFromLeftTarget;
						_leftLockOnTarget = target.LockOnTransform;
						break;
					case < 0 when distanceFromRightTarget < shortestRightTargetDistance:
						shortestRightTargetDistance = distanceFromRightTarget;
						_rightLockOnTarget = target.LockOnTransform;
						break;
				}
			}

			if(eventInfo.isLeftTarget && _leftLockOnTarget) _currentLockOnTarget = _leftLockOnTarget;
			else if(eventInfo.isRightTarget && _rightLockOnTarget) _currentLockOnTarget = _rightLockOnTarget;
		}

		private void FindAvailableTargets()
		{
			var colliderBuff = new Collider[_cameraData.maxTargets];
			int buffSize = Physics.OverlapSphereNonAlloc(_playerTransform.position, _cameraData.lockOnSphereRadius,
			                                             colliderBuff, _cameraData.lockOnLayer);

			for(int i = 0; i < buffSize; i++)
			{
				if(!colliderBuff[i].TryGetComponent(out UnitManager unit)) continue;

				Vector3 playerPos = _playerTransform.position;
				Vector3 unitPos = unit.transform.position;
				Vector3 lockTargetDir = (unitPos - playerPos).normalized;
				float distanceFromTarget = Vector3.Distance(playerPos, unitPos);
				float viewAngle = Vector3.Angle(lockTargetDir, _cameraData.cameraTransform.forward);

				if(unit.transform.root == _playerTransform.root || !(viewAngle > -_cameraData.clampViewAngle) ||
				   !(viewAngle < _cameraData.clampViewAngle) || !(distanceFromTarget <= _cameraData.maxLockOnDistance)) continue;

				if(Physics.Linecast(_playerTransform.position, unit.LockOnTransform.position, out RaycastHit hit) &&
				   hit.transform.gameObject.layer != 6)
					_availableTargets.Add(unit);
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