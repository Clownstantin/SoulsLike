using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(Animator))]
	public class AnimatorHandler : MonoBehaviour
	{
		[SerializeField] private float _movementClamp = 0.55f;
		[SerializeField] private float _dampTime = 0.1f;
		[SerializeField] private bool _canRotate;

		private PlayerManager _playerManager;
		private Animator _animator;
		private InputHandler _inputHandler;
		private PlayerLocomotion _playerLocomotion;

		private int _verticalHash;
		private int _horizontalHash;
		private int _isInteractingHash;

		private const string Vertical = nameof(Vertical);
		private const string Horizontal = nameof(Horizontal);

		public const string IsInteracting = nameof(IsInteracting);
		public const string Roll = nameof(Roll);
		public const string Stepback = nameof(Stepback);

		public bool CanRotate => _canRotate;

		private void OnAnimatorMove()
		{
			if(!_playerManager.IsInteracting) return;

			float delta = Time.deltaTime;

			_playerLocomotion.Rigidbody.drag = 0;
			Vector3 deltaPos = _animator.deltaPosition;
			deltaPos.y = 0;
			Vector3 velocity = deltaPos / delta;
			_playerLocomotion.Rigidbody.velocity = velocity;
		}

		public void Init()
		{
			_animator = GetComponent<Animator>();
			_inputHandler = GetComponentInParent<InputHandler>();
			_playerManager = GetComponentInParent<PlayerManager>();
			_playerLocomotion = GetComponentInParent<PlayerLocomotion>();

			_verticalHash = Animator.StringToHash(Vertical);
			_horizontalHash = Animator.StringToHash(Horizontal);
			_isInteractingHash = Animator.StringToHash(IsInteracting);
		}

		public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting = false)
		{
			float vertical = ClampedAxis(verticalMovement);
			float horizontal = ClampedAxis(horizontalMovement);
			float delta = Time.deltaTime;

			if(isSprinting) vertical = 2f;

			_animator.SetFloat(_verticalHash, vertical, _dampTime, delta);
			_animator.SetFloat(_horizontalHash, horizontal, _dampTime, delta);
		}

		public void PlayTargetAnimation(string targetAnim, bool isInteracting)
		{
			_animator.applyRootMotion = isInteracting;
			_animator.SetBool(_isInteractingHash, isInteracting);
			_animator.CrossFade(targetAnim, 0.2f);
		}

		public void StartOrStopRotation(bool rotState) => _canRotate = rotState;

		private float ClampedAxis(float movementType)
		{
			float axis;

			if(movementType > 0 && movementType < _movementClamp) axis = 0.5f;
			else if(movementType > _movementClamp) axis = 1f;
			else if(movementType < 0f && movementType > -_movementClamp) axis = -0.5f;
			else if(movementType < -_movementClamp) axis = -1f;
			else axis = 0f;

			return axis;
		}
	}
}
