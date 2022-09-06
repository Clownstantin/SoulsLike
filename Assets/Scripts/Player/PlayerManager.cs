using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(PlayerStats), typeof(PlayerLocomotion), typeof(PlayerAttackSystem)),
	 RequireComponent(typeof(PlayerInventory), typeof(PlayerInput))]
	public class PlayerManager : UpdateableComponent
	{
		private AnimatorHandler _animatorHandler = default;
		private CameraHandler _cameraHandler = default;

		private PlayerInput _inputHandler = default;
		private PlayerInteractSystem _interactSystem = default;
		private PlayerLocomotion _playerLocomotion = default;
		private PlayerStats _playerStats = default;
		private PlayerAttackSystem _playerAttack = default;
		private PlayerInventory _playerInventory = default;

		private WeaponSlotManager _weaponSlotManager = default;
		private Transform _myTransform = default;

		#region MonoBehaviour
		private void Awake()
		{
			_playerStats = GetComponent<PlayerStats>();
			_playerLocomotion = GetComponent<PlayerLocomotion>();
			_playerAttack = GetComponent<PlayerAttackSystem>();
			_playerInventory = GetComponent<PlayerInventory>();
			_inputHandler = GetComponent<PlayerInput>();
			_interactSystem = GetComponent<PlayerInteractSystem>();

			_animatorHandler = GetComponentInChildren<AnimatorHandler>();
			_weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
		}

		protected override void OnStart()
		{
			_myTransform = transform;
			_cameraHandler = GameManager.Instance.CameraHandler;

			_weaponSlotManager.Init();
			_playerAttack.Init(_playerInventory, _animatorHandler, _weaponSlotManager);
			_playerLocomotion.Init(_inputHandler);
			_animatorHandler.Init();
			_interactSystem.Init();
			_playerInventory.Init();
			_playerStats.Init();
		}

		public override void OnUpdate(float delta)
		{
			bool isInteracting = _animatorHandler.IsInteracting;

			_inputHandler.TickInput(delta, isInteracting, _animatorHandler.CanDoCombo);
			_animatorHandler.UpdateAnimatorValues(delta, _inputHandler.MoveAmount, 0, _playerLocomotion.IsSprinting, _playerLocomotion.IsInAir);

			if(!isInteracting)
			{
				_playerLocomotion.HandleMovement();
				_playerLocomotion.HandleRotation(delta, _animatorHandler.CanRotate);
				_playerLocomotion.HandleRollingAndSprinting();
			}

			_playerLocomotion.HandleFalling(delta, isInteracting);
			_interactSystem.CheckObjectToInteract(_inputHandler.InteractInput);
		}

		public override void OnFixedUpdate(float delta)
		{
			if(_cameraHandler == null) return;
			_cameraHandler.FollowTarget(delta, _myTransform.position);
			_cameraHandler.HandleCameraRotation(delta, _inputHandler.MouseX, _inputHandler.MouseY);
		}

		public override void OnLateUpdate(float delta)
		{
			_inputHandler.ResetFlags();
			_playerLocomotion.HandleInAirTimer(delta);
		}
		#endregion
	}
}
