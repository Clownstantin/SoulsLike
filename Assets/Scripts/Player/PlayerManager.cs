using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(PlayerStats), typeof(PlayerLocomotion), typeof(PlayerAttacker)),
	 RequireComponent(typeof(PlayerInventory), typeof(PlayerInputHandler))]
	public class PlayerManager : UpdateableComponent
	{
		private Animator _animator = default;
		private AnimatorHandler _animatorHandler = default;
		private CameraHandler _cameraHandler = default;

		private PlayerInputHandler _inputHandler = default;
		private PlayerInteractSystem _interactSystem = default;
		private PlayerLocomotion _playerLocomotion = default;
		private PlayerStats _playerStats = default;
		private PlayerAttacker _playerAttacker = default;
		private PlayerInventory _playerInventory = default;

		private WeaponSlotManager _weaponSlotManager = default;
		private Transform _myTransform = default;

		private bool _isInteracting = default;
		private bool _canDoCombo = default;

		#region MonoBehaviour
		private void Awake()
		{
			_playerStats = GetComponent<PlayerStats>();
			_playerLocomotion = GetComponent<PlayerLocomotion>();
			_playerAttacker = GetComponent<PlayerAttacker>();
			_playerInventory = GetComponent<PlayerInventory>();
			_inputHandler = GetComponent<PlayerInputHandler>();
			_interactSystem = GetComponent<PlayerInteractSystem>();

			_animator = GetComponentInChildren<Animator>();
			_animatorHandler = GetComponentInChildren<AnimatorHandler>();
			_weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();

			_weaponSlotManager.Init(_animator, _playerStats);
		}

		private void OnEnable()
		{
			this.AddListener<GamePause>(OnGamePause);
			this.AddListener<GameResume>(OnGameResume);
		}

		private void OnDisable()
		{
			this.RemoveListener<GamePause>(OnGamePause);
			this.RemoveListener<GameResume>(OnGameResume);
		}

		protected override void OnStart()
		{
			_myTransform = transform;
			_cameraHandler = GameManager.Instance.CameraHandler;

			_playerAttacker.Init(_playerInventory, _animatorHandler, _weaponSlotManager);
			_interactSystem.Init(_animatorHandler, _playerInventory, _playerLocomotion);
			_playerLocomotion.Init(_animatorHandler, _inputHandler);
			_animatorHandler.Init(_playerLocomotion, _animator);
			_playerInventory.Init();
			_playerStats.Init();
		}

		public override void OnUpdate(float delta)
		{
			_isInteracting = _animator.GetBool(AnimatorParameterBase.IsInteracting);
			_canDoCombo = _animator.GetBool(AnimatorParameterBase.CanDoCombo);

			_inputHandler.TickInput(delta, _isInteracting, _canDoCombo);
			_animatorHandler.UpdateIsInteractingFlag(_isInteracting);

			if(!_isInteracting)
			{
				_playerLocomotion.HandleMovement(delta);
				_playerLocomotion.HandleRollingAndSprinting();
			}

			_playerLocomotion.HandleFalling(delta, _isInteracting);

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

		private void OnGamePause(GamePause _)
		{
			_animator.enabled = false;
			_playerLocomotion.rigidbody.isKinematic = true;
		}

		private void OnGameResume(GameResume _)
		{
			_animator.enabled = true;
			_playerLocomotion.rigidbody.isKinematic = false;
		}
	}
}
