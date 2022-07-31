using UnityEngine;

namespace SoulsLike
{
	public class PlayerLocomotion : MonoBehaviour
	{
		[Header("Movement Stats")]
		[SerializeField] private float _movementSpeed = 5f;
		[SerializeField] private float _walkingSpeed = 1f;
		[SerializeField] private float _sprintSpeed = 7f;
		[SerializeField] private float _rotationSpeed = 10f;
		[SerializeField] private float _fallingSpeed = 45f;
		[Header("Ground & Air Detection Stats")]
		[SerializeField] private float _groundDetectionRayStart = 0.5f;
		[SerializeField] private float _minDistanceToFall = 1f;
		[SerializeField] private float _groundDirectionRayDistance = 0.2f;

		private Transform _cameraObject;
		private InputHandler _inputHandler;
		private PlayerManager _playerManager;

		private Vector3 _normalVector;
		private Vector3 _moveDirection;
		private Vector3 _targetPosition;
		private LayerMask _ignoreForGroundCheck;

		[HideInInspector] public Transform myTransform;
		[HideInInspector] public AnimatorHandler animatorHandler;

		public new Rigidbody rigidbody;
		public GameObject normalCamera;
		public float inAirTimer;

		public void Init(PlayerManager playerManager, AnimatorHandler animatorHandler, InputHandler inputHandler)
		{
			rigidbody = GetComponent<Rigidbody>();

			_inputHandler = inputHandler;
			_playerManager = playerManager;

			this.animatorHandler = animatorHandler;

			_cameraObject = Camera.main.transform;
			myTransform = transform;

			_playerManager.isGrounded = true;
			_ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
		}

		public void HandleMovement(float delta)
		{
			if(_inputHandler.rollFlag || _playerManager.isInteracting) return;

			_moveDirection = _cameraObject.forward * _inputHandler.vertical;
			_moveDirection += _cameraObject.right * _inputHandler.horizontal;
			_moveDirection.Normalize();
			_moveDirection.y = 0;

			float speed = _movementSpeed;

			if(_inputHandler.sprintFlag && _inputHandler.moveAmount > 0.5f)
			{
				speed = _sprintSpeed;
				_playerManager.isSprinting = true;
			}
			else
			{
				if(_inputHandler.moveAmount < 0.5f) speed = _walkingSpeed;
				_playerManager.isSprinting = false;
			}

			_moveDirection *= speed;

			Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
			rigidbody.velocity = projectedVelocity;

			animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0, _playerManager.isSprinting);
			if(animatorHandler.canRotate) HandleRotation(delta);
		}

		public void HandleRollingAndSprinting()
		{
			if(_playerManager.isInteracting) return;

			if(_inputHandler.rollFlag)
			{
				_moveDirection = _cameraObject.forward * _inputHandler.vertical;
				_moveDirection += _cameraObject.right * _inputHandler.horizontal;

				if(_inputHandler.moveAmount > 0)
				{
					animatorHandler.PlayTargetAnimation(AnimationNameBase.Roll, true);
					_moveDirection.y = 0;
					Quaternion rollRotation = Quaternion.LookRotation(_moveDirection);
					myTransform.rotation = rollRotation;
				}
				else animatorHandler.PlayTargetAnimation(AnimationNameBase.Stepback, true);
			}
		}

		public void HandleFalling(float delta)
		{
			_playerManager.isGrounded = false;

			Vector3 origin = myTransform.position;
			origin.y += _groundDetectionRayStart;

			if(Physics.Raycast(origin, myTransform.forward, out _, 0.4f))
				_moveDirection = Vector3.zero;

			if(_playerManager.isInAir)
			{
				rigidbody.AddForce(Vector3.down * _fallingSpeed);
				rigidbody.AddForce(_moveDirection * _fallingSpeed / 10f);
			}

			Vector3 direction = _moveDirection;
			direction.Normalize();
			origin += direction * _groundDirectionRayDistance;

			_targetPosition = myTransform.position;
			Debug.DrawRay(origin, Vector3.down * _minDistanceToFall, Color.red, 0.1f, false);

			if(Physics.Raycast(origin, Vector3.down, out RaycastHit hit, _minDistanceToFall, _ignoreForGroundCheck))
			{
				_normalVector = hit.normal;
				Vector3 targetPos = hit.point;
				_playerManager.isGrounded = true;
				_targetPosition.y = targetPos.y;

				if(_playerManager.isInAir)
				{
					if(inAirTimer > 0.5f)
					{
						Debug.Log($"You were in the air for {inAirTimer}");
						animatorHandler.PlayTargetAnimation(AnimationNameBase.Land, true);
						inAirTimer = 0;
					}
					else
					{
						animatorHandler.PlayTargetAnimation(AnimationNameBase.Locomotion, false);
						inAirTimer = 0;
					}

					_playerManager.isInAir = false;
				}
			}
			else
			{
				if(_playerManager.isGrounded) _playerManager.isGrounded = false;

				if(!_playerManager.isInAir)
				{
					if(!_playerManager.isInteracting) animatorHandler.PlayTargetAnimation(AnimationNameBase.Fall, true);

					Vector3 velocity = rigidbody.velocity;
					velocity.Normalize();
					rigidbody.velocity = velocity * (_movementSpeed * 0.5f);
					_playerManager.isInAir = true;
				}
			}

			if(_playerManager.isGrounded)
			{
				if(_playerManager.isInteracting || _inputHandler.moveAmount > 0)
					myTransform.position = Vector3.Lerp(myTransform.position, _targetPosition, delta);
				else
					myTransform.position = _targetPosition;
			}
		}

		private void HandleRotation(float delta)
		{
			float moveOverride = _inputHandler.moveAmount;

			Vector3 targetDir = _cameraObject.forward * _inputHandler.vertical;
			targetDir += _cameraObject.right * _inputHandler.horizontal;
			targetDir.Normalize();
			targetDir.y = 0;

			if(targetDir == Vector3.zero) targetDir = myTransform.forward;

			float rotSpeed = _rotationSpeed;
			Quaternion lookRotation = Quaternion.LookRotation(targetDir);
			Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, lookRotation, rotSpeed * delta);

			myTransform.rotation = targetRotation;
		}
	}
}
