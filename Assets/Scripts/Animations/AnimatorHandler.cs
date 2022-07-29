using UnityEngine;

namespace SoulsLike
{
	public class AnimatorHandler : MonoBehaviour
	{
		private PlayerManager _playerManager;
		private InputHandler _inputHandler;
		private PlayerLocomotion _playerLocomotion;

		private int _verticalHash;
		private int _horizontalHash;
		private int _isInteractingHash;

		private const string Vertical = nameof(Vertical);
		private const string Horizontal = nameof(Horizontal);

		public Animator animator;

		public bool canRotate;

		public const string IsInteracting = nameof(IsInteracting);
		public const string Roll = nameof(Roll);
		public const string Stepback = nameof(Stepback);

		#region MonoBehaviour
		private void OnAnimatorMove()
		{
			if(_playerManager.isInteracting == false) return;

			float delta = Time.deltaTime;

			_playerLocomotion.rigidbody.drag = 0;
			Vector3 deltaPosition = animator.deltaPosition;
			deltaPosition.y = 0;
			Vector3 velocity = deltaPosition / delta;
			_playerLocomotion.rigidbody.velocity = velocity;
		}
		#endregion

		public void Init()
		{
			animator = GetComponent<Animator>();
			_playerManager = GetComponentInParent<PlayerManager>();
			_playerLocomotion = GetComponentInParent<PlayerLocomotion>();
			_inputHandler = GetComponentInParent<InputHandler>();

			_verticalHash = Animator.StringToHash(Vertical);
			_horizontalHash = Animator.StringToHash(Horizontal);
			_isInteractingHash = Animator.StringToHash(IsInteracting);
		}

		public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
		{
			float vertical = ClampAxis(verticalMovement);
			float horizontal = ClampAxis(horizontalMovement);

			if(isSprinting)
			{
				vertical = 2f;
				horizontal = horizontalMovement;
			}

			animator.SetFloat(_verticalHash, vertical, 0.1f, Time.deltaTime);
			animator.SetFloat(_horizontalHash, horizontal, 0.1f, Time.deltaTime);
		}

		public void PlayTargetAnimation(string animationName, bool isInteracting)
		{
			animator.applyRootMotion = isInteracting;
			animator.SetBool(_isInteractingHash, isInteracting);
			animator.CrossFade(animationName, 0.2f);
		}

		public void CanRotate() => canRotate = true;

		public void StopRotation() => canRotate = false;

		private float ClampAxis(float axis)
		{
			if(axis > 0 && axis < 0.55f) axis = 0.5f;
			else if(axis > 0.55f) axis = 1f;
			else if(axis < 0 && axis > -0.55f) axis = -0.5f;
			else if(axis < -0.55f) axis = -1f;
			else axis = 0;

			return axis;
		}
	}
}
