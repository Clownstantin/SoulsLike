using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(InputHandler))]
	public class PlayerManager : MonoBehaviour
	{
		private InputHandler _inputHandler;
		private Animator _animator;

		private void Start()
		{
			_inputHandler = GetComponent<InputHandler>();
			_animator = GetComponentInChildren<Animator>();
		}

		private void Update()
		{
			_inputHandler.SetIsInteractingParam(_animator.GetBool(AnimatorHandler.IsInteracting));
			_inputHandler.SetRollFlag(false);
			_inputHandler.SetSprintFlag(false);
		}
	}
}