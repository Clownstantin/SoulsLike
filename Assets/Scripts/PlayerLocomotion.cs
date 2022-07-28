using UnityEngine;

namespace SoulsLike
{
	public class PlayerLocomotion : MonoBehaviour
	{
		[Header("Stats")]
		[SerializeField] private float _movementSpeed = 5f;
		[SerializeField] private float _rotationSpeed = 10f;

		private Transform _cameraObject;
		private InputHandler _inputHandler;

		private Vector3 _moveDirection;
		private Vector3 _normalVector;
		private Vector3 _targetPosition;

		[HideInInspector] public Transform myTransform;
		[HideInInspector] public AnimatorHandler animatorHandler;

		public new Rigidbody rigidbody;
		public GameObject normalCamera;

		#region Monobehavior
		private void Start()
		{
			rigidbody = GetComponent<Rigidbody>();
			_inputHandler = GetComponent<InputHandler>();

			animatorHandler = GetComponentInChildren<AnimatorHandler>();
			animatorHandler.Init();

			_cameraObject = Camera.main.transform;
			myTransform = transform;
		}

		private void Update()
		{
			float delta = Time.deltaTime;

			_inputHandler.TickInput(delta);

			_moveDirection = _cameraObject.forward * _inputHandler.vertical;
			_moveDirection += _cameraObject.right * _inputHandler.horizontal;
			_moveDirection.Normalize();
			_moveDirection.y = 0;

			float speed = _movementSpeed;
			_moveDirection *= speed;

			Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
			rigidbody.velocity = projectedVelocity;

			animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0);
			if(animatorHandler.canRotate) HandleRotation(delta);
		}
		#endregion

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
