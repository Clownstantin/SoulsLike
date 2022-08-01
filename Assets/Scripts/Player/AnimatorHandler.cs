using UnityEngine;

namespace SoulsLike
{
	public class AnimatorHandler : MonoBehaviour
	{
		private PlayerManager _playerManager;
		private PlayerLocomotion _playerLocomotion;

		private int _verticalHash;
		private int _horizontalHash;
		private int _isInteractingHash;
		private int _canDoComboHash;

		private const string Vertical = nameof(Vertical);
		private const string Horizontal = nameof(Horizontal);

		public const string IsInteracting = nameof(IsInteracting);
		public const string CanDoCombo = nameof(CanDoCombo);

		public Animator animator;
		public bool canRotate;

		private void OnAnimatorMove()
		{
			if(!_playerManager.isInteracting) return;

			float delta = Time.deltaTime;

			_playerLocomotion.rigidbody.drag = 0;
			Vector3 deltaPosition = animator.deltaPosition;
			deltaPosition.y = 0;
			Vector3 velocity = deltaPosition / delta;
			_playerLocomotion.rigidbody.velocity = velocity;
		}

		public void Init(PlayerManager playerManager, PlayerLocomotion playerLocomotion, Animator animator)
		{
			this.animator = animator;

			_playerManager = playerManager;
			_playerLocomotion = playerLocomotion;

			_verticalHash = Animator.StringToHash(Vertical);
			_horizontalHash = Animator.StringToHash(Horizontal);
			_isInteractingHash = Animator.StringToHash(IsInteracting);
			_canDoComboHash = Animator.StringToHash(CanDoCombo);
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

		public void EnableCombo() => animator.SetBool(_canDoComboHash, true);

		public void DisableCombo() => animator.SetBool(_canDoComboHash, false);

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
