using UnityEngine;

namespace SoulsLike
{
	public class InputHandler : MonoBehaviour
	{
		private PlayerControls _inputActions;

		private Vector2 _movementInput;
		private Vector2 _cameraInput;

		public float horizontal;
		public float vertical;
		public float moveAmount;
		public float mouseX;
		public float mouseY;

		public bool b_Input;
		public bool rollFlag;
		public bool sprintFlag;
		public float rollInputTimer;

		#region Monobehavior
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
		#endregion

		public void TickInput(float delta)
		{
			MoveInput(delta);
			HandleRollInput(delta);
		}

		private void MoveInput(float delta)
		{
			horizontal = _movementInput.x;
			vertical = _movementInput.y;
			moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
			mouseX = _cameraInput.x;
			mouseY = _cameraInput.y;
		}

		private void HandleRollInput(float delta)
		{
			b_Input = _inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

			if(b_Input)
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
	}
}
