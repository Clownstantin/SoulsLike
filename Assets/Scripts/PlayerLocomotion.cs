using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(Rigidbody), typeof(InputHandler))]
	public class PlayerLocomotion : MonoBehaviour
	{
		[Header("Stats")]
		[SerializeField] private float _movementSpeed = 5f;
		[SerializeField] private float _rotationSpeed = 10f;

		private Transform _cameraTransform;
		private Vector3 _moveDirection;

		private InputHandler _inputHandler;
		private AnimatorHandler _animatorHandler;

		private Transform _transform;
		private Rigidbody _rigidbody;
		private Camera _normalCamera;

		private Vector3 _normal;
		private Vector3 _targetPosition;

		private void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_inputHandler = GetComponent<InputHandler>();
			_animatorHandler = GetComponentInChildren<AnimatorHandler>();
			_animatorHandler.Init();

			_cameraTransform = Camera.main.transform;
			_transform = transform;
		}

		private void Update()
		{
			float delta = Time.deltaTime;
			_inputHandler.TickInput(delta);

			_moveDirection = _cameraTransform.forward * _inputHandler.Vertical;
			_moveDirection += _cameraTransform.right * _inputHandler.Horizontal;
			_moveDirection.Normalize();
			_moveDirection.y = 0;

			float speed = _movementSpeed;
			_moveDirection *= speed;

			Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normal);
			_rigidbody.velocity = projectedVelocity;

			_animatorHandler.UpdateAnimatorValues(_inputHandler.MoveAmount, 0);

			if(_animatorHandler.CanRotate) HandleRotation(delta);
		}

		#region Movement
		private void HandleRotation(float delta)
		{
			Vector3 targetDir = Vector3.zero;
			float moveOverride = _inputHandler.MoveAmount;

			targetDir = _cameraTransform.forward * _inputHandler.Vertical;
			targetDir += _cameraTransform.right * _inputHandler.Horizontal;
			targetDir.Normalize();
			targetDir.y = 0;

			if(targetDir == Vector3.zero) targetDir = _transform.forward;

			float rotSpeed = _rotationSpeed;
			Quaternion lookRot = Quaternion.LookRotation(targetDir);
			Quaternion targetRotation = Quaternion.Slerp(_transform.rotation, lookRot, rotSpeed * delta);

			_transform.rotation = targetRotation;
		}
		#endregion
	}
}