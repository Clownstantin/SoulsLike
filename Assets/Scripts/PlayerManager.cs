using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(InputHandler))]
	public class PlayerManager : MonoBehaviour
	{
		private CameraHandler _cameraHandler;
		private InputHandler _inputHandler;
		private PlayerLocomotion _playerLocomotion;
		private Animator _animator;

		private bool _isInteracting;
		private bool _isSprinting;

		public bool IsInteracting => _isInteracting;
		public bool IsSprinting => _isSprinting;

		private void Awake() => _cameraHandler = CameraHandler.Instance;

		private void Start()
		{
			_inputHandler = GetComponent<InputHandler>();
			_playerLocomotion = GetComponent<PlayerLocomotion>();
			_animator = GetComponentInChildren<Animator>();
		}

		private void Update()
		{
			float delta = Time.deltaTime;

			_isInteracting = _animator.GetBool(AnimatorHandler.IsInteracting);

			_inputHandler.TickInput(delta);

			_playerLocomotion.HandleMovement(delta);
			_playerLocomotion.HandleRollingAndSprinting(delta);
		}

		private void FixedUpdate()
		{
			float delta = Time.fixedDeltaTime;

			if(_cameraHandler == null) return;

			_cameraHandler.FollowTarget(delta);
			_cameraHandler.HandleCameraRotation(delta, _inputHandler.MouseX, _inputHandler.MouseY);
		}

		private void LateUpdate()
		{
			_inputHandler.SetRollFlag(false);
			_inputHandler.SetSprintFlag(false);
			_isSprinting = _inputHandler.RollButtonPressed;
		}

		public void SetIsSprinting(bool isSprinting) => _isSprinting = isSprinting;
	}
}