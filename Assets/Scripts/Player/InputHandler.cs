using UnityEngine;

namespace SoulsLike
{
	public class InputHandler : MonoBehaviour
	{
		private PlayerControls _inputActions = default;
		private PlayerManager _playerManager = default;
		private PlayerAttacker _playerAttacker = default;
		private PlayerInventory _playerInventory = default;

		private Vector2 _movementInput = default;
		private Vector2 _cameraInput = default;

		private float _horizontal = default;
		private float _vertical = default;
		private float _moveAmount = default;
		private float _mouseX = default;
		private float _mouseY = default;

		private bool _interactInput = default;
		private bool _rollInput = default;
		private bool _rightLightAttackInput = default;
		private bool _rightHeavyAttackInput = default;

		private bool _quickSlotUpInput = default;
		private bool _quickSlotDownInput = default;
		private bool _quickSlotLeftInput = default;
		private bool _quickSlotRightInput = default;

		private bool _sprintFlag = default;
		private bool _comboFlag = default;
		private bool _rollFlag = default;
		private float _rollInputTimer = default;

		public float Horizontal => _horizontal;
		public float Vertical => _vertical;
		public float MoveAmount => _moveAmount;
		public float MouseX => _mouseX;
		public float MouseY => _mouseY;

		public bool RollFlag => _rollFlag;
		public bool SprintFlag => _sprintFlag;
		public bool ComboFlag => _comboFlag;

		public bool InteractInput => _interactInput;

		private void OnEnable()
		{
			_inputActions ??= new PlayerControls();
			_inputActions.PlayerMovement.Movement.performed += m => _movementInput = m.ReadValue<Vector2>();
			_inputActions.PlayerMovement.Camera.performed += c => _cameraInput = c.ReadValue<Vector2>();

			_inputActions.Enable();
		}

		private void OnDisable()
		{
			_inputActions.PlayerMovement.Movement.performed -= m => _movementInput = m.ReadValue<Vector2>();
			_inputActions.PlayerMovement.Camera.performed -= c => _cameraInput = c.ReadValue<Vector2>();

			_inputActions.Disable();
		}

		public void Init(PlayerManager playerManager, PlayerAttacker playerAttacker, PlayerInventory playerInventory)
		{
			_playerManager = playerManager;
			_playerAttacker = playerAttacker;
			_playerInventory = playerInventory;
		}

		public void TickInput(float delta)
		{
			MoveInput();
			HandleRollInput(delta);
			HandleAttackInput();
			HandleQuickSlotsInput();
			HandleInteractInput();
		}

		public void ResetFlags(bool state = false)
		{
			_interactInput = state;
			_rollFlag = state;
			_sprintFlag = state;

			_rightLightAttackInput = state;
			_rightHeavyAttackInput = state;

			_quickSlotUpInput = state;
			_quickSlotDownInput = state;
			_quickSlotLeftInput = state;
			_quickSlotRightInput = state;
		}

		private void MoveInput()
		{
			_horizontal = _movementInput.x;
			_vertical = _movementInput.y;
			_moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
			_mouseX = _cameraInput.x;
			_mouseY = _cameraInput.y;
		}

		private void HandleRollInput(float delta)
		{
			_rollInput = _inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

			if(_rollInput)
			{
				_rollInputTimer += delta;
				_sprintFlag = true;
			}
			else
			{
				if(_rollInputTimer > 0 && _rollInputTimer < 0.5f)
				{
					_sprintFlag = false;
					_rollFlag = true;
				}

				_rollInputTimer = 0;
			}
		}

		private void HandleInteractInput() => _interactInput = _inputActions.PlayerActions.Interact.WasPerformedThisFrame();

		private void HandleAttackInput()
		{
			_rightLightAttackInput = _inputActions.PlayerActions.LightAttack.WasPerformedThisFrame();
			_rightHeavyAttackInput = _inputActions.PlayerActions.HeavyAttack.WasPerformedThisFrame();

			PerformAttack(_rightLightAttackInput, _rightHeavyAttackInput, _playerInventory.RightWeapon);
		}

		private void HandleQuickSlotsInput()
		{
			_quickSlotRightInput = _inputActions.PlayerActions.DPadRight.WasPerformedThisFrame();
			_quickSlotLeftInput = _inputActions.PlayerActions.DPadLeft.WasPerformedThisFrame();

			if(_quickSlotRightInput) _playerInventory.ChangeWeaponInSlot();
			else if(_quickSlotLeftInput) _playerInventory.ChangeWeaponInSlot(true);
		}

		private void PerformAttack(bool lightAttackInput, bool heavyAttackInput, WeaponItem weapon)
		{
			if(lightAttackInput || heavyAttackInput)
			{
				if(_playerManager.CanDoCombo)
				{
					_comboFlag = true;
					_playerAttacker.HandleWeaponCombo(weapon);
					_comboFlag = false;
				}
				else
				{
					if(_playerManager.IsInteracting || _playerManager.CanDoCombo) return;

					if(lightAttackInput) _playerAttacker.HandleLightAttack(weapon);
					else if(heavyAttackInput) _playerAttacker.HandleHeavyAttack(weapon);
				}
			}
		}
	}
}
