using UnityEngine;

namespace SoulsLike
{
	public class PlayerLocomotion : MonoBehaviour
	{
		[SerializeField] private MovementData _movementData = default;

		private PlayerInputHandler _inputHandler = default;
		private AnimatorHandler _animatorHandler = default;
		private Transform _cameraObject = default;
		private Transform _myTransform = default;
		private Rigidbody _rigidbody = default;

		private Vector3 _normalVector = default;
		private Vector3 _moveDirection = default;
		private Vector3 _targetPosition = default;
		private LayerMask _ignoreForGroundCheck = default;

		private bool _isGrounded = default;
		private bool _isSprinting = default;
		private bool _isInAir = default;
		private float _inAirTimer = default;

		public void Init(Rigidbody rigidbody, AnimatorHandler animatorHandler, PlayerInputHandler inputHandler)
		{
			_rigidbody = rigidbody;
			_inputHandler = inputHandler;
			_animatorHandler = animatorHandler;

			_cameraObject = Camera.main.transform;
			_myTransform = transform;

			_isGrounded = true;
			_ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
		}

		public void HandleMovement(float delta)
		{
			if(_inputHandler.RollFlag) return;
			HandleMoveDirection();

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
			_rigidbody.velocity = projectedVelocity;

			_animatorHandler.UpdateAnimatorValues(_inputHandler.MoveAmount, 0, _isSprinting);
			if(_animatorHandler.CanRotate) HandleRotation(delta);
		}

		public void HandleRollingAndSprinting()
		{
			if(!_inputHandler.RollFlag) return;
			HandleMoveDirection();

			if(_inputHandler.MoveAmount > 0)
			{
				_animatorHandler.PlayTargetAnimation(AnimationNameBase.Roll, true);
				_myTransform.rotation = Quaternion.LookRotation(_moveDirection);
			}
			else _animatorHandler.PlayTargetAnimation(AnimationNameBase.Stepback, true);
		}

		public void HandleInAirTimer(float delta)
		{
			if(_isInAir) _inAirTimer += delta;
		}

		public void HandleFalling(float delta, bool isInteracting)
		{
			_isGrounded = false;
			float fallingSpeed = _movementData.fallingSpeed;
			float minDistToFall = _movementData.minDistanceToFall;
			float moveSpeed = _movementData.movementSpeed;

			Vector3 origin = _myTransform.position;
			origin.y += _movementData.groundDetectionRayStart;

			if(Physics.Raycast(origin, _myTransform.forward, out _, 0.4f))
				_moveDirection = Vector3.zero;

			if(_isInAir)
			{
				_rigidbody.AddForce(Vector3.down * fallingSpeed);
				_rigidbody.AddForce(_moveDirection * fallingSpeed / 10f);
			}

			origin += _moveDirection * _movementData.groundDirectionRayDistance;
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
				_targetPosition += delta * moveSpeed * _myTransform.forward;

				if(!_isInAir)
				{
					if(!isInteracting) _animatorHandler.PlayTargetAnimation(AnimationNameBase.Fall, true);

					Vector3 velocity = _rigidbody.velocity;
					velocity.Normalize();
					_rigidbody.velocity = velocity * (moveSpeed * 0.5f);
					_isInAir = true;
				}
			}

			if(isInteracting || _inputHandler.MoveAmount > 0)
				_myTransform.position = Vector3.Lerp(_myTransform.position, _targetPosition, delta / 0.1f);
			else
				_myTransform.position = _targetPosition;
		}

		private void HandleRotation(float delta)
		{
			HandleMoveDirection();

			if(_moveDirection == Vector3.zero) _moveDirection = _myTransform.forward;

			float rotSpeed = _movementData.rotationSpeed;
			Quaternion lookRotation = Quaternion.LookRotation(_moveDirection);
			Quaternion targetRotation = Quaternion.Slerp(_myTransform.rotation, lookRotation, rotSpeed * delta);

			_myTransform.rotation = targetRotation;
		}

		private void HandleMoveDirection()
		{
			_moveDirection = _cameraObject.forward * _inputHandler.Vertical;
			_moveDirection += _cameraObject.right * _inputHandler.Horizontal;
			_moveDirection.Normalize();
			_moveDirection.y = 0;
		}
	}
}
