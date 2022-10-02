using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.Serialization;

namespace SoulsLike
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerLocomotion : MonoBehaviour
	{
		[FormerlySerializedAs("_movementData")] [SerializeField]
		private PlayerMovementData _playerMovementData = default;

		private PlayerInput _inputHandler = default;
		private CameraHandler _cameraHandler = default;

		private Transform _cameraTransform = default;
		private Transform _myTransform = default;
		private Rigidbody _rigidbody = default;

		private Vector3 _normalVector = default;
		private Vector3 _moveDirection = default;
		private Vector3 _targetPosition = default;
		private LayerMask _ignoreForGroundCheck = default;

		private float _currentSpeed = default;

		private bool _isGrounded = default;
		private bool _isSprinting = default;
		private bool _isInAir = default;
		private float _inAirTimer = default;

		public bool IsSprinting => _isSprinting;
		public bool IsInAir => _isInAir;

		private void OnEnable() => Subscribe();

		private void OnDisable() => Unsubscribe();

		public void Init(PlayerInput inputHandler, CameraHandler cameraHandler)
		{
			_rigidbody = GetComponent<Rigidbody>();
			_inputHandler = inputHandler;
			_cameraHandler = cameraHandler;

			_cameraTransform = _cameraHandler.CameraTransform;
			_myTransform = transform;

			_isGrounded = true;
			_ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
		}

		public void HandleMovement()
		{
			if(_inputHandler.RollFlag) return;

			HandleMoveDirection();
			_moveDirection *= _currentSpeed;

			Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
			_rigidbody.velocity = projectedVelocity;
		}

		public void HandleSprinting()
		{
			_currentSpeed = _playerMovementData.movementSpeed;

			if(_inputHandler.SprintFlag && _inputHandler.MoveAmount > 0.5f)
			{
				_currentSpeed = _playerMovementData.sprintSpeed;
				_isSprinting = true;
			}
			else
			{
				if(_inputHandler.MoveAmount < 0.5f) _currentSpeed = _playerMovementData.walkingSpeed;
				_isSprinting = false;
			}
		}

		public void HandleRolling(bool rollFlag)
		{
			if(!rollFlag) return;
			HandleMoveDirection();

			bool isMoving = _inputHandler.MoveAmount > 0;
			if(isMoving) _myTransform.rotation = Quaternion.LookRotation(_moveDirection);

			this.TriggerEvent(new RollEvent(isMoving));
		}

		public void HandleInAirTimer(float delta)
		{
			if(_isInAir) _inAirTimer += delta;
		}

		public void HandleFalling(float delta, bool isInteracting)
		{
			_isGrounded = false;
			float fallingSpeed = _playerMovementData.fallingSpeed;
			float minDistToFall = _playerMovementData.minDistanceToFall;
			float moveSpeed = _playerMovementData.movementSpeed;

			Vector3 origin = _myTransform.position;
			origin.y += _playerMovementData.groundDetectionRayStart;

			if(Physics.Raycast(origin, _myTransform.forward, out _, 0.4f))
				_moveDirection = Vector3.zero;

			if(_isInAir)
			{
				_rigidbody.AddForce(Vector3.down * fallingSpeed);
				_rigidbody.AddForce(_moveDirection * fallingSpeed / 10f);
			}

			origin += _moveDirection * _playerMovementData.groundDirectionRayDistance;
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
					bool isLongLand = _inAirTimer > 0.5f;
					if(isLongLand)
					{
						Debug.Log($"You were in the air for {_inAirTimer}");
						_inAirTimer = 0;
					}
					else _inAirTimer = 0;

					this.TriggerEvent(new Landed(isLongLand));
					_isInAir = false;
				}
			}
			else
			{
				if(_isGrounded) _isGrounded = false;
				_targetPosition += delta * moveSpeed * _myTransform.forward;

				if(!_isInAir)
				{
					if(!isInteracting) this.TriggerEvent(new Fall());

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

		public void HandleRotation(float delta)
		{
			if(_cameraHandler.IsLockedOnTarget && !_inputHandler.SprintFlag && !_inputHandler.RollFlag)
			{
				Vector3 rotationDir = _cameraHandler.CurrentLockOnTarget.transform.position - _myTransform.position;
				rotationDir.y = 0;
				rotationDir.Normalize();

				RotateTowardsDirection(delta, rotationDir);
			}
			else
			{
				HandleMoveDirection();

				if(_moveDirection == Vector3.zero) _moveDirection = _myTransform.forward;
				RotateTowardsDirection(delta, _moveDirection);
			}
		}

		public void Subscribe()
		{
			this.AddListener<PickUpEvent>(OnPickUp);
			this.AddListener<JumpEvent>(OnJump);
			this.AddListener<AnimationPlayEvent>(OnAnimationPlay);
			this.AddListener<GamePause>(OnGamePause);
			this.AddListener<GameResume>(OnGameResume);
		}

		public void Unsubscribe()
		{
			this.RemoveListener<PickUpEvent>(OnPickUp);
			this.RemoveListener<JumpEvent>(OnJump);
			this.RemoveListener<AnimationPlayEvent>(OnAnimationPlay);
			this.RemoveListener<GamePause>(OnGamePause);
			this.RemoveListener<GameResume>(OnGameResume);
		}

		private void RotateTowardsDirection(float delta, Vector3 direction)
		{
			float rotSpeed = _playerMovementData.rotationSpeed;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			Quaternion targetRotation = Quaternion.Slerp(_myTransform.rotation, lookRotation, rotSpeed * delta);

			_myTransform.rotation = targetRotation;
		}

		private void HandleMoveDirection()
		{
			_moveDirection = _cameraTransform.forward * _inputHandler.Vertical;
			_moveDirection += _cameraTransform.right * _inputHandler.Horizontal;
			_moveDirection.Normalize();
			_moveDirection.y = 0;
		}

		private void OnJump(JumpEvent eventInfo)
		{
			if(!(eventInfo.moveAmount > 0)) return;
			HandleMoveDirection();
			_myTransform.rotation = Quaternion.LookRotation(_moveDirection);
		}

		private void OnPickUp(PickUpEvent _) => _rigidbody.velocity = Vector3.zero;

		private void OnGameResume(GameResume _) => _rigidbody.isKinematic = false;

		private void OnGamePause(GamePause _) => _rigidbody.isKinematic = true;

		private void OnAnimationPlay(AnimationPlayEvent eventInfo)
		{
			_rigidbody.drag = 0;
			_rigidbody.velocity = eventInfo.velocity;
		}
	}
}