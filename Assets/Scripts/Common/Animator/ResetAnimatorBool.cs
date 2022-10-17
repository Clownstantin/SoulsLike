using UnityEngine;

namespace SoulsLike
{
	public class ResetAnimatorBool : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			foreach(AnimatorControllerParameter parameter in animator.parameters)
			{
				if(parameter.type != AnimatorControllerParameterType.Bool) continue;
				animator.SetBool(parameter.nameHash, false);
			}
		}
	}
}