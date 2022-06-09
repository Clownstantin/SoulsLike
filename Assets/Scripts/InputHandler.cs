using UnityEngine;

namespace SoulsLike
{
	public class InputHandler : MonoBehaviour
	{
		private float _horizontal;
		private float _vertical;
		private float _moveAmount;
		private float _mouseX;
		private float _mouseY;

		private bool _rollButtonPressed;
		private bool _rollFlag;
		private float _rollInputTimer;
		private bool _sprintFlag;

		private PlayerControls _playerControls;

		private Vector2 _movement;
		private Vector2 _cameraInput;

		public float MoveAmount => _moveAmount;
		public float Horizontal => _horizontal;
		public float Vertical => _vertical;
		public float MouseX => _mouseX;
		public float MouseY => _mouseY;
		public bool RollFlag => _rollFlag;
		public bool RollButtonPressed => _rollButtonPressed;
		public bool SprintFlag => _sprintFlag;


		private void OnEnable()
		{
			_playerControls ??= new PlayerControls();
			_playerControls.PlayerMovement.Movement.performed += m => _movement = m.ReadValue<Vector2>();
			_playerControls.PlayerMovement.Camera.performed += c => _cameraInput = c.ReadValue<Vector2>();
			_playerControls.Enable();
		}

		private void OnDisable() => _playerControls.Disable();

		public void TickInput(float delta)
		{
			MoveInput(delta);
			HandleRollInput(delta);
		}

		public void SetRollFlag(bool rollFlag) => _rollFlag = rollFlag;

		public void SetSprintFlag(bool sprintFlag) => _sprintFlag = sprintFlag;

		private void MoveInput(float delta)
		{
			_horizontal = _movement.x;
			_vertical = _movement.y;
			_moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
			_mouseX = _cameraInput.x;
			_mouseY = _cameraInput.y;
		}

		private void HandleRollInput(float delta)
		{
			_rollButtonPressed = _playerControls.PlayerActions.Roll.IsPressed();

			if(_rollButtonPressed)
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
	}
}
