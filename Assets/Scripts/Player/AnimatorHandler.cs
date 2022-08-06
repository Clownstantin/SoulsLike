using UnityEngine;

namespace SoulsLike
{
	public class AnimatorHandler : MonoBehaviour
	{
		private Animator _animator;
		private PlayerManager _playerManager;
		private PlayerLocomotion _playerLocomotion;

		private int _verticalHash;
		private int _horizontalHash;
		private int _isInteractingHash;
		private int _canDoComboHash;

		private bool _canRotate;

		private const string Vertical = nameof(Vertical);
		private const string Horizontal = nameof(Horizontal);

		public const string IsInteracting = nameof(IsInteracting);
		public const string CanDoCombo = nameof(CanDoCombo);

		public bool CanRotate => _canRotate;

		private void OnAnimatorMove()
		{
			if(!_playerManager.IsInteracting) return;

			float delta = Time.deltaTime;

			_playerLocomotion.rigidbody.drag = 0;
			Vector3 deltaPosition = _animator.deltaPosition;
			deltaPosition.y = 0;
			Vector3 velocity = deltaPosition / delta;
			_playerLocomotion.rigidbody.velocity = velocity;
		}

		public void Init(PlayerManager playerManager, PlayerLocomotion playerLocomotion, Animator animator)
		{
			_animator = animator;

			_playerManager = playerManager;
			_playerLocomotion = playerLocomotion;

			_verticalHash = Animator.StringToHash(Vertical);
			_horizontalHash = Animator.StringToHash(Horizontal);
			_isInteractingHash = Animator.StringToHash(IsInteracting);
			_canDoComboHash = Animator.StringToHash(CanDoCombo);

			EnableRotation();
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

			_animator.SetFloat(_verticalHash, vertical, 0.1f, Time.deltaTime);
			_animator.SetFloat(_horizontalHash, horizontal, 0.1f, Time.deltaTime);
		}

		public void PlayTargetAnimation(string animationName, bool isInteracting)
		{
			_animator.applyRootMotion = isInteracting;
			_animator.SetBool(_isInteractingHash, isInteracting);
			_animator.CrossFade(animationName, 0.2f);
		}

		public void EnableRotation() => _canRotate = true;

		public void StopRotation() => _canRotate = false;

		#region AnimationEvents
		public void EnableCombo() => _animator.SetBool(_canDoComboHash, true);

		public void DisableCombo() => _animator.SetBool(_canDoComboHash, false);
		#endregion

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
