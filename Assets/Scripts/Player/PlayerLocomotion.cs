using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerLocomotion : MonoBehaviour
	{
		[SerializeField] private MovementData _movementData = default;

		private PlayerInputHandler _inputHandler = default;
		private AnimatorHandler _animatorHandler = default;
		private PlayerManager _playerManager = default;
		private Transform _cameraObject = default;
		private Transform _myTransform = default;

		private Vector3 _normalVector = default;
		private Vector3 _moveDirection = default;
		private Vector3 _targetPosition = default;
		private LayerMask _ignoreForGroundCheck = default;

		private bool _isGrounded = default;
		private bool _isSprinting = default;
		private bool _isInAir = default;
		private float _inAirTimer = default;

		[HideInInspector] public new Rigidbody rigidbody = default;

		public void Init(PlayerManager playerManager, AnimatorHandler animatorHandler, PlayerInputHandler inputHandler)
		{
			rigidbody = GetComponent<Rigidbody>();

			_inputHandler = inputHandler;
			_playerManager = playerManager;

			_animatorHandler = animatorHandler;

			_cameraObject = Camera.main.transform;
			_myTransform = transform;

			_isGrounded = true;
			_ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
		}

		public void HandleMovement(float delta)
		{
			if(_inputHandler.RollFlag || _playerManager.IsInteracting) return;

			_moveDirection = _cameraObject.forward * _inputHandler.Vertical;
			_moveDirection += _cameraObject.right * _inputHandler.Horizontal;
			_moveDirection.Normalize();
			_moveDirection.y = 0;

			float speed = _movementData.movementSpeed;

			if(_inputHandler.SprintFlag && _inputHandler.MoveAmount > 0.5f)
			{
				speed = _movementData.sprintSpeed;
				_isSprinting = true;
			}
			else
			{
				if(_inputHandler.MoveAmount < 0.5f) speed = _movementData.walkingSpeed;
				_isSprinting = false;
			}

			_moveDirection *= speed;

			Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
			rigidbody.velocity = projectedVelocity;

			_animatorHandler.UpdateAnimatorValues(_inputHandler.MoveAmount, 0, _isSprinting);
			if(_animatorHandler.CanRotate) HandleRotation(delta);
		}

		public void HandleRollingAndSprinting()
		{
			if(_playerManager.IsInteracting) return;

			if(_inputHandler.RollFlag)
			{
				_moveDirection = _cameraObject.forward * _inputHandler.Vertical;
				_moveDirection += _cameraObject.right * _inputHandler.Horizontal;

				if(_inputHandler.MoveAmount > 0)
				{
					_animatorHandler.PlayTargetAnimation(AnimationNameBase.Roll, true);
					_moveDirection.y = 0;
					Quaternion rollRotation = Quaternion.LookRotation(_moveDirection);
					_myTransform.rotation = rollRotation;
				}
				else _animatorHandler.PlayTargetAnimation(AnimationNameBase.Stepback, true);
			}
		}

		public void HandleInAirTimer(float delta) => _inAirTimer += _isInAir ? delta : 0;

		public void HandleFalling(float delta)
		{
			_isGrounded = false;
			float fallingSpeed = _movementData.fallingSpeed;
			float minDistToFall = _movementData.minDistanceToFall;

			Vector3 origin = _myTransform.position;
			origin.y += _movementData.groundDetectionRayStart;

			if(Physics.Raycast(origin, _myTransform.forward, out _, 0.4f))
				_moveDirection = Vector3.zero;

			if(_isInAir)
			{
				rigidbody.AddForce(Vector3.down * fallingSpeed);
				rigidbody.AddForce(_moveDirection * fallingSpeed / 10f);
			}

			Vector3 direction = _moveDirection;
			direction.Normalize();
			origin += direction * _movementData.groundDirectionRayDistance;

			_targetPosition = _myTransform.position;
			Debug.DrawRay(origin, Vector3.down * minDistToFall, Color.red, 0.1f, false);

			if(Physics.Raycast(origin, Vector3.down, out RaycastHit hit, minDistToFall, _ignoreForGroundCheck))
			{
				_normalVector = hit.normal;
				Vector3 targetPos = hit.point;
				_isGrounded = true;
				_targetPosition.y = targetPos.y;

				if(_isInAir)
				{
					if(_inAirTimer > 0.5f)
					{
						Debug.Log($"You were in the air for {_inAirTimer}");
						_animatorHandler.PlayTargetAnimation(AnimationNameBase.Land, true);
						_inAirTimer = 0;
					}
					else
					{
						_animatorHandler.PlayTargetAnimation(AnimationNameBase.Empty, false);
						_inAirTimer = 0;
					}

					_isInAir = false;
				}
			}
			else
			{
				if(_isGrounded) _isGrounded = false;

				if(!_isInAir)
				{
					if(!_playerManager.IsInteracting) _animatorHandler.PlayTargetAnimation(AnimationNameBase.Fall, true);

					Vector3 velocity = rigidbody.velocity;
					velocity.Normalize();
					rigidbody.velocity = velocity * (_movementData.movementSpeed * 0.5f);
					_isInAir = true;
				}
			}

			if(_playerManager.IsInteracting || _inputHandler.MoveAmount > 0)
				_myTransform.position = Vector3.Lerp(_myTransform.position, _targetPosition, delta / 0.1f);
			else
				_myTransform.position = _targetPosition;
		}

		private void HandleRotation(float delta)
		{
			float moveOverride = _inputHandler.MoveAmount;

			Vector3 targetDir = _cameraObject.forward * _inputHandler.Vertical;
			targetDir += _cameraObject.right * _inputHandler.Horizontal;
			targetDir.Normalize();
			targetDir.y = 0;

			if(targetDir == Vector3.zero) targetDir = _myTransform.forward;

			float rotSpeed = _movementData.rotationSpeed;
			Quaternion lookRotation = Quaternion.LookRotation(targetDir);
			Quaternion targetRotation = Quaternion.Slerp(_myTransform.rotation, lookRotation, rotSpeed * delta);

			_myTransform.rotation = targetRotation;
		}
	}
}
