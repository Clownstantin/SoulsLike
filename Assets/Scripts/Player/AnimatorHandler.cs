using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public class AnimatorHandler : MonoBehaviour
	{
		private Animator _animator = default;
		private PlayerLocomotion _playerLocomotion = default;

		private int _verticalHash = default;
		private int _horizontalHash = default;
		private int _isInteractingHash = default;
		private int _canDoComboHash = default;

		private bool _canRotate = default;
		private bool _isInteracting = default;

		public bool CanRotate => _canRotate;

		private void OnEnable()
		{
			this.AddListener(EventID.OnHealthChanged, _ => PlayTargetAnimation(AnimationNameBase.DamageTaken, true));
			this.AddListener(EventID.OnPlayerDeath, _ => PlayTargetAnimation(AnimationNameBase.Death, true));
		}

		private void OnDisable()
		{
			this.RemoveListener(EventID.OnHealthChanged, _ => PlayTargetAnimation(AnimationNameBase.DamageTaken, true));
			this.RemoveListener(EventID.OnPlayerDeath, _ => PlayTargetAnimation(AnimationNameBase.Death, true));
		}

		private void OnAnimatorMove()
		{
			if(!_isInteracting) return;

			float delta = Time.deltaTime;

			_playerLocomotion.rigidbody.drag = 0;
			Vector3 deltaPosition = _animator.deltaPosition;
			deltaPosition.y = 0;
			Vector3 velocity = deltaPosition / delta;
			_playerLocomotion.rigidbody.velocity = velocity;
		}

		public void Init(PlayerLocomotion playerLocomotion, Animator animator)
		{
			_animator = animator;
			_playerLocomotion = playerLocomotion;

			_verticalHash = Animator.StringToHash(AnimatorParameterBase.Vertical);
			_horizontalHash = Animator.StringToHash(AnimatorParameterBase.Horizontal);
			_isInteractingHash = Animator.StringToHash(AnimatorParameterBase.IsInteracting);
			_canDoComboHash = Animator.StringToHash(AnimatorParameterBase.CanDoCombo);

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

		public void UpdateIsInteractingFlag(bool isInteracting) => _isInteracting = isInteracting;

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
