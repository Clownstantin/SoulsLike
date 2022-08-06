using UnityEngine;

namespace SoulsLike
{
	public class InputHandler : MonoBehaviour
	{
		private PlayerControls _inputActions;
		private PlayerManager _playerManager;
		private PlayerAttacker _playerAttacker;
		private PlayerInventory _playerInventory;

		private Vector2 _movementInput;
		private Vector2 _cameraInput;

		private float _horizontal;
		private float _vertical;
		private float _moveAmount;
		private float _mouseX;
		private float _mouseY;

		private bool _rightLightAttackInput;
		private bool _rightHeavyAttackInput;
		private bool _rollInput;
		private bool _quickSlotUpInput;
		private bool _quickSlotDownInput;
		private bool _quickSlotLeftInput;
		private bool _quickSlotRightInput;

		private bool _sprintFlag;
		private bool _comboFlag;
		private bool _rollFlag;
		private float _rollInputTimer;

		public float Horizontal => _horizontal;
		public float Vertical => _vertical;
		public float MoveAmount => _moveAmount;
		public float MouseX => _mouseX;
		public float MouseY => _mouseY;

		public bool RollFlag => _rollFlag;
		public bool SprintFlag => _sprintFlag;
		public bool ComboFlag => _comboFlag;

		private void OnEnable()
		{
			if(_inputActions == null)
			{
				_inputActions = new PlayerControls();
				_inputActions.PlayerMovement.Movement.performed += m => _movementInput = m.ReadValue<Vector2>();
				_inputActions.PlayerMovement.Camera.performed += c => _cameraInput = c.ReadValue<Vector2>();
			}

			_inputActions.Enable();
		}

		private void OnDisable() => _inputActions.Disable();

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
		}

		public void ResetFlags(bool state = false)
		{
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

		private void HandleAttackInput()
		{
			_rightLightAttackInput = _inputActions.PlayerActions.LightAttack.WasPerformedThisFrame();
			_rightHeavyAttackInput = _inputActions.PlayerActions.HeavyAttack.WasPerformedThisFrame();

			PerformAttack(_rightLightAttackInput, _rightHeavyAttackInput, _playerInventory.rightWeapon);
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

		private void HandleQuickSlotsInput()
		{
			_quickSlotRightInput = _inputActions.PlayerActions.DPadRight.WasPerformedThisFrame();
			_quickSlotLeftInput = _inputActions.PlayerActions.DPadLeft.WasPerformedThisFrame();

			if(_quickSlotRightInput) _playerInventory.ChangeWeaponInSlot();
			else if(_quickSlotLeftInput) _playerInventory.ChangeWeaponInSlot(true);
		}
	}
}
