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

		public float horizontal;
		public float vertical;
		public float moveAmount;
		public float mouseX;
		public float mouseY;

		public bool rightLightAttackInput;
		public bool rightHeavyAttackInput;
		public bool rollInput;
		public bool d_Pad_Up;
		public bool d_Pad_Down;
		public bool d_Pad_Left;
		public bool d_Pad_Right;

		public bool rollFlag;
		public bool sprintFlag;
		public bool comboFlag;
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
			rightLightAttackInput = _inputActions.PlayerActions.LightAttack.WasPerformedThisFrame();
			rightHeavyAttackInput = _inputActions.PlayerActions.HeavyAttack.WasPerformedThisFrame();

			PerformAttack(rightLightAttackInput, rightHeavyAttackInput, _playerInventory.rightWeapon);
		}

		private void PerformAttack(bool lightAttack, bool heavyAttack, WeaponItem weapon)
		{
			if(lightAttack || heavyAttack)
			{
				if(_playerManager.canDoCombo)
				{
					comboFlag = true;
					_playerAttacker.HandleWeaponCombo(weapon);
					comboFlag = false;
				}
				else
				{
					if(_playerManager.isInteracting || _playerManager.canDoCombo) return;

					if(lightAttack) _playerAttacker.HandleLightAttack(weapon);
					else if(heavyAttack) _playerAttacker.HandleHeavyAttack(weapon);
				}
			}
		}

		private void HandleQuickSlotsInput()
		{
			d_Pad_Right = _inputActions.PlayerActions.DPadRight.WasPerformedThisFrame();
			d_Pad_Left = _inputActions.PlayerActions.DPadLeft.WasPerformedThisFrame();

			if(d_Pad_Right) _playerInventory.ChangeWeaponInSlot();
			else if(d_Pad_Left) _playerInventory.ChangeWeaponInSlot(true);
		}
	}
}
