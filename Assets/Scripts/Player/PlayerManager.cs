using UnityEngine;

namespace SoulsLike
{
	public class PlayerManager : MonoBehaviour
	{
		private Animator _animator;
		private AnimatorHandler _animatorHandler;
		private InputHandler _inputHandler;
		private CameraHandler _cameraHandler;

		private PlayerLocomotion _playerLocomotion;
		private PlayerStats _playerStats;
		private PlayerAttacker _playerAttacker;
		private PlayerInventory _playerInventory;

		private WeaponSlotManager _weaponSlotManager;
		private Transform _myTransform;

		public bool isInteracting;

		[Header("Player Flags")]
		public bool isSprinting;
		public bool isInAir;
		public bool isGrounded;
		public bool canDoCombo;

		#region MonoBehaviour
		private void Awake()
		{
			_playerStats = GetComponent<PlayerStats>();
			_playerLocomotion = GetComponent<PlayerLocomotion>();
			_playerAttacker = GetComponent<PlayerAttacker>();
			_playerInventory = GetComponent<PlayerInventory>();
			_inputHandler = GetComponent<InputHandler>();

			_animator = GetComponentInChildren<Animator>();
			_animatorHandler = GetComponentInChildren<AnimatorHandler>();
			_weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();

			_weaponSlotManager.Init(_animator);
		}

		private void Start()
		{
			_myTransform = transform;
			_cameraHandler = CameraHandler.instance;

			_playerLocomotion.Init(this, _animatorHandler, _inputHandler);
			_playerAttacker.Init(_inputHandler, _animatorHandler);
			_playerInventory.Init(_weaponSlotManager);
			_inputHandler.Init(this, _playerAttacker, _playerInventory);
			_animatorHandler.Init(this, _playerLocomotion, _animator);
			_playerStats.Init(_animatorHandler);
		}

		private void Update()
		{
			float delta = Time.deltaTime;

			isInteracting = _animator.GetBool(AnimatorHandler.IsInteracting);
			canDoCombo = _animator.GetBool(AnimatorHandler.CanDoCombo);

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
				_cameraHandler.FollowTarget(delta, _myTransform);
				_cameraHandler.HandleCameraRotation(delta, _inputHandler.mouseX, _inputHandler.mouseY);
			}
		}

		private void LateUpdate()
		{
			ResetFlags();

			if(isInAir) _playerLocomotion.inAirTimer += Time.deltaTime;
		}

		private void ResetFlags(bool state = false)
		{
			_inputHandler.rollFlag = state;
			_inputHandler.sprintFlag = state;
			_inputHandler.rightLightAttackInput = state;
			_inputHandler.rightHeavyAttackInput = state;
			_inputHandler.d_Pad_Up = state;
			_inputHandler.d_Pad_Down = state;
			_inputHandler.d_Pad_Left = state;
			_inputHandler.d_Pad_Right = state;
		}
		#endregion
	}
}
