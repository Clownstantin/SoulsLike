using UnityEngine;

namespace SoulsLike
{
	public class ResetAnimatorBool : StateMachineBehaviour
	{
		[SerializeField] private bool _isInteracting;
		[SerializeField] private bool _canDoCombo;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(_isInteracting) animator.SetBool(AnimatorHandler.IsInteracting, false);
			if(_canDoCombo) animator.SetBool(AnimatorHandler.CanDoCombo, false);
		}
	}
}
