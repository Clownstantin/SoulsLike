using UnityEngine;

namespace SoulsLike
{
	public class PlayerManager : MonoBehaviour
	{
		private InputHandler _inputHandler;
		private Animator _animator;
		private CameraHandler _cameraHandler;
		private PlayerLocomotion _playerLocomotion;

		public bool isInteracting;

		[Header("Player Flags")]
		public bool isSprinting;
		public bool isInAir;
		public bool isGrounded;

		#region MonoBehaviour
		private void Start()
		{
			_inputHandler = GetComponent<InputHandler>();
			_playerLocomotion = GetComponent<PlayerLocomotion>();
			_animator = GetComponentInChildren<Animator>();

			_cameraHandler = CameraHandler.instance;
		}

		private void Update()
		{
			float delta = Time.deltaTime;

			isInteracting = _animator.GetBool(AnimatorHandler.IsInteracting);

			_inputHandler.TickInput(delta);
			_playerLocomotion.HandleMovement(delta);
			_playerLocomotion.HandleRollingAndSprinting();
			_playerLocomotion.HandleFalling(delta);
		}

		private void FixedUpdate()
		{
			float delta = Time.fixedDeltaTime;

			if(_cameraHandler != null)
			{
				_cameraHandler.FollowTarget(delta);
				_cameraHandler.HandleCameraRotation(delta, _inputHandler.mouseX, _inputHandler.mouseY);
			}
		}

		private void LateUpdate()
		{
			_inputHandler.rollFlag = false;
			_inputHandler.sprintFlag = false;
			_inputHandler.rightLightAttackInput = false;
			_inputHandler.rightHeavyAttackInput = false;

			if(isInAir) _playerLocomotion.inAirTimer += Time.deltaTime;
		}
		#endregion
	}
}
