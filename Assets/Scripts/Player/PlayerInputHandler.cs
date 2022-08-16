using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoulsLike
{
	public class PlayerInputHandler : MonoBehaviour
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
		private bool _rollInput = default;
		private bool _rightLightAttackInput = default;
		private bool _rightHeavyAttackInput = default;

		private bool _quickSlotUpInput = default;
		private bool _quickSlotDownInput = default;
		private bool _quickSlotLeftInput = default;
		private bool _quickSlotRightInput = default;

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
			MoveInput();
			HandleInteractInput();
			HandleRollInput(delta);
			HandleAttackInput(isInteracting, canDoCombo);
			HandleQuickSlotsInput();
		}

		public void ResetFlags()
		{
			_interactInput = false;
			_rollFlag = false;
			_sprintFlag = false;

			_rightLightAttackInput = false;
			_rightHeavyAttackInput = false;

			_quickSlotUpInput = false;
			_quickSlotDownInput = false;
			_quickSlotLeftInput = false;
			_quickSlotRightInput = false;
		}

		private void MoveInput()
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
			_rollInput = CheckRollInput();

			if(_rollInput)
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

		private void HandleInteractInput() => _interactInput = CheckInput(_inputActions.PlayerActions.Interact);

		private void HandleAttackInput(bool isInteracting, bool canDocombo)
		{
			_rightLightAttackInput = CheckInput(_inputActions.PlayerActions.LightAttack);
			_rightHeavyAttackInput = CheckInput(_inputActions.PlayerActions.HeavyAttack);

			if(!_rightLightAttackInput && !_rightHeavyAttackInput) return;

			var conditions = (_rightLightAttackInput, _rightHeavyAttackInput, isInteracting, canDocombo);
			this.TriggerEvent(EventID.OnRightWeaponAttack, conditions);
		}

		private void HandleQuickSlotsInput()
		{
			_quickSlotRightInput = CheckInput(_inputActions.PlayerActions.DPadRight);
			_quickSlotLeftInput = CheckInput(_inputActions.PlayerActions.DPadLeft);

			if(!_quickSlotRightInput && !_quickSlotLeftInput) return;

			var conditions = (_quickSlotRightInput, _quickSlotLeftInput);
			this.TriggerEvent(EventID.OnWeaponSwitch, conditions);
		}

		private static bool CheckInput(InputAction action) => action.WasPerformedThisFrame();
	}
}