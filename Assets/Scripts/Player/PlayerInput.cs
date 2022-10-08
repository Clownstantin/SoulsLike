using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoulsLike
{
	public class PlayerInput : MonoBehaviour, IEventListener, IEventSender
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
			HandleTwoHandInput();
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
			bool rollInput = CheckRollPhase(_inputActions.PlayerActions.Roll);

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

			bool CheckRollPhase(InputAction action) => action.phase == InputActionPhase.Performed;
		}

		private void HandleAttackInput(bool isInteracting, bool canDocombo)
		{
			bool rightLightAttackInput = CheckInputPerformed(_inputActions.PlayerActions.LightAttack);
			bool rightHeavyAttackInput = CheckInputPerformed(_inputActions.PlayerActions.HeavyAttack);

			if(!rightLightAttackInput && !rightHeavyAttackInput) return;
			this.TriggerEvent(new RightWeaponAttack(rightLightAttackInput, rightHeavyAttackInput, isInteracting, canDocombo));
		}

		private void HandleQuickSlotsInput()
		{
			bool quickSlotRightInput = CheckInputPerformed(_inputActions.PlayerQuickSlots.DPadRight);
			bool quickSlotLeftInput = CheckInputPerformed(_inputActions.PlayerQuickSlots.DPadLeft);

			if(!quickSlotRightInput && !quickSlotLeftInput) return;
			this.TriggerEvent(new WeaponSwitchEvent(quickSlotRightInput, quickSlotLeftInput));
		}

		private void HandleInteractInput() => _interactInput = CheckInputPerformed(_inputActions.PlayerActions.Interact);

		private void HandleJumpInput(bool isInteracting)
		{
			if(CheckInputPerformed(_inputActions.PlayerActions.Jump) && !isInteracting) this.TriggerEvent(new JumpEvent(_moveAmount));
		}

		private void HandleInventoryInput()
		{
			if(CheckInputPerformed(_inputActions.PlayerActions.OpenMenu)) this.TriggerEvent(new ToggleSelectionMenuEvent());
		}

		private void HandleLockOnInput()
		{
			bool lockOnLeftInput = CheckInputPerformed(_inputActions.PlayerActions.LockSwitchLeft);
			bool lockOnRightInput = CheckInputPerformed(_inputActions.PlayerActions.LockSwitchRight);

			if(CheckInputPerformed(_inputActions.PlayerActions.LockOn)) this.TriggerEvent(new LockOnTargetEvent());

			if(!lockOnLeftInput && !lockOnRightInput) return;
			this.TriggerEvent(new SwitchOnTargetEvent(lockOnLeftInput, lockOnRightInput));
		}

		private void HandleTwoHandInput()
		{
			if(CheckInputPerformed(_inputActions.PlayerActions.TwoHand)) this.TriggerEvent(new ToggleTwoHandEvent());
		}

		private static bool CheckInputPerformed(InputAction action) => action.WasPerformedThisFrame();
	}
}