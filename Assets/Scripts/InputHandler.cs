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

		private PlayerControls _playerControls;
		private CameraHandler _cameraHandler;

		private Vector2 _movement;
		private Vector2 _cameraInput;

		public float MoveAmount => _moveAmount;
		public float Horizontal => _horizontal;
		public float Vertical => _vertical;
		public float MouseX => _mouseX;
		public float MouseY => _mouseY;

		private void Awake() => _cameraHandler = CameraHandler.Instance;

		private void OnEnable()
		{
			_playerControls ??= new PlayerControls();
			_playerControls.PlayerMovement.Movement.performed += m => _movement = m.ReadValue<Vector2>();
			_playerControls.PlayerMovement.Camera.performed += c => _cameraInput = c.ReadValue<Vector2>();
			_playerControls.Enable();
		}

		private void FixedUpdate()
		{
			float delta = Time.fixedDeltaTime;

			if(_cameraHandler == null) return;

			_cameraHandler.FollowTarget(delta);
			_cameraHandler.HandleCameraRotation(delta, _mouseX, _mouseY);
		}

		private void OnDisable() => _playerControls.Disable();

		public void TickInput(float delta) => MoveInput(delta);

		private void MoveInput(float delta)
		{
			_horizontal = _movement.x;
			_vertical = _movement.y;
			_moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
			_mouseX = _cameraInput.x;
			_mouseY = _cameraInput.y;
		}
	}
}
