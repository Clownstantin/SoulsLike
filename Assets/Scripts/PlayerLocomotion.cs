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
		private Vector3 _targetPosition;
		private LayerMask _ignoreForGroundCheck;

		[HideInInspector] public Transform myTransform;
		[HideInInspector] public AnimatorHandler animatorHandler;

		public new Rigidbody rigidbody;
		public GameObject normalCamera;

		public Vector3 moveDirection;
		public float inAirTimer;

		#region Monobehavior
		private void Start()
		{
			rigidbody = GetComponent<Rigidbody>();
			_inputHandler = GetComponent<InputHandler>();
			_playerManager = GetComponent<PlayerManager>();

			animatorHandler = GetComponentInChildren<AnimatorHandler>();
			animatorHandler.Init();

			_cameraObject = Camera.main.transform;
			myTransform = transform;

			_playerManager.isGrounded = true;
			_ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
		}
		#endregion

		public void HandleMovement(float delta)
		{
			if(_inputHandler.rollFlag || _playerManager.isInteracting) return;

			moveDirection = _cameraObject.forward * _inputHandler.vertical;
			moveDirection += _cameraObject.right * _inputHandler.horizontal;
			moveDirection.Normalize();
			moveDirection.y = 0;

			float speed = _movementSpeed;

			if(_inputHandler.sprintFlag)
			{
				speed = _sprintSpeed;
				_playerManager.isSprinting = true;
				moveDirection *= speed;
			}
			else moveDirection *= speed;

			Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, _normalVector);
			rigidbody.velocity = projectedVelocity;

			animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0, _playerManager.isSprinting);
			if(animatorHandler.canRotate) HandleRotation(delta);
		}

		public void HandleRollingAndSprinting(float delta)
		{
			if(animatorHandler.animator.GetBool(AnimatorHandler.IsInteracting)) return;

			if(_inputHandler.rollFlag)
			{
				moveDirection = _cameraObject.forward * _inputHandler.vertical;
				moveDirection += _cameraObject.right * _inputHandler.horizontal;

				if(_inputHandler.moveAmount > 0)
				{
					animatorHandler.PlayTargetAnimation(AnimatorHandler.Roll, true);
					moveDirection.y = 0;
					Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
					myTransform.rotation = rollRotation;
				}
				else animatorHandler.PlayTargetAnimation(AnimatorHandler.Stepback, true);
			}
		}

		public void HandleFalling(float delta, Vector3 moveDirection)
		{
			_playerManager.isGrounded = false;

			Vector3 origin = myTransform.position;
			origin.y += _groundDetectionRayStart;

			if(Physics.Raycast(origin, myTransform.forward, out _, 0.4f))
				moveDirection = Vector3.zero;

			if(_playerManager.isInAir)
			{
				rigidbody.AddForce(Vector3.down * _fallingSpeed);
				rigidbody.AddForce(moveDirection * _fallingSpeed / 10f);
			}

			Vector3 direction = moveDirection;
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
						animatorHandler.PlayTargetAnimation(AnimatorHandler.Land, true);
						inAirTimer = 0;
					}
					else
					{
						animatorHandler.PlayTargetAnimation(AnimatorHandler.Locomotion, false);
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
					if(!_playerManager.isInteracting)
						animatorHandler.PlayTargetAnimation(AnimatorHandler.Fall, true);

					Vector3 velocity = rigidbody.velocity;
					velocity.Normalize();
					rigidbody.velocity = velocity * (_movementSpeed * 0.5f);
					_playerManager.isInAir = true;
				}
			}

			if(_playerManager.isGrounded)
			{
				if(_playerManager.isInteracting || _inputHandler.moveAmount > 0)
					myTransform.position = Vector3.Lerp(myTransform.position, _targetPosition, Time.deltaTime);
				else
					myTransform.position = _targetPosition;
			}
		}

		private void HandleRotation(float delta)
		{
			Vector3 targetDir = Vector3.zero;
			float moveOverride = _inputHandler.moveAmount;

			targetDir = _cameraObject.forward * _inputHandler.vertical;
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
