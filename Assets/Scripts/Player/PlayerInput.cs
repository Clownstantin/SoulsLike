using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoulsLike
{
	public class PlayerInput : MonoBehaviour
	{
		private PlayerControls _inputActions = default;

		private Vector2 _movementInput = default;
		private Vector2 _cameraInput = default;

		private float _horizontal = default;
		private float _vertical = default;
		private float _moveAmount = default;
		private float _mouseX = default;
		private float _mouseY = default;

		private bool _interactInput = default;

		private bool _sprintFlag = default;
		private bool _rollFlag = default;
		private float _rollInputTimer = default;

		public float Horizontal => _horizontal;
		public float Vertical => _vertical;
		public float MoveAmount => _moveAmount;
		public float MouseX => _mouseX;
		public float MouseY => _mouseY;

		public bool RollFlag => _rollFlag;
		public bool SprintFlag => _sprintFlag;
		public bool InteractInput => _interactInput;

		private void OnEnable()
		{
			_inputActions ??= new PlayerControls();
			_inputActions.PlayerMovement.Movement.performed += SetMovementInput;
			_inputActions.PlayerMovement.Camera.performed += SetCameraInput;
			_inputActions.Enable();
		}

		private void OnDisable()
		{
			_inputActions.PlayerMovement.Movement.performed -= SetMovementInput;
			_inputActions.PlayerMovement.Camera.performed -= SetCameraInput;
			_inputActions.Disable();
		}

		public void TickInput(float delta, bool isInteracting, bool canDoCombo)
		{
			HandleMoveInput();
			HandleInteractInput();
			HandleRollInput(delta);
			HandleAttackInput(isInteracting, canDoCombo);
			HandleQuickSlotsInput();
			HandleJumpInput(isInteracting);
			HandleInventoryInput();
			HandleLockOnInput();
		}

		public void ResetFlags()
		{
			_rollFlag = false;
			_sprintFlag = false;
		}

		private void HandleMoveInput()
		{
			_horizontal = _movementInput.x;
			_vertical = _movementInput.y;
			_moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
			_mouseX = _cameraInput.x;
			_mouseY = _cameraInput.y;
		}

		private void SetCameraInput(InputAction.CallbackContext c) => _cameraInput = c.ReadValue<Vector2>();

		private void SetMovementInput(InputAction.CallbackContext m) => _movementInput = m.ReadValue<Vector2>();

		private void HandleRollInput(float delta)
		{
			bool rollInput = CheckRollInput();

			if(rollInput)
			{
				_rollInputTimer += delta;
				_sprintFlag = true;
			}
			else
			{
				if(_rollInputTimer is > 0 and < 0.5f)
				{
					_sprintFlag = false;
					_rollFlag = true;
				}
				_rollInputTimer = 0;
			}

			bool CheckRollInput() => _inputActions.PlayerActions.Roll.phase == InputActionPhase.Performed;
		}

		private void HandleAttackInput(bool isInteracting, bool canDocombo)
		{
			bool rightLightAttackInput = CheckInput(_inputActions.PlayerActions.LightAttack);
			bool rightHeavyAttackInput = CheckInput(_inputActions.PlayerActions.HeavyAttack);

			if(!rightLightAttackInput && !rightHeavyAttackInput) return;
			this.TriggerEvent(new RightWeaponAttack(rightLightAttackInput, rightHeavyAttackInput, isInteracting, canDocombo));
		}

		private void HandleQuickSlotsInput()
		{
			bool quickSlotRightInput = CheckInput(_inputActions.PlayerQuickSlots.DPadRight);
			bool quickSlotLeftInput = CheckInput(_inputActions.PlayerQuickSlots.DPadLeft);

			if(!quickSlotRightInput && !quickSlotLeftInput) return;
			this.TriggerEvent(new WeaponSwitchEvent(quickSlotRightInput, quickSlotLeftInput));
		}

		private void HandleInteractInput() => _interactInput = CheckInput(_inputActions.PlayerActions.Interact);

		private void HandleJumpInput(bool isInteracting)
		{
			bool jumpInput = CheckInput(_inputActions.PlayerActions.Jump);
			if(jumpInput && !isInteracting) this.TriggerEvent(new JumpEvent(_moveAmount));
		}

		private void HandleInventoryInput()
		{
			bool inventoryInput = CheckInput(_inputActions.PlayerActions.Inventory);
			if(inventoryInput) this.TriggerEvent(new ToggleSelectionMenuEvent());
		}

		private void HandleLockOnInput()
		{
			bool lockOnInput = CheckInput(_inputActions.PlayerActions.LockOn);
			bool lockOnLeftInput = CheckInput(_inputActions.PlayerActions.LockSwitchLeft);
			bool lockOnRightInput = CheckInput(_inputActions.PlayerActions.LockSwitchRight);

			if(lockOnInput) this.TriggerEvent(new LockOnTargetEvent());

			if(!lockOnLeftInput && !lockOnRightInput) return;
			this.TriggerEvent(new SwitchOnTargetEvent(lockOnLeftInput, lockOnRightInput));
		}

		private static bool CheckInput(InputAction action) => action.WasPerformedThisFrame();
	}
}
