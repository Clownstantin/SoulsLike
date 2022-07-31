using UnityEngine;

namespace SoulsLike
{
	public class InputHandler : MonoBehaviour
	{
		private PlayerControls _inputActions;
		private PlayerAttacker _playerAttacker;
		private PlayerInventory _playerInventory;

		private Vector2 _movementInput;
		private Vector2 _cameraInput;

		public float horizontal;
		public float vertical;
		public float moveAmount;
		public float mouseX;
		public float mouseY;

		public bool rightLightAttackInput;
		public bool rightHeavyAttackInput;
		public bool rollInput;

		public bool rollFlag;
		public bool sprintFlag;
		public float rollInputTimer;

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

		public void Init(PlayerAttacker playerAttacker, PlayerInventory playerInventory)
		{
			_playerAttacker = playerAttacker;
			_playerInventory = playerInventory;
		}

		public void TickInput(float delta)
		{
			MoveInput();
			HandleRollInput(delta);
			HandleAttackInput();
		}

		private void MoveInput()
		{
			horizontal = _movementInput.x;
			vertical = _movementInput.y;
			moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
			mouseX = _cameraInput.x;
			mouseY = _cameraInput.y;
		}

		private void HandleRollInput(float delta)
		{
			rollInput = _inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

			if(rollInput)
			{
				rollInputTimer += delta;
				sprintFlag = true;
			}
			else
			{
				if(rollInputTimer > 0 && rollInputTimer < 0.5f)
				{
					sprintFlag = false;
					rollFlag = true;
				}

				rollInputTimer = 0;
			}
		}

		private void HandleAttackInput()
		{
			rightLightAttackInput = _inputActions.PlayerActions.LightAttack.IsPressed();
			rightHeavyAttackInput = _inputActions.PlayerActions.HeavyAttack.IsPressed();

			if(rightLightAttackInput) _playerAttacker.HandleLightAttack(_playerInventory.rightWeapon);
			if(rightHeavyAttackInput) _playerAttacker.HandleHeavyAttack(_playerInventory.rightWeapon);
		}
	}
}
