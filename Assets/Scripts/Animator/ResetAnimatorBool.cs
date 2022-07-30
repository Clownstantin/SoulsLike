using UnityEngine;

namespace SoulsLike
{
	public class ResetAnimatorBool : StateMachineBehaviour
	{
		public bool isInteracting;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(isInteracting) animator.SetBool(AnimatorHandler.IsInteracting, false);
		}
	}
}
