using UnityEngine;
using UnityEngine.InputSystem;

namespace SoulsLike
{
	public class PlayerInputHandler : MonoBehaviour
	{
		private PlayerControls _inputActions = default;
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

		public void Init(PlayerAttacker playerAttacker, PlayerInventory playerInventory)
		{
			_playerAttacker = playerAttacker;
			_playerInventory = playerInventory;
		}

		public void TickInput(float delta, bool isInteracting, bool canDoCombo)
		{
			MoveInput();
			HandleInteractInput();
			HandleRollInput(delta);
			HandleAttackInput(isInteracting, canDoCombo);
			HandleQuickSlotsInput();
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
			_rollInput = CheckRollInput();

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

			bool CheckRollInput() => _inputActions.PlayerActions.Roll.phase == InputActionPhase.Performed;
		}

		private void HandleInteractInput() => _interactInput = CheckInput(_inputActions.PlayerActions.Interact);

		private void HandleAttackInput(bool isInteracting, bool canDocombo)
		{
			_rightLightAttackInput = CheckInput(_inputActions.PlayerActions.LightAttack);
			_rightHeavyAttackInput = CheckInput(_inputActions.PlayerActions.HeavyAttack);

			PerformAttack(_rightLightAttackInput, _rightHeavyAttackInput, isInteracting, canDocombo, _playerInventory.RightWeapon);
		}

		private void HandleQuickSlotsInput()
		{
			_quickSlotRightInput = CheckInput(_inputActions.PlayerActions.DPadRight);
			_quickSlotLeftInput = CheckInput(_inputActions.PlayerActions.DPadLeft);

			if(_quickSlotRightInput) _playerInventory.ChangeWeaponInSlot();
			else if(_quickSlotLeftInput) _playerInventory.ChangeWeaponInSlot(true);
		}

		private bool CheckInput(InputAction action) => action.WasPerformedThisFrame();

		private void PerformAttack(bool lightAttackInput, bool heavyAttackInput, bool isInteracting, bool canDoCombo, WeaponItem weapon)
		{
			if(lightAttackInput || heavyAttackInput)
			{
				if(canDoCombo)
				{
					_comboFlag = true;
					_playerAttacker.HandleWeaponCombo(weapon);
					_comboFlag = false;
				}
				else
				{
					if(isInteracting || canDoCombo) return;

					if(lightAttackInput) _playerAttacker.HandleLightAttack(weapon);
					else if(heavyAttackInput) _playerAttacker.HandleHeavyAttack(weapon);
				}
			}
		}
	}
}
