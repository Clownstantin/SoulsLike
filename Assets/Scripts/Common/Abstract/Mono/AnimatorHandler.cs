using UnityEngine;

namespace SoulsLike
{
	public abstract class AnimatorHandler : MonoBehaviour
	{
		[SerializeField] protected float crossFadeTransitionDuration = 0.2f;

		protected Animator animator = default;

		protected int verticalHash = default;
		protected int horizontalHash = default;
		protected int isInteractingHash = default;
		protected int canDoComboHash = default;
		protected int isInAirHash = default;
		protected int isUsingRightHandHash = default;

		public virtual void Init()
		{
			animator = GetComponent<Animator>();

			verticalHash = Animator.StringToHash(AnimatorParameterBase.Vertical);
			horizontalHash = Animator.StringToHash(AnimatorParameterBase.Horizontal);
			isInteractingHash = Animator.StringToHash(AnimatorParameterBase.IsInteracting);
			canDoComboHash = Animator.StringToHash(AnimatorParameterBase.CanDoCombo);
			isInAirHash = Animator.StringToHash(AnimatorParameterBase.IsInAir);
			isUsingRightHandHash = Animator.StringToHash(AnimatorParameterBase.IsUsingRightHand);
		}

		public void PlayTargetAnimation(string animationName, bool isInteracting)
		{
			animator.applyRootMotion = isInteracting;
			animator.SetBool(isInteractingHash, isInteracting);
			animator.CrossFade(animationName, crossFadeTransitionDuration);
		}

		#region AnimationEvents
		private void EnableCombo() => animator.SetBool(canDoComboHash, true);

		public void DisableCombo() => animator.SetBool(canDoComboHash, false);
		#endregion
	}
}