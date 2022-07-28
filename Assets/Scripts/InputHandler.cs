using UnityEngine;

namespace SoulsLike
{
	public class InputHandler : MonoBehaviour
	{
		private PlayerControls _inputActions;
		private CameraHandler _cameraHandler;

		private Vector2 _movementInput;
		private Vector2 _cameraInput;

		public float horizontal;
		public float vertical;
		public float moveAmount;
		public float mouseX;
		public float mouseY;

		#region Monobehavior
		private void Awake() => _cameraHandler = CameraHandler.instance;

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

		private void FixedUpdate()
		{
			float delta = Time.fixedDeltaTime;

			if(_cameraHandler != null)
			{
				_cameraHandler.FollowTarget(delta);
				_cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
			}
		}

		private void OnDisable() => _inputActions.Disable();
		#endregion

		public void TickInput(float delta)
		{
			MoveInput(delta);
		}

		private void MoveInput(float delta)
		{
			horizontal = _movementInput.x;
			vertical = _movementInput.y;
			moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
			mouseX = _cameraInput.x;
			mouseY = _cameraInput.y;
		}
	}
}
