using UnityEngine;

namespace SoulsLike
{
	public class ResetAnimatorBool : StateMachineBehaviour
	{
		[SerializeField] private bool _isInteracting = default;
		[SerializeField] private bool _canDoCombo = default;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(_isInteracting) animator.SetBool(AnimatorParameterBase.IsInteracting, false);
			if(_canDoCombo) animator.SetBool(AnimatorParameterBase.CanDoCombo, false);
		}
	}
}
